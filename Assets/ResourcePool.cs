using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] private WaterQualityParameters waterQuality; // Reference to the WaterQualityParameters script
    [SerializeField] private float lightAvailability = 100.0f; // Assuming a default value

    public float ConsumeResource(ref float resource, float amount)
    {
        float consumed = Mathf.Min(resource, amount);
        resource -= consumed;
        return consumed;
    }

    public void ReleaseResource(ref float resource, float amount)
    {
        resource += amount;
    }

    public float GetNutrientAvailability()
    {
        return waterQuality.GetNutrientLevel(); // Directly get nutrient availability from WaterQualityParameters
    }

    public float GetLightAvailability()
    {
        return lightAvailability;
    }

    public void ReduceLightAvailability(float amount)
    {
        lightAvailability -= amount;
        lightAvailability = Mathf.Clamp(lightAvailability, 0.0f, 1000.0f);
    }

    public void AdjustNutrientAvailability(float amount)
    {
        // If light availability is low, reduce the rate of nutrient consumption
        float lightFactor = lightAvailability / 1000.0f; // Assuming 1000 is the max light availability
        float adjustedAmount = amount * lightFactor;

        // Adjust nutrient level in WaterQualityParameters
        waterQuality.AdjustNutrientLevels(adjustedAmount);

        // Clamping the nutrient availability between 0 and a maximum value (let's assume 2000 for now)
        // This is now done in the WaterQualityParameters script
    }
}
