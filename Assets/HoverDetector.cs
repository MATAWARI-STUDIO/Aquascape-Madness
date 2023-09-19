using UnityEngine;

public class HoverDetector : MonoBehaviour
{
    public JSONLoader jsonLoader; // Assign this reference in the Inspector
    public string plantName; // Assign the plant name in the Inspector

    private void Update()
    {
        if (jsonLoader != null)
        {
            Debug.Log("jsonLoader is not null.");

            if (jsonLoader.plantDataPanel != null)
            {
                Debug.Log("plantDataPanel is not null. Active: " + jsonLoader.plantDataPanel.IsActive());

                if (!jsonLoader.plantDataPanel.IsActive())
                {
                    Plant plantData = jsonLoader.GetPlantDataByName(plantName);

                    if (plantData != null)
                    {
                        Debug.Log("Plant data found: " + plantData.name);

                        if (jsonLoader.plantDataPanel != null)
                        {
                            jsonLoader.plantDataPanel.UpdatePlantData(plantData);
                            jsonLoader.plantDataPanel.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("plantDataPanel is null.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Plant data is null for plantName: " + plantName);
                    }
                }
                else
                {
                    jsonLoader.plantDataPanel.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("plantDataPanel is null.");
            }
        }
        else
        {
            Debug.LogError("jsonLoader is null.");
        }
    }

}
