using UnityEngine;
using System.Collections.Generic; // Required for List<T>

public class LightIntensityManager : MonoBehaviour
{
    public List<Light> lightGameObjects; // List of Light components
    public float currentLightIntensity;

    private void Update()
    {
        foreach (Light light in lightGameObjects)
        {
            SimulateLightIntensity(light);
        }
    }

    public void SimulateLightIntensity(Light lightGameObject)
    {
        // Simple sinusoidal model for day-night light intensity cycle
        float amplitude = 1.0f;
        float frequency = 0.1f;
        currentLightIntensity = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);

        // Clamp light intensity to [0, 1]
        currentLightIntensity = Mathf.Clamp(currentLightIntensity, 0, 1);

        if (lightGameObject != null)
        {
            lightGameObject.intensity = currentLightIntensity;
        }
    }
}
