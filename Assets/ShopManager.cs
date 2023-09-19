using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] categoryPanels; // Array of category panels (Plants, Fishes, Substrates, etc.)
    public GameObject plantDataPanel;   // Reference to the plant data panel
    public GameObject fishDataPanel;    // Reference to the fish data panel
    public GameObject substrateDataPanel; // Reference to the substrate data panel
    public AudioSource hoverSound;
    public AudioSource clickSound; // Reference to the AudioSource component for the click sound

    // ... Add references to other data panels if needed

    private GameObject currentCategoryPanel; // Keep track of the currently active category panel

    private void Start()
    {
        // Initialize the shop by showing the first category panel
        ShowCategoryPanel(0); // Assuming 0 is the index of the first category
    }

    public void ShowCategoryPanel(int categoryIndex)
    {
        // Hide the currently active category panel
        if (currentCategoryPanel != null)
        {
            currentCategoryPanel.SetActive(false);
        }

        // Show the selected category panel
        currentCategoryPanel = categoryPanels[categoryIndex];
        currentCategoryPanel.SetActive(true);
    }

    public void ShowPlantDataPanel(Plant plant)
    {
        plantDataPanel.SetActive(true);
        // Set the plant data in the plant data panel based on the provided Plant object
        // You can access the UI elements in the plant data panel and update their values
    }

    public void ShowFishDataPanel(Fish fish)
    {
        fishDataPanel.SetActive(true);
        // Set the fish data in the fish data panel based on the provided Fish object
        // You can access the UI elements in the fish data panel and update their values
    }

    public void ShowSubstrateDataPanel(Substrate substrate)
    {
        substrateDataPanel.SetActive(true);
        // Set the substrate data in the substrate data panel based on the provided Substrate object
        // You can access the UI elements in the substrate data panel and update their values
    }

    public void PlayHoverSound()
    {
        hoverSound.Play();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }
    // ... Add similar methods for other data panels
}
