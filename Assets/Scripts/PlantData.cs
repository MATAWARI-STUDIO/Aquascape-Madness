using UnityEngine;
using System;

[System.Serializable]
public class PlantData
{
    public Plant[] plants;
}

[System.Serializable]
public class Plant
{
    public string id;
    public string name;
    public string type;
    public float[] pH;
    public float[] ammonia_ppm;
    public float[] nitrite_ppm;
    public float[] nitrate_ppm;
    public float[] o2_production_mgphg;
    public string light_requirement;
    public float light_intensity_lux;
    public float[] co2_needs_ppm;
    public float[] temperature_range_celsius;
    public float nitrateLevel;
    public int price_usd;
    public float plantSize;
    public float growthRate;
    public float nutrientReleaseRate;
    public float oxygenConsumptionRate;
    public string lightSpectrum;
    public float co2AbsorptionRate;
    public int health;
    public int stress;
    public Interaction interactions;
    public float[] carbonate_hardness;
    public float[] general_hardness;
    public string description;
    public float nutrientUptakeRate;
    public float herbivoreFoodAmount;
    public float ammoniaEffect; // Effect of the plant on ammonia levels
    public float nitrateEffect; // Effect of the plant on nitrate levels
    public float pHEffect;      // Effect of the plant on pH levels

    // New fields for phosphorus and potassium
    public float[] phosphorus_ppm;
    public float[] potassium_ppm;

    public float Size { get; set; } // Representing the size of the plant

    public float LightThreshold { get; set; } // Representing the light threshold for the plant

    public Vector3 Position { get; set; }

    public float CalculateAdjustedGrowthRate(float currentTemperature)
    {
        float optimalTemperature = (temperature_range_celsius[0] + temperature_range_celsius[1]) / 2f;
        float temperatureEffect = 1f - Mathf.Abs(currentTemperature - optimalTemperature) / 20f;
        temperatureEffect = Mathf.Clamp01(temperatureEffect);
        return growthRate * temperatureEffect;
    }
}

[System.Serializable]
public class Interaction
{
    public FishInteraction fish;
}
