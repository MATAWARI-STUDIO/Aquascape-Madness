using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public LayerMask fishLayer;  // Define this in the Unity editor to specify the layer your fish objects are on

    // Predator properties
    private Transform targetPrey;
    public float chaseSpeed = 0.5f;
    public float chaseRadius = 5.0f;
    public float eatDistance = 0.5f;

    public bool isPrey;
    public float fleeSpeed = 0.6f;
    public float dangerRadius = 4.0f;

    public Fish fish;
    public FishData fishData;
    public WaterQualityParameters waterQualityParameters;
    public GameObject deadCreatureIndicator;
    public ResourcePool resourcePool;
    public FishInfoPanel fishInfoPanel;
    private JSONLoader jsonLoader;

    public float health = 100.0f;
    public float nutritionValue = 50.0f;

    private bool isCollidingWithWater = false;

    // Growth cooldown
    private float lastGrowthTime = 0.0f;
    private const float growthCooldown = 10.0f;

    private void Start()
    {
        jsonLoader = FindObjectOfType<JSONLoader>();

        if (fish.isHerbivorous || fish.predatorFoodAmount > 0)
        {
            fishInfoPanel = FindObjectOfType<FishInfoPanel>();
        }
    }

    private void Update()
    {
        if (isCollidingWithWater)
        {
            ApplyWaterEffects();
            ApplyBacterialEffects();

            if (fish.predatorFoodAmount > 0)
            {
                ChasePrey();
            }
            if (isPrey)
            {
                FleeFromPredator();
            }

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void ApplyWaterEffects()
    {
        float pHValue = waterQualityParameters.GetpH();
        float ammoniaValue = waterQualityParameters.GetAmmoniaLevel();
        float nitriteValue = waterQualityParameters.GetNitriteLevel();
        float nitrateValue = waterQualityParameters.GetNitrateLevel();
        float o2ProductionRate = waterQualityParameters.GetCurrentOxygen();
        float currentTemperature = waterQualityParameters.GetTemperature();

        float ammoniaEffect = ammoniaValue * 0.05f;
        float nitrateEffect = nitrateValue * 0.02f;

        health -= ammoniaEffect;
        health -= nitrateEffect;

        if (fish.isHerbivorous)
        {
            resourcePool.AdjustNutrientAvailability(-0.1f);
            waterQualityParameters.AdjustNitrateLevel(-0.05f);
        }
    }


    public void DecreaseHealth(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private float CalculateLightIntensityFactor()
    {
        float totalIntensityFactor = 0f;
        if (jsonLoader != null && jsonLoader.lightData != null)
        {
            foreach (var light in jsonLoader.lightData.lights)
            {
                totalIntensityFactor += light.intensity_adjustment_factor;
            }
        }
        return totalIntensityFactor;
    }

    public void ApplyWaterEffects(float pHValue, float ammoniaValue, float nitriteValue, float nitrateValue, float o2ProductionRate, float currentTemperature)
    {
        float ammoniaEffect = ammoniaValue * 0.05f;
        float nitrateEffect = nitrateValue * 0.02f;

        health -= ammoniaEffect;
        health -= nitrateEffect;

        if (fish.isHerbivorous)
        {
            resourcePool.AdjustNutrientAvailability(-0.1f);
            waterQualityParameters.AdjustNitrateLevel(-0.05f);
        }
    }

    private void ApplyBacterialEffects()
    {
        float harmfulBacteriaThreshold = waterQualityParameters.MaxBacteriaPopulation * 0.8f;
        if (waterQualityParameters.BacteriaPopulation > harmfulBacteriaThreshold)
        {
            health -= 5;
        }
    }

    public void Grow()
    {
        if (Time.time - lastGrowthTime > growthCooldown)
        {
            if (health > 50 && nutritionValue > 30)  // Made the nutrition condition a bit stricter
            {
                health += 1.0f;  // Reduced the rate of health growth
                transform.localScale += new Vector3(0.005f, 0.005f, 0.005f);  // Reduced the physical growth rate
                lastGrowthTime = Time.time;
            }
        }
    }

    public void Eat()
    {
        if (waterQualityParameters.AlgaePopulation > 10)
        {
            nutritionValue += 5.0f;
            waterQualityParameters.AdjustAlgaePopulation(-5.0f);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);

        if (deadCreatureIndicator != null)
        {
            Instantiate(deadCreatureIndicator, transform.position, Quaternion.identity);
        }

        Debug.Log("Creature died: " + fish.name);

        if (fish.isHerbivorous)
        {
            resourcePool.AdjustNutrientAvailability(nutritionValue);
        }
        else if (fish.predatorFoodAmount > 0)
        {
            waterQualityParameters.AdjustBacteriaPopulation(50f);
        }

        DecompositionManager.Instance.HandleDecomposition(nutritionValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & fishLayer) != 0)
        {
            if (fish.predatorFoodAmount > 0)
            {
                targetPrey = other.transform;

                // Decrease prey's health
                FishBehavior preyBehavior = other.gameObject.GetComponent<FishBehavior>();
                if (preyBehavior != null)
                {
                    preyBehavior.DecreaseHealth(20);
                }
            }
            else if (isPrey)
            {
                FleeFromPredator();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = false;
        }
    }

    public void EatFish(FishBehavior prey)
    {
        nutritionValue += prey.nutritionValue;
        Debug.Log($"{name} ate {prey.fish.name}!");
        prey.GetConsumed();
    }

    public void GetConsumed()
    {
        gameObject.SetActive(false);

        if (deadCreatureIndicator != null)
        {
            Instantiate(deadCreatureIndicator, transform.position, Quaternion.identity);
        }

        Debug.Log($"The fish {fish.name} has been consumed.");

        if (fish.isHerbivorous)
        {
            resourcePool.AdjustNutrientAvailability(nutritionValue);
        }
        else if (fish.predatorFoodAmount > 0)
        {
            waterQualityParameters.AdjustBacteriaPopulation(50f);
        }

        DecompositionManager.Instance.HandleDecomposition(nutritionValue);
    }

    public void Predation(PreyBehavior prey)
    {
        nutritionValue += prey.GetNutritionValue();
        prey.GetConsumed();

        if (prey.GetNutritionValue() > 20)
        {
            Grow();
        }
    }


    private void ChasePrey()
    {
        if (targetPrey == null)
        {
            // Search for prey
            Collider[] hits = Physics.OverlapSphere(transform.position, chaseRadius, fishLayer);
            foreach (var hit in hits)
            {
                FishBehavior prey = hit.GetComponent<FishBehavior>();
                if (prey && prey.isPrey)  // Ensure the target is actually a prey
                {
                    targetPrey = hit.transform;
                    break;
                }
            }
        }
        else
        {
            // Chase the target prey
            Vector3 directionToPrey = (targetPrey.position - transform.position).normalized;
            transform.position += directionToPrey * chaseSpeed * Time.deltaTime;

            float distanceToPrey = Vector3.Distance(transform.position, targetPrey.position);
            if (distanceToPrey <= eatDistance)
            {
                EatFish(targetPrey.GetComponent<FishBehavior>());
                targetPrey = null; // Reset target after consuming
            }
        }
    }

    private void FleeFromPredator()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, dangerRadius, fishLayer);
        foreach (var hit in hits)
        {
            FishBehavior otherFish = hit.GetComponent<FishBehavior>();
            if (otherFish && otherFish.fish.predatorFoodAmount > 0)
            {
                // Flee in the opposite direction of the predator
                Vector3 directionAwayFromPredator = (transform.position - hit.transform.position).normalized;
                transform.position += directionAwayFromPredator * fleeSpeed * Time.deltaTime;
            }
        }
    }

}
