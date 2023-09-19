using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightInfoPanel))]
public class LightInfoPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LightInfoPanel lightInfoPanel = (LightInfoPanel)target;

        // Draw the default inspector
        base.OnInspectorGUI();

        if (lightInfoPanel.LightGameObject != null)
        {
            float intensity = EditorGUILayout.Slider("Light Intensity", lightInfoPanel.LightGameObject.intensity, 0, 8);
            lightInfoPanel.LightGameObject.intensity = intensity;
            lightInfoPanel.IntensitySlider.value = intensity;
        }

        // Custom slider for light temperature
        float temperature = EditorGUILayout.Slider("Light Temperature", lightInfoPanel.TemperatureSlider.value, 1000, 10000);
        lightInfoPanel.TemperatureSlider.value = temperature;

    }
}
