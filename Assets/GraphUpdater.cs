using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphUpdater : MonoBehaviour
{
    public WaterQualityManager waterQualityManager;
    public TextMeshProUGUI pHTextUI; // Reference to UI text element for pH
    public TextMeshProUGUI ammoniaTextUI; // Reference to UI text element for ammonia
    public TextMeshProUGUI nitriteTextUI; // Reference to UI text element for nitrite
    public TextMeshProUGUI nitrateTextUI; // Reference to UI text element for nitrate
    // Add references to other UI elements here

    private void Start()
    {
        InvokeRepeating("UpdateGraph", 0f, 2f); // Update graph every 2 seconds
    }

    private void UpdateGraph()
    {
        float pHValue = waterQualityManager.GetpH();
        float ammoniaValue = waterQualityManager.GetAmmoniaLevel();
        float nitriteValue = waterQualityManager.GetNitriteLevel();
        float nitrateValue = waterQualityManager.GetNitrateLevel();
        // Get other parameter values similarly

        UpdateGraphUI(pHTextUI, pHValue);
        UpdateGraphUI(ammoniaTextUI, ammoniaValue);
        UpdateGraphUI(nitriteTextUI, nitriteValue);
        UpdateGraphUI(nitrateTextUI, nitrateValue);
        // Update other UI elements similarly
    }

    private void UpdateGraphUI(TextMeshProUGUI textUI, float value)
    {
        textUI.text = value.ToString("F2"); // Display the value in the UI element
    }
}
