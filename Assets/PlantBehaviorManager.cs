using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviorManager : MonoBehaviour
{
    public WaterQualityParameters waterQuality;
    public JSONLoader jsonLoader;
    public List<Plant> allPlants = new List<Plant>();

    private void Start()
    {
        JSONLoader jsonLoader = FindObjectOfType<JSONLoader>();
        if (jsonLoader != null)
        {
            allPlants = new List<Plant>(jsonLoader.plantData.plants);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Plant plant = other.GetComponent<Plant>();
            PlantTraits plantTraits = other.GetComponent<PlantTraits>();

            if (plant != null || plantTraits != null)
            {
                if (plant != null)
                {
                    waterQuality.AdjustAmmoniaLevel(plant.ammoniaEffect);
                    waterQuality.AdjustNitrateLevel(plant.nitrateEffect);
                    waterQuality.AdjustpHLevel(plant.pHEffect);
                }
                else
                {
                    waterQuality.AdjustAmmoniaLevel(plantTraits.AmmoniaEffect);
                    waterQuality.AdjustNitrateLevel(plantTraits.NitrateEffect);
                    waterQuality.AdjustpHLevel(plantTraits.pHEffect);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Plant plant = other.GetComponent<Plant>();
            PlantTraits plantTraits = other.GetComponent<PlantTraits>();

            if (plant != null || plantTraits != null)
            {
                if (plant != null)
                {
                    waterQuality.AdjustAmmoniaLevel(-plant.ammoniaEffect);
                    waterQuality.AdjustNitrateLevel(-plant.nitrateEffect);
                    waterQuality.AdjustpHLevel(-plant.pHEffect);
                }
                else
                {
                    waterQuality.AdjustAmmoniaLevel(-plantTraits.AmmoniaEffect);
                    waterQuality.AdjustNitrateLevel(-plantTraits.NitrateEffect);
                    waterQuality.AdjustpHLevel(-plantTraits.pHEffect);
                }
            }
        }
    }

    public void SimulatePlantBehavior()
    {
        foreach (Plant plant in allPlants)
        {
            SimulatePlantGrowth(plant);
            SimulateNutrientUptake(plant);
            SimulateLightSensitivity(plant);
        }
    }

    public void SimulatePlantGrowth(Plant plant)
    {
        // Calculate and adjust plant growth based on various factors
        float growthRate = CalculatePlantGrowthRate(plant);
        plant.Size += growthRate * Time.deltaTime;
    }

    public void SimulateNutrientUptake(Plant plant)
    {
        // Calculate and adjust nutrient uptake by the plant
        float nutrientUptakeRate = CalculateNutrientUptakeRate(plant);
        waterQuality.ReduceNutrientLevels(nutrientUptakeRate, Time.deltaTime);
    }

    public void SimulateLightSensitivity(Plant plant)
    {
        // Calculate and adjust plant behavior based on light sensitivity
        float lightIntensity = CalculateLightIntensity(plant.Position);

        if (lightIntensity > plant.LightThreshold)
        {
            // Perform actions for sufficient light
        }
        else
        {
            // Adjust behavior for low light
        }
    }

    private float CalculatePlantGrowthRate(Plant plant)
    {
        // Calculate growth rate based on nutrient levels, temperature, etc.
        float growthRate = 0.0f; // Implement your calculation logic here
        return growthRate;
    }

    private float CalculateNutrientUptakeRate(Plant plant)
    {
        // Calculate nutrient uptake rate based on plant properties and environment
        float nutrientUptakeRate = plant.nutrientUptakeRate; // Implement your calculation logic here
        return nutrientUptakeRate;
    }

    private float CalculateLightIntensity(Vector3 plantPosition)
    {
        // Calculate light intensity based on plant position and environment
        float lightIntensity = 0.0f; // Implement your calculation logic here
        return lightIntensity;
    }

    public void AdjustPlantGrowthBasedOnSubstrate(Substrate substrate, float currentTemperature)
    {
        foreach (Plant plant in allPlants)
        {
            // Calculate growth adjustment based on substrate properties
            float growthAdjustment = CalculateGrowthAdjustmentFromSubstrate(plant, substrate);

            // Adjust plant growth rate based on temperature and growth adjustment
            float adjustedGrowthRate = plant.CalculateAdjustedGrowthRate(currentTemperature) + growthAdjustment;
            plant.Size += adjustedGrowthRate * Time.deltaTime;
        }
    }

    private float CalculateGrowthAdjustmentFromSubstrate(Plant plant, Substrate substrate)
    {
        // Calculate growth adjustment based on substrate properties
        float pHEffect = CalculatePHEffect(plant, substrate);
        float nutrientInteractionEffect = CalculateNutrientInteractionEffect(plant, substrate);
        float overallGrowthAdjustment = pHEffect + nutrientInteractionEffect;
        return overallGrowthAdjustment;
    }

    private float CalculatePHEffect(Plant plant, Substrate substrate)
    {
        // Calculate pH effect on plant growth based on substrate and plant properties
        float preferredPH = plant.pH[0];
        float currentPH = waterQuality.GetpH();
        float pHEffect = Mathf.Clamp01(1.0f - Mathf.Abs(currentPH - preferredPH) / 2.0f);
        return pHEffect;
    }

    private float CalculateNutrientInteractionEffect(Plant plant, Substrate substrate)
    {
        // Calculate nutrient interaction effect based on substrate and plant properties
        float substrateNitrogen = substrate.interactions.water.effectOnNitrate;
        float substratePhosphorus = substrate.phosphorus_ppm[0];
        float substratePotassium = substrate.potassium_ppm[0];

        float nutrientInteractionEffect = (plant.nitrate_ppm[0] - substrateNitrogen) * 0.1f
                                         + (plant.phosphorus_ppm[0] - substratePhosphorus) * 0.05f
                                         + (plant.potassium_ppm[0] - substratePotassium) * 0.05f;

        return nutrientInteractionEffect;
    }

    public void OnPlantDeath(GameObject plantObject)
    {
        Plant plant = plantObject.GetComponent<Plant>();
        if (plant != null)
        {
            allPlants.Remove(plant);
            waterQuality.AdjustAmmoniaLevel(-plant.ammoniaEffect);
            waterQuality.AdjustNitrateLevel(-plant.nitrateEffect);
            waterQuality.AdjustpHLevel(-plant.pHEffect);
            Destroy(plantObject);
        }
    }


}
