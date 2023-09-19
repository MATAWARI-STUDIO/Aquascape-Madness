using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public Fish fish;
    public WaterQualityParameters waterQualityParameters;
    public GameObject deadCreatureIndicator;
    public ResourcePool resourcePool;
    public FishData fishData;
    public FishInfoPanel fishInfoPanel;

    public float health = 100.0f;
    public float nutritionValue = 50.0f;

    private bool isCollidingWithWater = false;

    private void Start()
    {
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

            // Pass the arguments correctly to ApplyWaterEffects on this instance
            ApplyWaterEffects(fishData, pHValue, ammoniaValue, nitriteValue, nitrateValue, o2ProductionRate, currentTemperature);

            ApplyBacterialEffects();

            if (health <= 0)
            {
                Die();
            }
        }
    }


    public void ApplyWaterEffects(FishData fishData, float pHValue, float ammoniaValue, float nitriteValue, float nitrateValue, float o2ProductionRate, float currentTemperature)
    {
        float ammoniaEffect = ammoniaValue * 0.05f; // Reduced the negative effect
        float nitrateEffect = nitrateValue * 0.02f; // Reduced the negative effect

        health -= ammoniaEffect;
        health -= nitrateEffect;

        if (fish.isHerbivorous)
        {
            resourcePool.AdjustNutrientAvailability(-0.1f); // Increased nutrient consumption
            waterQualityParameters.AdjustNitrateLevel(-0.05f); // Herbivorous fish consume nitrates directly
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
            health += 2.0f;
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
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

        // Add the following line to handle decomposition
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

    public void PredatorPreyInteraction(PredatorBehavior predator)
    {
        // Handle interactions with predators
    }

    public void Predation(PreyBehavior prey)
    {
        // Handle predation on prey
    }
}
