using UnityEngine;

public class FilterBehavior : MonoBehaviour
{
    public string filterName;
    public JSONLoader.FilterData filterData; // Notice the explicit reference to JSONLoader.FilterData
    public WaterQualityParameters waterQualityParameters; // Reference to WaterQualityParameters

    private void Start()
    {
        JSONLoader jsonLoader = FindObjectOfType<JSONLoader>();
        if (jsonLoader != null)
        {
            filterData = jsonLoader.filterData;
            if (filterData == null)
            {
                Debug.LogError("Filter data not found for: " + gameObject.name);
            }
        }
        else
        {
            Debug.LogError("JSONLoader not found in the scene. Make sure it exists.");
        }

        // Find and store a reference to WaterQualityParameters in the scene
        waterQualityParameters = FindObjectOfType<WaterQualityParameters>();
        if (waterQualityParameters == null)
        {
            Debug.LogError("WaterQualityParameters not found in the scene. Make sure it exists.");
        }
    }

    public void ApplyFilterEffects()
    {
        ApplyEffectOnpH();
        ApplyEffectOnAmmonia();
        ApplyEffectOnNitrite();
        ApplyEffectOnNitrate();
        ApplyEffectOnOxygen();
    }

    private void ApplyEffectOnpH()
    {
        if (filterData != null && waterQualityParameters != null)
        {
            float pHChangeRate = filterData.filters[0].pHChangeRate;
            waterQualityParameters.AdjustpHLevel(pHChangeRate); // Adjust pH due to filter effect
        }
    }

    private void ApplyEffectOnAmmonia()
    {
        if (filterData != null && waterQualityParameters != null)
        {
            float ammoniaReduction = filterData.filters[0].ammoniaChangeRate;
            waterQualityParameters.AdjustAmmoniaLevel(ammoniaReduction);
        }
    }

    private void ApplyEffectOnNitrite()
    {
        if (filterData != null && waterQualityParameters != null)
        {
            float nitriteChangeRate = filterData.filters[0].nitriteChangeRate;
            waterQualityParameters.AdjustNitriteLevel(nitriteChangeRate);
        }
    }

    private void ApplyEffectOnNitrate()
    {
        if (filterData != null && waterQualityParameters != null)
        {
            float nitrateChangeRate = filterData.filters[0].nitrateChangeRate;
            waterQualityParameters.AdjustNitrateLevel(nitrateChangeRate);
        }
    }

    private void ApplyEffectOnOxygen()
    {
        if (filterData != null && waterQualityParameters != null)
        {
            float oxygenChangeRate = filterData.filters[0].oxygenChangeRate;
            waterQualityParameters.AdjustOxygenLevel(oxygenChangeRate);
        }
    }
}
