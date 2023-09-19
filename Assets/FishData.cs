using UnityEngine;

[System.Serializable]
public class FishData
{
    public Fish[] fishes;
}

[System.Serializable]
public class Fish
{
    public string name;
    public string type;
    public float[] pH_tolerance;
    public float[] ammonia_tolerance_ppm;
    public float[] nitrite_tolerance_ppm;
    public float[] nitrate_tolerance_ppm;
    public float[] o2_production_mgphg;
    public float[] co2_production_ppm;
    public float[] carbonate_hardness_Tolerance;
    public float[] general_hardness_Tolerance;
    public float[] temperature_range_celsius;
    public FishInteraction interaction_with_plant;
    public FishInteraction interaction_with_water;
    public bool isHerbivorous;
    public float herbivoreFoodConsumptionRate;
    public float herbivoreFoodAmount;
    public float totalPlantBiomass;
    public float hunger;
    public float predatorFoodAmount;
    public int price_usd;
    public string description;
    public int health;
    public int stress;
    public float effectOnNitrate;
    public float[] temperature_tolerance_celsius;
}

[System.Serializable]
public class FishInteraction
{
    public float effectOnpH;
    public float effectOnAmmonia;
    public float effectOnNitrite;
    public float effectOnNitrate;
    public float effectOnCO2Production;
}

[System.Serializable]
public class FishInteractionWithWater
{
    public float effectOnpH;
    public float effectOnAmmonia;
    public float effectOnNitrite;
    public float effectOnNitrate;
    public float effectOnCO2Production;
}
