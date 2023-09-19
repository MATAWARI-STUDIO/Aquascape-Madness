using UnityEngine;

public class PlantTraits : MonoBehaviour
{
    [Header("Health and Stress")]
    public float initialHealth = 100.0f;
    public float health;
    public float initialStress = 0.0f;
    public float stress;

    [Header("Environmental Requirements")]
    public float pHLevel;
    public float ammoniaLevel;
    public float nitriteLevel;
    public float nitrateLevel;
    public float o2Production;
    public float co2Level;

    [Header("Growth and Metabolism")]
    public float growthRate;
    public float plantSize;
    private float nutrientUptakeRate;

    [Header("Nutritional Value")]
    public float nutritionValue; // Added nutrition value for the plant

    public float NutrientUptakeRate => nutrientUptakeRate;

    [Header("Physical Characteristics")]
    public float phosphorusLevel;
    public float potassiumLevel;

    public float AmmoniaEffect { get; set; }
    public float NitrateEffect { get; set; }
    public float pHEffect { get; set; }

    private void Start()
    {
        health = initialHealth;
        stress = initialStress;
    }

    private void Update()
    {
        ApplyEnvironmentalEffects();
        health = Mathf.Clamp(health, 0.0f, 100.0f);
        stress = Mathf.Clamp(stress, 0.0f, 100.0f);
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

    public void UpdatePlantHealth(float healthChange)
    {
        health += healthChange;
        health = Mathf.Clamp(health, 0.0f, 100.0f);
    }

    public void UpdatePlantStress(float stressChange)
    {
        stress += stressChange;
        stress = Mathf.Clamp(stress, 0.0f, 100.0f);
    }
}
