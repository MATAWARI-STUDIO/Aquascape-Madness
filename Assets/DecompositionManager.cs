using UnityEngine;

public class DecompositionManager : MonoBehaviour
{
    private static DecompositionManager _instance;
    public static DecompositionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DecompositionManager>();
                if (_instance == null)
                {
                    Debug.LogError("No instance of DecompositionManager found!");
                }
            }
            return _instance;
        }
    }

    public WaterQualityParameters waterQuality;
    public float decompositionRate = 0.01f; // Adjust this rate as needed.
    public float nutrientReleaseRate = 0.005f; // Adjust this rate as needed.
    [SerializeField] private WaterQualityParameters waterQualityParameters;

    private float totalOrganicMatter;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        totalOrganicMatter = 0.0f;
    }

    private void Update()
    {
        DecomposeOrganicMatter();
    }

    public void AddOrganicMatter(float amount)
    {
        totalOrganicMatter += amount;
    }

    private void DecomposeOrganicMatter()
    {
        if (totalOrganicMatter > 0)
        {
            float decomposedMatter = totalOrganicMatter * decompositionRate * Time.deltaTime;
            totalOrganicMatter -= decomposedMatter;

            float nutrientsReleased = decomposedMatter * nutrientReleaseRate;
            waterQuality.AdjustNutrientLevels(nutrientsReleased);
        }
    }

    public void HandleDecomposition(float amount)
    {
        if (waterQualityParameters != null)
        {
            waterQualityParameters.AdjustNutrientLevels(amount);
        }
        else
        {
            Debug.LogError("WaterQualityParameters reference is not set in DecompositionManager.");
        }
    }
}
