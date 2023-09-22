[System.Serializable]
public class FilterData
{
    public Filter[] filters;
}

[System.Serializable]
public class Filter
{
    public string type;
    public string displayName;
    public string description;
    public float price;
    public float effectiveness;
    public string filterMedia;
    public float filterCapacity;
    public float pHChangeRate;
    public float ammoniaChangeRate;
    public float nitriteChangeRate;
    public float nitrateChangeRate;
    public float oxygenChangeRate;
    public float powerDraw { get; set; }
    public float flowRate { get; set; }
    public float airflowRate { get; set; }
}

