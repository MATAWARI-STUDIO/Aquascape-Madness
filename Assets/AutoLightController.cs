using UnityEngine;

public class AutoLightControl : MonoBehaviour
{
    public Light lightComponent;
    public LightInfoPanel lightInfoPanel;
    public TimeController timeController;

    public int autoOnHour;
    public int autoOnMinute;
    public int autoOffHour;
    public int autoOffMinute;

    private bool isLightOn = false;

    private void Update()
    {
        if (ShouldTurnLightOn())
        {
            TurnLightOn();
        }
        else if (ShouldTurnLightOff())
        {
            TurnLightOff();
        }
    }

    private bool ShouldTurnLightOn()
    {
        // Check if the current time matches the auto-on time
        return timeController != null &&
               !isLightOn &&
               timeController.CurrentHour == autoOnHour &&
               timeController.CurrentMinute == autoOnMinute;
    }

    private bool ShouldTurnLightOff()
    {
        // Check if the current time matches the auto-off time
        return timeController != null &&
               isLightOn &&
               timeController.CurrentHour == autoOffHour &&
               timeController.CurrentMinute == autoOffMinute;
    }

    private void TurnLightOn()
    {
        if (lightComponent != null)
        {
            lightComponent.enabled = true;
            isLightOn = true;
            // You can also update the LightInfoPanel or other UI elements here
        }
    }

    private void TurnLightOff()
    {
        if (lightComponent != null)
        {
            lightComponent.enabled = false;
            isLightOn = false;
            // You can also update the LightInfoPanel or other UI elements here
        }
    }
}
