using UnityEngine;
using UnityEngine.UI;

public class OpenWaterChangePanelButton : MonoBehaviour
{
    public WaterChangeManager waterChangeManager; // Reference to the WaterChangeManager script.

    private void Start()
    {
        // Ensure the WaterChangeManager reference is set in the Inspector.
        if (waterChangeManager == null)
        {
            Debug.LogError("WaterChangeManager reference is not set. Please assign it in the Inspector.");
        }
    }

    public void OnButtonClick()
    {
        // Call the OpenWaterChangePanel method to open the Water Change Panel.
        waterChangeManager.OpenWaterChangePanel();
    }
}
