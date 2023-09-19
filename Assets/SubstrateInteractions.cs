using UnityEngine;

public class SubstrateInteractions : MonoBehaviour
{
    [Header("Substrate Interactions")]
    public float effectOnBacterialGrowth = 1.0f;
    public float effectOnLightReflection = 1.0f;
    public float effectOnWaterFlow = 1.0f;
    public float effectOnHeatRetention = 1.0f;
    public bool supportsAquaticFauna = false;
    public float effectOnToxicity = 1.0f;
    public float effectOnRootPenetration = 1.0f;

    [Header("Simulation Results")]
    public float effectOnWaterClarity = 0.0f;
    public float effectOnAlgaeGrowth = 0.0f;
    public float effectOnNutrientDistribution = 0.0f;
    public float effectOnWaterTemperatureStability = 0.0f;
    public float effectOnBiodiversity = 0.0f;
    public float effectOnAquaticLifeHealth = 0.0f;
    public float effectOnPlantHealth = 0.0f;
    public float effectOnPlantGrowth = 0.0f;

    private void Update()
    {
        // Simulate substrate interactions
        SimulateSubstrateInteractions();

        // Print simulation results (for testing purposes)
        PrintSimulationResults();
    }

    private void SimulateSubstrateInteractions()
    {
        // Add your simulation logic here based on the substrate interactions variables

        // If bacterial growth is promoted, it might reduce water clarity over time
        if (effectOnBacterialGrowth > 1.0f)
        {
            effectOnWaterClarity -= 0.1f;
        }

        // If the substrate reflects a lot of light, it might inhibit algae growth
        if (effectOnLightReflection > 0.8f)
        {
            effectOnAlgaeGrowth -= 0.2f;
        }

        // If the substrate affects water flow, it might also affect nutrient distribution
        if (effectOnWaterFlow < 0.5f)
        {
            effectOnNutrientDistribution -= 0.15f;
        }
        else if (effectOnWaterFlow > 1.5f)
        {
            effectOnNutrientDistribution += 0.15f;
        }

        // If the substrate retains heat, it might stabilize water temperature
        if (effectOnHeatRetention > 0.7f)
        {
            effectOnWaterTemperatureStability += 0.2f;
        }

        // If the substrate supports aquatic fauna, it might increase the biodiversity of the ecosystem
        if (supportsAquaticFauna)
        {
            effectOnBiodiversity += 0.25f;
        }

        // If the substrate affects toxicity, it might harm aquatic life
        if (effectOnToxicity > 1.0f)
        {
            effectOnAquaticLifeHealth -= 0.3f;
        }
        else if (effectOnToxicity < 0.5f)
        {
            effectOnAquaticLifeHealth += 0.2f;
        }

        // If the substrate affects root penetration, it might affect plant health and growth
        if (effectOnRootPenetration < 0.5f)
        {
            effectOnPlantHealth -= 0.2f;
        }
        else if (effectOnRootPenetration > 1.5f)
        {
            effectOnPlantGrowth += 0.3f;
        }
    }

    private void PrintSimulationResults()
    {
        // Print the simulation results (for testing purposes)
        Debug.Log("Water Clarity: " + effectOnWaterClarity);
        Debug.Log("Algae Growth: " + effectOnAlgaeGrowth);
        Debug.Log("Nutrient Distribution: " + effectOnNutrientDistribution);
        Debug.Log("Water Temperature Stability: " + effectOnWaterTemperatureStability);
        Debug.Log("Biodiversity: " + effectOnBiodiversity);
        Debug.Log("Aquatic Life Health: " + effectOnAquaticLifeHealth);
        Debug.Log("Plant Health: " + effectOnPlantHealth);
        Debug.Log("Plant Growth: " + effectOnPlantGrowth);
    }
}
