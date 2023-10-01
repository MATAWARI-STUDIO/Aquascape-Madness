using UnityEngine;

public class PlantBehavior : MonoBehaviour
{
    public string plantName;
    public WaterQualityParameters waterQualityManager;
    public GameObject deathIndicator;
    public GameObject wiltingIndicator;
    public PlantTraits plantTraits;

    private bool isCollidingWithWater = false;
    private bool isWilting = false;
    private float wiltingTimer = 0f;

    public float lightConsumptionRate = 0.1f;
    public float nutrientConsumptionRate = 0.1f;

    private void Start()
    {
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (plantTraits == null)
        {
            Debug.LogError($"PlantTraits component not found on GameObject: {gameObject.name}");
        }

        if (waterQualityManager == null)
        {
            Debug.LogError($"WaterQualityManager reference is not set for plant: {plantName}");
        }

        if (string.IsNullOrEmpty(plantName))
        {
            Debug.LogError($"PlantName is not set for PlantBehavior script on GameObject: {gameObject.name}");
        }
    }

    private void Update()
    {
        if (isCollidingWithWater)
        {
            ApplyWaterEffects(waterQualityManager.GetpH(), waterQualityManager.GetAmmoniaLevel(), waterQualityManager.GetNitrateLevel());

            if (plantTraits.GetHealth() <= 0 && !isWilting)
            {
                StartWilting();
            }

            if (isWilting)
            {
                UpdateWilting();
            }
        }
    }

    public void ApplyWaterEffects(float pHValue, float ammoniaValue, float nitrateValue)
    {
        Debug.Log($"Incoming pHValue: {pHValue}, ammoniaValue: {ammoniaValue}, nitrateValue: {nitrateValue}");

        if (plantTraits != null)
        {
            float pHHealthEffect = CalculatePHEffect(pHValue);
            float ammoniaHealthEffect = CalculateAmmoniaEffect(ammoniaValue);
            float nitrateHealthEffect = CalculateNitrateEffect(nitrateValue);

            Debug.Log($"Calculated pHHealthEffect: {pHHealthEffect}, ammoniaHealthEffect: {ammoniaHealthEffect}, nitrateHealthEffect: {nitrateHealthEffect}");

            plantTraits.pHEffect = pHHealthEffect;
            plantTraits.AmmoniaEffect = ammoniaHealthEffect;
            plantTraits.NitrateEffect = nitrateHealthEffect;

            plantTraits.Health += pHHealthEffect + ammoniaHealthEffect + nitrateHealthEffect;
            plantTraits.Stress += pHHealthEffect + ammoniaHealthEffect + nitrateHealthEffect;

            plantTraits.Health = Mathf.Clamp(plantTraits.Health, 0.0f, 100.0f);
            plantTraits.Stress = Mathf.Clamp(plantTraits.Stress, 0.0f, 100.0f);
        }
        else
        {
            Debug.LogWarning("PlantTraits reference is not set.");
        }
    }

    private float CalculatePHEffect(float pHValue)
    {
        const float optimalPHRangeMin = 6.5f;
        const float optimalPHRangeMax = 7.5f;

        return (pHValue < optimalPHRangeMin || pHValue > optimalPHRangeMax) ? -5.0f : 0.0f;
    }

    private float CalculateAmmoniaEffect(float ammoniaValue)
    {
        // Gradual effect for ammonia: The closer the ammonia value is to the threshold, the more negative effect it has.
        const float maxAmmoniaThreshold = 1.0f;
        return Mathf.Lerp(0, -10.0f, Mathf.Clamp01((ammoniaValue - 0.8f) / (maxAmmoniaThreshold - 0.8f)));
    }

    private float CalculateNitrateEffect(float nitrateValue)
    {
        const float maxNitrateThreshold = 1.0f;
        return (nitrateValue > maxNitrateThreshold) ? -5.0f : 0.0f;
    }

    private void Die()
    {
        if (wiltingIndicator != null)
        {
            Instantiate(wiltingIndicator, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
        Debug.Log($"Plant died: {plantName}");
        DecompositionManager.Instance.HandleDecomposition(plantTraits.NutritionValue);
    }

    public void StartWilting()
    {
        isWilting = true;
        wiltingTimer = 30f;
    }

    public void UpdateWilting()
    {
        wiltingTimer -= Time.deltaTime;
        if (wiltingTimer <= 0f)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isCollidingWithWater = false;
        }
    }

    public void UpdatePlantBehavior(float consumedLight, float consumedNutrient)
    {
        float lightEffect = consumedLight * lightConsumptionRate;
        float nutrientEffect = consumedNutrient * nutrientConsumptionRate;

        plantTraits.UpdatePlantHealth(lightEffect + nutrientEffect);
        plantTraits.UpdatePlantStress(-nutrientEffect);  // Assuming nutrients reduce stress
    }
}
