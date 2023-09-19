using UnityEngine;

public class BacteriaBehavior : MonoBehaviour
{
    public WaterQualityParameters waterQualityParameters;
    public ResourcePool resourcePool; // Add this line

    public void UpdateBacteria()
    {
        ApplyBacteriaEffects();
        SimulateBacteriaPopulation();
        BacterialResourceCycling();
    }

    private void ApplyBacteriaEffects()
    {
        // For example, adjust nutrient levels and affect other organisms' health
        float ammoniaEffect = 0.1f; // Example effect on ammonia levels
        float pHEffect = -0.05f;    // Example effect on pH levels

        waterQualityParameters.AdjustAmmoniaLevel(ammoniaEffect);
        waterQualityParameters.AdjustpHLevel(pHEffect);
    }

    private void SimulateBacteriaPopulation()
    {
        // Simulate population growth based on available nutrients
        float growthRate = 0.05f; // Example growth rate

        float nutrientAvailability = resourcePool.GetNutrientAvailability();
        float growth = nutrientAvailability * growthRate * Time.deltaTime;

        waterQualityParameters.AdjustBacteriaPopulation(growth);
    }

    private void BacterialResourceCycling()
    {
        // Convert waste products into nutrients
        float conversionRate = 0.2f; // Example conversion rate

        float wasteLevel = waterQualityParameters.GetWasteLevel();
        float convertedNutrients = wasteLevel * conversionRate * Time.deltaTime;

        resourcePool.AdjustNutrientAvailability(convertedNutrients);
        waterQualityParameters.AdjustWasteLevel(-convertedNutrients);
    }
}
