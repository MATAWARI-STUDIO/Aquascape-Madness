[System.Serializable]
public class Substrate
{
    public string id;
    public string name;
    public string type;
    public float pH_effect;
    public int cation_exchange_capacity;
    public int nutrient_holding_capacity;
    public float[] particle_size_mm;
    public string color;
    public float price_usd;
    public string description;
    public string suitability_for_plants;
    public Interactions interactions;
    public float[] potassium_ppm;
    public float[] phosphorus_ppm;

    // Add a property or field to hold a reference to a Plant object
    public Plant plant;

    [System.Serializable]
    public class Interactions
    {
        public PlantInteractions plants;
        public WaterInteractions water;

        [System.Serializable]
        public class PlantInteractions
        {
            public float effectOnGrowthRate;
            public float effectOnNutrientUptake;
        }

        [System.Serializable]
        public class WaterInteractions
        {
            public float effectOnpH;
            public float effectOnAmmonia;
            public float effectOnNitrite;
            public float effectOnNitrate;
            public float effectOnPotassium;
            public float effectOnPhosphorus;
        }
    }
}
