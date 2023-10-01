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
    public float nutrientUptakeRate;

    [Header("Nutritional Value")]
    public float nutritionValue;

    [Header("Physical Characteristics")]
    public float phosphorusLevel;
    public float potassiumLevel;

    public float NutrientUptakeRate => nutrientUptakeRate;

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float Stress
    {
        get { return stress; }
        set { stress = value; }
    }

    public float NutritionValue => nutritionValue;

    public float AmmoniaEffect
    {
        get { return _ammoniaEffect; }
        set
        {
            Debug.Log($"AmmoniaEffect set to: {value}");
            _ammoniaEffect = value;
        }
    }
    private float _ammoniaEffect;

    public float NitrateEffect
    {
        get { return _nitrateEffect; }
        set
        {
            Debug.Log($"NitrateEffect set to: {value}");
            _nitrateEffect = value;
        }
    }
    private float _nitrateEffect;

    public float pHEffect
    {
        get { return _pHEffect; }
        set
        {
            Debug.Log($"pHEffect set to: {value}");
            _pHEffect = value;
        }
    }
    private float _pHEffect;

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

        if (health <= 0.0f)
        {
            // The plant has died due to its health reaching zero.
            // Implement any specific logic or events here related to the plant's death.
            Debug.Log("The plant has died.");
        }
    }

    private void ApplyEnvironmentalEffects()
    {
        float ammoniaEffect = AmmoniaEffect * Time.deltaTime;
        health += ammoniaEffect;
        Debug.Log($"Ammonia Effect: {ammoniaEffect}, New Health: {health}");
        stress += ammoniaEffect;

        float nitrateEffect = NitrateEffect * Time.deltaTime;
        health += nitrateEffect;
        Debug.Log($"Nitrate Effect: {nitrateEffect}, New Health: {health}");
        stress += nitrateEffect;

        float pHChange = pHEffect * Time.deltaTime;
        health += pHChange;
        Debug.Log($"pH Effect: {pHChange}, New Health: {health}");
        stress += pHChange;
    }

    public float GetHealth() => health;

    public float GetStress() => stress;

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
