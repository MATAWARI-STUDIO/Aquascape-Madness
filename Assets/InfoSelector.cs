using UnityEngine;

public class InfoSelector : MonoBehaviour
{
    public JSONLoader jsonLoader;
    public PlantInfoPanel plantInfoPanel;
    public FishInfoPanel fishInfoPanel;
    public LightInfoPanel lightInfoPanel;
    public FilterInfoPanel filterInfoPanel;
    public Camera mainCamera;
    public LayerMask interactableLayerMask;
    public static ObjectPlacementController instance;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ObjectPlacementController.instance.IsObjectBeingPlaced())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, interactableLayerMask))
            {
                Debug.Log("Hit object: " + hit.transform.gameObject.name);

                PlantBehavior plantBehavior = hit.transform.GetComponent<PlantBehavior>();
                FishBehavior fishBehavior = hit.transform.GetComponent<FishBehavior>();
                LightBehavior lightBehavior = hit.transform.GetComponent<LightBehavior>();
                FilterBehavior filterBehavior = hit.transform.GetComponent<FilterBehavior>();

                // Deactivate all panels initially
                plantInfoPanel.gameObject.SetActive(false);
                fishInfoPanel.gameObject.SetActive(false);
                lightInfoPanel.gameObject.SetActive(false);
                filterInfoPanel.gameObject.SetActive(false);

                if (plantBehavior != null)
                {
                    Debug.Log("Hit object is a plant: " + plantBehavior.plantName);
                    Plant selectedPlant = jsonLoader.GetPlantDataByName(plantBehavior.plantName);
                    plantInfoPanel.gameObject.SetActive(true);
                    plantInfoPanel.UpdatePlantInfo(selectedPlant);
                }
                else if (fishBehavior != null)
                {
                    Debug.Log("Hit object is a fish: " + fishBehavior.fish.name);
                    Fish selectedFish = jsonLoader.GetFishDataByName(fishBehavior.fish.name);
                    fishInfoPanel.gameObject.SetActive(true);
                    fishInfoPanel.UpdateFishInfo(selectedFish);
                }
                else if (lightBehavior != null)
                {
                    Debug.Log("Hit object is a light.");
                    JSONLoader.Lights selectedLight = jsonLoader.GetLightByName(lightBehavior.lightDataName);
                    if (selectedLight != null)
                    {
                        lightInfoPanel.gameObject.SetActive(true);
                        lightInfoPanel.UpdateLightInfo(selectedLight);
                    }
                    else
                    {
                        Debug.LogError("Selected light data is null.");
                    }
                }

                else if (filterBehavior != null)
                {
                    Debug.Log("Hit object is a filter: " + filterBehavior.filterName);
                    Filter selectedFilter = jsonLoader.GetFilterByName(filterBehavior.filterName);
                    filterInfoPanel.gameObject.SetActive(true);
                    filterInfoPanel.UpdateFilterInfo(selectedFilter);
                }
                else
                {
                    Debug.Log("Hit object is not recognized as plant, fish, light, or filter.");
                }
            }
        }
    }
}
