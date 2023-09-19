using UnityEngine;

public class AlgaeBehavior : MonoBehaviour
{
    public WaterQualityParameters waterQualityParameters;
    public ResourcePool resourcePool;
    private float bloomThreshold = 2.0f;

    public void UpdateAlgae()
    {
        ApplyAlgaeEffects();
        SimulateAlgaeGrowth();
        AlgalResourceCycling();
        CheckForBloom();
    }

    private void ApplyAlgaeEffects()
    {
        float pHEffect = 0.05f * Time.deltaTime;
        float nitrateEffect = -0.1f * Time.deltaTime;

        waterQualityParameters.AdjustpHLevel(pHEffect);
        waterQualityParameters.AdjustNitrateLevel(nitrateEffect);
    }

    private void SimulateAlgaeGrowth()
    {
        float growthRate = 0.05f;

        float nutrientAvailability = resourcePool.GetNutrientAvailability();
        float lightAvailability = resourcePool.GetLightAvailability();
        float growth = nutrientAvailability * lightAvailability * growthRate * Time.deltaTime;

        waterQualityParameters.AdjustAlgaePopulation(growth);
    }

    private void AlgalResourceCycling()
    {
        float conversionRate = 0.2f;

        float algaePopulation = waterQualityParameters.GetAlgaePopulation();
        float convertedNutrients = algaePopulation * conversionRate * Time.deltaTime;

        resourcePool.AdjustNutrientAvailability(convertedNutrients);
        waterQualityParameters.AdjustAlgaePopulation(-convertedNutrients);
    }

    private void CheckForBloom()
    {
        if (waterQualityParameters.GetAlgaePopulation() >= bloomThreshold)
        {
            // Oxygen Depletion due to decomposition of dead algae
            float oxygenReduction = 0.2f * Time.deltaTime;
            waterQualityParameters.AdjustOxygenLevel(-oxygenReduction);

            // Toxin Production
            float toxinProductionRate = 0.05f * Time.deltaTime;
            waterQualityParameters.AdjustToxinLevel(toxinProductionRate);

            // Light Blockage
            float lightBlockageRate = 0.1f * Time.deltaTime;
            resourcePool.ReduceLightAvailability(lightBlockageRate);

        }
    }
}
