using UnityEngine;

public class TemperatureManager : MonoBehaviour
{
    public float currentTemperature;

    public void SimulateTemperatureEffects()
    {
        // Simple model for temperature fluctuation
        float amplitude = 2.0f;
        float frequency = 0.01f;
        currentTemperature = 25.0f + amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.time);

        // Clamp temperature to a reasonable range (e.g., [0, 40])
        currentTemperature = Mathf.Clamp(currentTemperature, 0, 40);
    }

    // Add a method to get the current temperature
    public float GetTemperature()
    {
        return currentTemperature;
    }
}
