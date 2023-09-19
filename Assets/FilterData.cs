using UnityEngine;

[System.Serializable]
public class FilterData
{
    public Filter[] filters;
}

[System.Serializable]
public class Filter
{
    public string type;
    public string displayName; // Updated from 'name'
    public string description;
    public float price; // Updated from 'price_usd'
    public FilterEffectiveness effectiveness;
    public string filterMedia;
    public float filterCapacity;
    public float pHChangeRate;
    public float ammoniaChangeRate;
    public float nitriteChangeRate;
    public float nitrateChangeRate;
    public float oxygenChangeRate;
}

[System.Serializable]
public class FilterEffectiveness
{
    public float mechanical;
    public float biological;
    public float chemical;
}
