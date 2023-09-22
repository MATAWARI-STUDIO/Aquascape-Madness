using UnityEngine;
using System.Collections.Generic;

public class FilterBehaviorManager : MonoBehaviour
{
    [SerializeField] private List<FilterBehavior> filters = new List<FilterBehavior>();

    private WaterQualityParameters waterQualityParameters;

    private void Start()
    {
        // Find and store a reference to WaterQualityParameters in the scene
        waterQualityParameters = FindObjectOfType<WaterQualityParameters>();
        if (waterQualityParameters == null)
        {
            Debug.LogError("WaterQualityParameters not found in the scene. Make sure it exists.");
        }
    }

    public void ApplyFilterEffects()
    {
        foreach (FilterBehavior filter in filters)
        {
            if (filter != null)
            {
                filter.ApplyFilterEffects();
            }
        }
    }

    public void AddFilter(FilterBehavior filter)
    {
        if (filter != null && !filters.Contains(filter))
        {
            filters.Add(filter);
        }
    }

    public void RemoveFilter(FilterBehavior filter)
    {
        if (filter != null)
        {
            filters.Remove(filter);
        }
    }
}
