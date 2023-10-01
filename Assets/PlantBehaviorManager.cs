using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviorManager : MonoBehaviour
{
    public WaterQualityParameters waterQuality;
    public JSONLoader jsonLoader;
    public List<Plant> allPlants = new List<Plant>();

    private void Start()
    {
        // Maintain the JSONLoader logic as you mentioned
        JSONLoader foundJsonLoader = FindObjectOfType<JSONLoader>();
        if (foundJsonLoader != null)
        {
            jsonLoader = foundJsonLoader;
            allPlants = new List<Plant>(jsonLoader.plantData.plants);
        }
    }

    // The following methods are for clarity and to ensure that each method has a single responsibility.

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider entered: " + other.name);
        HandleWaterQualityAdjustment(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider exited: " + other.name);
        HandleWaterQualityAdjustment(other, -1); // Reverse the effect when exiting
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
            // Actions for sufficient light can be added here
        }
        else
        {
            // Adjust behavior for low light can be added here
        }
    }

    // Further methods continue with logic split to maintain single responsibility per method

    private float CalculatePlantGrowthRate(Plant plant)
    {
        // Logic to calculate growth rate
        float growthRate = 0.0f;
        return growthRate;
    }

    private float CalculateNutrientUptakeRate(Plant plant)
    {
        float nutrientUptakeRate = plant.nutrientUptakeRate;
        return nutrientUptakeRate;
    }

    private float CalculateLightIntensity(Vector3 plantPosition)
    {
        float lightIntensity = 0.0f;
        return lightIntensity;
    }

    private void HandleWaterQualityAdjustment(Collider other, float multiplier = 1)
    {
        if (other.CompareTag("Water"))
        {
            Plant plant = other.GetComponent<Plant>();
            PlantTraits plantTraits = other.GetComponent<PlantTraits>();
            if (plant != null)
            {
                waterQuality.AdjustAmmoniaLevel(plant.ammoniaEffect * multiplier);
                waterQuality.AdjustNitrateLevel(plant.nitrateEffect * multiplier);
                waterQuality.AdjustpHLevel(plant.pHEffect * multiplier);
            }
            else if (plantTraits != null)
            {
                waterQuality.AdjustAmmoniaLevel(plantTraits.AmmoniaEffect * multiplier);
                waterQuality.AdjustNitrateLevel(plantTraits.NitrateEffect * multiplier);
                waterQuality.AdjustpHLevel(plantTraits.pHEffect * multiplier);
            }
        }
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
