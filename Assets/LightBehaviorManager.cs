using UnityEngine;

public class LightBehaviorManager : MonoBehaviour
{
    public WaterQualityParameters waterQualityParameters;
    public LightIntensityManager lightIntensityManager;

    public string lightDataName;

    private void Start()
    {
        if (waterQualityParameters == null)
        {
            Debug.LogError("WaterQualityParameters reference not set in LightBehaviorManager");
            enabled = false;
            return;
        }

        if (lightIntensityManager == null)
        {
            Debug.LogError("LightIntensityManager reference not set in LightBehaviorManager");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Update water quality based on the current light intensity
        UpdateWaterQualityWithArtificialLight(lightIntensityManager.currentLightIntensity);
    }

    private void UpdateWaterQualityWithArtificialLight(float lightIntensity)
    {
        // Adjust algae growth based on light intensity
        waterQualityParameters.AdjustAlgaePopulation(lightIntensity * 0.05f);

        // Adjust plant nutrient uptake based on light intensity
        waterQualityParameters.AdjustNitrateLevel(-lightIntensity * 0.02f);
        waterQualityParameters.AdjustNutrientLevels(-lightIntensity * 0.01f);

        // Adjust temperature based on light intensity
        AdjustTemperature(lightIntensity * 0.1f);
    }

    private void AdjustTemperature(float change)
    {
        float newTemperature = waterQualityParameters.GetTemperature() + change;
        waterQualityParameters.SetTemperature(newTemperature);
    }
}
