using UnityEngine;

public class LightCycle : MonoBehaviour
{
    public Light aquariumLight; // Reference to the light source in Unity
    public float cycleDuration = 120.0f; // Duration of the complete light cycle in seconds
    public AnimationCurve intensityCurve; // Intensity curve for the light cycle

    private float elapsedTime = 0.0f;

    private void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the normalized time for the light cycle
        float normalizedTime = Mathf.PingPong(elapsedTime / cycleDuration, 1.0f);

        // Evaluate the intensity curve to get the desired intensity
        float targetIntensity = intensityCurve.Evaluate(normalizedTime);

        // Set the light intensity
        if (aquariumLight != null)
        {
            aquariumLight.intensity = targetIntensity;
        }
    }

    // Method to update the light cycle based on a normalized time of day
    public void UpdateCycle(float normalizedTimeOfDay)
    {
        // Calculate the intensity for the given normalized time of day
        float targetIntensity = intensityCurve.Evaluate(normalizedTimeOfDay);

        // Set the light intensity
        if (aquariumLight != null)
        {
            aquariumLight.intensity = targetIntensity;
        }
    }
}
