using UnityEngine;
using System.Collections.Generic;

public class FilterBehaviorManager : MonoBehaviour
{
    [SerializeField] private List<FilterBehavior> filters = new List<FilterBehavior>();

    private WaterQualityParameters waterQualityParameters; // Updated reference

    private void Start()
    {
        waterQualityParameters = FindObjectOfType<WaterQualityParameters>(); // Updated type
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
        if (filter != null)
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
