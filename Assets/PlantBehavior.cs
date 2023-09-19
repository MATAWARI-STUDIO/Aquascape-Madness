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
    private float lightConsumptionRate = 0.1f;
    public float nutrientConsumptionRate = 0.1f;
    private float health;
    private float growthRate;

    private void Start()
    {
        plantTraits = GetComponent<PlantTraits>();
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
        if (plantTraits != null)
        {
            float pHHealthEffect = CalculatePHEffect(pHValue);
            float ammoniaHealthEffect = CalculateAmmoniaEffect(ammoniaValue);
            float nitrateHealthEffect = CalculateNitrateEffect(nitrateValue);

            plantTraits.health += pHHealthEffect + ammoniaHealthEffect + nitrateHealthEffect;
            plantTraits.stress += pHHealthEffect + ammoniaHealthEffect + nitrateHealthEffect;

            plantTraits.health = Mathf.Clamp(plantTraits.health, 0.0f, 100.0f);
            plantTraits.stress = Mathf.Clamp(plantTraits.stress, 0.0f, 100.0f);
        }
        else
        {
            Debug.LogWarning("PlantTraits reference is not set.");
        }
    }

    private float CalculatePHEffect(float pHValue)
    {
        float optimalPHRangeMin = 6.5f;
        float optimalPHRangeMax = 7.5f;

        if (pHValue < optimalPHRangeMin || pHValue > optimalPHRangeMax)
        {
            return -5.0f;
        }
        return 0.0f;
    }

    private float CalculateAmmoniaEffect(float ammoniaValue)
    {
        float maxAmmoniaThreshold = 1.0f;

        if (ammoniaValue > maxAmmoniaThreshold)
        {
            return -10.0f;
        }
        return 0.0f;
    }

    private float CalculateNitrateEffect(float nitrateValue)
    {
        float maxNitrateThreshold = 1.0f;

        if (nitrateValue > maxNitrateThreshold)
        {
            return -5.0f;
        }
        return 0.0f;
    }

    private void Die()
    {
        gameObject.SetActive(false);

        if (wiltingIndicator != null)
        {
            Instantiate(wiltingIndicator, transform.position, Quaternion.identity);
        }

        Debug.Log($"Plant died: {plantName}");

        // Add the following line to handle decomposition
        DecompositionManager.Instance.HandleDecomposition(plantTraits.nutritionValue);
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
        // Logic to adjust the plant's behavior based on consumed light and nutrients.
        // For example, you might adjust the plant's growth rate or health based on these values.
        float lightEffect = consumedLight * 0.05f; // Example value, adjust as needed
        float nutrientEffect = consumedNutrient * 0.1f; // Example value, adjust as needed

        plantTraits.UpdatePlantHealth(lightEffect + nutrientEffect);
        plantTraits.UpdatePlantStress(-nutrientEffect); // Assuming nutrients reduce stress
    }

}
