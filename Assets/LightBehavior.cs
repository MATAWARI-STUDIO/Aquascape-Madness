using UnityEngine;

public class LightBehavior : MonoBehaviour
{
    public string lightDataName;

    public float CurrentIntensity { get; private set; }

    private JSONLoader jsonLoader;
    private Light ledLight; // Reference to the Unity Light component

    private void Start()
    {
        ledLight = GetComponent<Light>();
        if (ledLight == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
            enabled = false;
            return;
        }

        LoadLightSettingFromJSON();
    }

    private void Update()
    {
        CurrentIntensity = GetArtificialLightIntensity();
        ledLight.intensity = CurrentIntensity; // Update the Unity light component's intensity
    }

    private void LoadLightSettingFromJSON()
    {
        jsonLoader = FindObjectOfType<JSONLoader>();
        if (jsonLoader == null)
        {
            Debug.LogError("JSONLoader component not found on this GameObject.");
            enabled = false;
            return;
        }

        // Fetch the light settings from JSON
        JSONLoader.Lights lights = jsonLoader.GetLightByName(lightDataName);
        if (lights != null)
        {
            // Set light properties based on JSON data
            ledLight.color = new Color(lights.color.r, lights.color.g, lights.color.b, lights.color.a);
            ledLight.intensity = lights.light_intensity_lux / 1000f; // Assuming conversion factor for lux to Unity's intensity
        }
        else
        {
            Debug.LogError("LightSetting not found in JSONLoader for name: " + lightDataName);
        }
    }

    private float GetArtificialLightIntensity()
    {
        if (jsonLoader != null)
        {
            JSONLoader.Lights lights = jsonLoader.GetLightByName(lightDataName);
            if (lights != null)
            {
                return lights.intensity;
            }
            else
            {
                Debug.LogError("LightSetting not found in JSONLoader for name: " + lightDataName);
            }
        }
        else
        {
            Debug.LogError("JSONLoader component not found on this GameObject.");
        }

        return 0f; // Return a default value if there's an error
    }
}
