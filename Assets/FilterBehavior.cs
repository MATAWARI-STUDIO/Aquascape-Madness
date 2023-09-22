using UnityEngine;

public class FilterBehavior : MonoBehaviour
{
    public string filterDataName;
    public Filter filterData; // Change the type to Filter
    public WaterQualityParameters waterQualityParameters;
    private JSONLoader jsonLoader;


    private void Start()
    {
        jsonLoader = GameObject.FindObjectOfType<JSONLoader>();
        if (jsonLoader == null)
        {
            Debug.LogError("No JSONLoader component found in the scene.");
            enabled = false;
            return;
        }

        // Find the filter data by name
        foreach (var filter in jsonLoader.filterData.filters)
        {
            if (filter.displayName == filterDataName)
            {
                filterData = filter;
                break;
            }
        }

        if (filterData == null)
        {
            Debug.LogError("Filter data not found for: " + filterDataName);
            enabled = false;
            return;
        }

        waterQualityParameters = FindObjectOfType<WaterQualityParameters>();
        if (waterQualityParameters == null)
        {
            Debug.LogError("WaterQualityParameters not found in the scene. Make sure it exists.");
            enabled = false;
            return;
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
        float pHChangeRate = filterData.pHChangeRate; // Access pHChangeRate directly
        waterQualityParameters.AdjustpHLevel(pHChangeRate);
    }

    private void ApplyEffectOnAmmonia()
    {
        float ammoniaReduction = filterData.ammoniaChangeRate; // Access ammoniaChangeRate directly
        waterQualityParameters.AdjustAmmoniaLevel(ammoniaReduction);
    }

    private void ApplyEffectOnNitrite()
    {
        float nitriteChangeRate = filterData.nitriteChangeRate; // Access nitriteChangeRate directly
        waterQualityParameters.AdjustNitriteLevel(nitriteChangeRate);
    }

    private void ApplyEffectOnNitrate()
    {
        float nitrateChangeRate = filterData.nitrateChangeRate; // Access nitrateChangeRate directly
        waterQualityParameters.AdjustNitrateLevel(nitrateChangeRate);
    }

    private void ApplyEffectOnOxygen()
    {
        float oxygenChangeRate = filterData.oxygenChangeRate; // Access oxygenChangeRate directly
        waterQualityParameters.AdjustOxygenLevel(oxygenChangeRate);
    }
}
