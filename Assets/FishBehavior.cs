using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public Fish fish;
    public FishData fishData;
    public WaterQualityParameters waterQualityParameters;
    public GameObject deadCreatureIndicator;
    public ResourcePool resourcePool;
    public FishInfoPanel fishInfoPanel;
    private JSONLoader jsonLoader;

    public const float MAX_HEALTH = 150.0f;
    public float health = 100.0f;
    public float nutritionValue = 50.0f;

    private bool isCollidingWithWater = false;

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
            float pHValue = waterQualityParameters.GetpH();
            float ammoniaValue = waterQualityParameters.GetAmmoniaLevel();
            float nitriteValue = waterQualityParameters.GetNitriteLevel();
            float nitrateValue = waterQualityParameters.GetNitrateLevel();
            float o2ProductionRate = waterQualityParameters.GetCurrentOxygen();
            float currentTemperature = waterQualityParameters.GetTemperature();

            ApplyWaterEffects(pHValue, ammoniaValue, nitriteValue, nitrateValue, o2ProductionRate, currentTemperature);
            ApplyBacterialEffects();

            float lightIntensityFactor = CalculateLightIntensityFactor();
            waterQualityParameters.ApplyFishEffect(fish, lightIntensityFactor);

            if (health <= 0)
            {
                Die();
            }
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
        if (health > 50 && nutritionValue > 25)
        {
            health = Mathf.Clamp(health + 2.0f, 0, MAX_HEALTH); // Clamp health
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    public void Eat()
    {
        if (waterQualityParameters.AlgaePopulation > 10)
        {
            nutritionValue += 3.0f; // Adjusted from 5.0f to 3.0f
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
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = true;
            Debug.Log("Creature immersed in water: " + fish.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = false;
            Debug.Log("Creature removed from water: " + fish.name);
        }
    }

    public void EatFish(FishBehavior prey)
    {
        nutritionValue += prey.nutritionValue * 0.7f; // Fish only gains 70% of the nutrition from the prey
        Debug.Log($"{name} ate {prey.fish.name}!");
        prey.GetConsumed();
    }

    public void GetConsumed()
    {
        // The fish is consumed, so it's no longer active in the scene.
        gameObject.SetActive(false);

        // If there's a dead creature indicator, instantiate it at the fish's position.
        if (deadCreatureIndicator != null)
        {
            Instantiate(deadCreatureIndicator, transform.position, Quaternion.identity);
        }

        // Log that the fish has been consumed.
        Debug.Log($"The fish {fish.name} has been consumed.");

        // Adjust the environmental variables based on the type of fish.
        if (fish.isHerbivorous)
        {
            resourcePool.AdjustNutrientAvailability(nutritionValue);
        }
        else if (fish.predatorFoodAmount > 0)
        {
            waterQualityParameters.AdjustBacteriaPopulation(50f);
        }

        // Handle the decomposition of the fish.
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
}
