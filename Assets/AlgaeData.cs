using UnityEngine;

[System.Serializable]
public class AlgaeData
{
    public Algae[] algae;
}

[System.Serializable]
public class Algae
{
    public string name;
    public string type;
    public float[] pH_tolerance;
    public float[] nitrate_tolerance_ppm;
    public float[] growth_rate_range;
    public float oxygen_production_rate;
    public AlgaeInteraction interaction_with_fish;
    public AlgaeInteractionWithWater interaction_with_water;
    public string description;
    public float health;
    public float effectOnOxygenProduction;
}

[System.Serializable]
public class AlgaeInteraction
{
    public float effectOnpH;
    public float effectOnNitrate;
    public float effectOnOxygenProduction;
}

[System.Serializable]
public class AlgaeInteractionWithWater
{
    public float effectOnpH;
    public float effectOnNitrate;
    public float effectOnOxygenProduction;
}
