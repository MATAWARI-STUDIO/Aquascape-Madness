using UnityEngine;

[System.Serializable]
public class BacteriaData
{
    public Bacteria[] bacteria;
}

[System.Serializable]
public class Bacteria
{
    public string name;
    public string type;
    public float[] pH_tolerance;
    public float[] ammonia_tolerance_ppm;
    public float[] growth_rate_range;
    public BacteriaInteraction interaction_with_fish;
    public BacteriaInteractionWithWater interaction_with_water;
    public string description;
    public float health;

}

[System.Serializable]
public class BacteriaInteraction
{
    public float effectOnpH;
    public float effectOnAmmonia;
}

[System.Serializable]
public class BacteriaInteractionWithWater
{
    public float effectOnpH;
    public float effectOnAmmonia;
}
