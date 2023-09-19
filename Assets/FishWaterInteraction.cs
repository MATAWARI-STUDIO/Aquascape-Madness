using UnityEngine;

public class FishWaterInteraction : MonoBehaviour
{
    public WaterQualityManager waterQualityManager;
    public JSONLoader jsonLoader;
    public FishInfoPanel fishInfoPanel;

    private FishBehavior fishBehavior;
    private bool isCollidingWithWater = false;
    private Lights[] lights;

    private void Start()
    {
        fishBehavior = GetComponent<FishBehavior>();

        if (waterQualityManager == null)
        {
            Debug.LogError("WaterQualityManager reference is not set.");
        }

        if (jsonLoader == null)
        {
            Debug.LogError("JSONLoader reference is not set.");
        }
        else
        {
            JSONLoader.Lights[] lights = jsonLoader.lightData.lights;
            jsonLoader.LoadFishData();
        }
    }

    private void Update()
    {
        if (fishBehavior == null || jsonLoader.fishData == null || jsonLoader.fishData.fishes == null)
        {
            Debug.LogError("FishBehavior or fish data is invalid.");
            return;
        }

        Fish[] fishes = jsonLoader.fishData.fishes;

        if (fishes.Length == 0)
        {
            Debug.LogError("Fish data or fishes array is empty.");
            return;
        }

        Fish fish = fishes[0];

        if (fish.health <= 0 || fish.stress >= 100) return;

        if (isCollidingWithWater && waterQualityManager != null && jsonLoader != null)
        {
            float pHValue = waterQualityManager.GetpH();
            float ammoniaValue = waterQualityManager.GetAmmoniaLevel();
            float nitriteValue = waterQualityManager.GetNitriteLevel();
            float nitrateValue = waterQualityManager.GetNitrateLevel();
            float o2ProductionRate = waterQualityManager.GetOxygenProduction();
            float currentTemperature = jsonLoader.GetCurrentTemperature();
            float lightIntensityFactor = CalculateLightIntensityFactor();

            fishBehavior.ApplyWaterEffects(fishBehavior.fishData, pHValue, ammoniaValue, nitriteValue, nitrateValue, o2ProductionRate, currentTemperature);

            foreach (Fish fishInstance in waterQualityManager.FishInstances)
            {
                waterQualityManager.ApplyFishEffect(fishInstance, lightIntensityFactor);
            }

            if (fishInfoPanel != null)
            {
                fishInfoPanel.UpdateFishInfo(fishBehavior.fish);
            }
        }
    }

    private float CalculateLightIntensityFactor()
    {
        float totalIntensityFactor = 0f;

        foreach (Lights lights in lights)
        {
            float intensityFactor = lights.intensity_adjustment_factor;
            totalIntensityFactor += intensityFactor;
        }

        return totalIntensityFactor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = true;
            if (waterQualityManager != null && fishBehavior != null)
            {
                float lightIntensityFactor = CalculateLightIntensityFactor();
                waterQualityManager.ApplyFishEffect(fishBehavior.fishData.fishes[0], lightIntensityFactor);
            }
            Debug.Log("Fish entered the water.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = false;
            if (waterQualityManager != null && fishBehavior != null)
            {
                float lightIntensityFactor = CalculateLightIntensityFactor();
                waterQualityManager.RemoveFishEffect(fishBehavior.fishData.fishes[0], lightIntensityFactor);
            }
            Debug.Log("Fish exited the water.");
        }
    }
}
