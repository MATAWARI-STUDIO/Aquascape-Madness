using UnityEngine;

public class FishTraits : MonoBehaviour
{
    [Header("Health, Stress, and Hunger")]
    public float initialHealth = 100.0f;
    public float health;
    public float initialStress = 0.0f;
    public float stress;
    public float initialHunger = 0.0f;
    public float hunger;

    [Header("Environmental Requirements")]
    public float pHLevel;
    public float ammoniaLevel;
    public float nitriteLevel;
    public float nitrateLevel;
    public float o2Production;
    public float co2Level;

    [Header("Predatory Behavior")]
    public bool isPredator;
    public float predatorFoodAmount;
    public float PredationRate => isPredator ? predatorFoodAmount : 0;

    [Header("Growth and Metabolism")]
    public bool isHerbivorous;
    public float herbivoreFoodConsumptionRate;
    public float totalPlantBiomassConsumed;

    [Header("Nutritional Value")]
    public float nutritionValue;

    [Header("Physical Characteristics")]
    public float priceUSD;
    public string description;

    public float AmmoniaEffect { get; set; }
    public float NitrateEffect { get; set; }
    public float pHEffect { get; set; }

    private void Start()
    {
        health = initialHealth;
        stress = initialStress;
        hunger = initialHunger;
    }

    private void Update()
    {
        ApplyEnvironmentalEffects();
        health = Mathf.Clamp(health, 0.0f, 100.0f);
        stress = Mathf.Clamp(stress, 0.0f, 100.0f);
        hunger = Mathf.Clamp(hunger, 0.0f, 100.0f);
    }

    private void ApplyEnvironmentalEffects()
    {
        float ammoniaEffect = AmmoniaEffect * Time.deltaTime;
        health += ammoniaEffect;
        stress += ammoniaEffect;

        float nitrateEffect = NitrateEffect * Time.deltaTime;
        health += nitrateEffect;
        stress += nitrateEffect;

        float pHChange = pHEffect * Time.deltaTime;
        health += pHChange;
        stress += pHChange;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetStress()
    {
        return stress;
    }

    public float GetHunger()
    {
        return hunger;
    }

    public void UpdateFishHealth(float healthChange)
    {
        health += healthChange;
        health = Mathf.Clamp(health, 0.0f, 100.0f);
    }

    public void UpdateFishStress(float stressChange)
    {
        stress += stressChange;
        stress = Mathf.Clamp(stress, 0.0f, 100.0f);
    }

    public void UpdateFishHunger(float hungerChange)
    {
        hunger += hungerChange;
        hunger = Mathf.Clamp(hunger, 0.0f, 100.0f);
    }
}
