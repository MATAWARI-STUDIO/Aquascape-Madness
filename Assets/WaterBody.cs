using UnityEngine;

public class WaterBody : MonoBehaviour
{
    [Header("Water Body Properties")]
    [SerializeField] private float volume = 100.0f; // Volume of the water body in liters

    public float NutrientUptakeRate { get; set; }

    // Properties to access the private fields
    public float Volume
    {
        get { return volume; }
        set { volume = value; }
    }

    // Properties to access box collider dimensions
    public float Width
    {
        get { return GetColliderWidth(); }
    }

    public float Length
    {
        get { return GetColliderLength(); }
    }

    public float Depth
    {
        get { return GetColliderDepth(); }
    }

    private void Start()
    {
        // Initialization logic if needed
    }

    private void Update()
    {
        // Update logic if needed
    }

    // Additional methods related to the water body can be added here

    private float GetColliderWidth()
    {
        // Assuming you have a BoxCollider component attached to this GameObject
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            return collider.size.x;
        }
        else
        {
            Debug.LogWarning("No BoxCollider found on WaterBody GameObject. Returning default width.");
            return 1.0f; // Return a default width if no collider is found
        }
    }

    private float GetColliderLength()
    {
        // Assuming you have a BoxCollider component attached to this GameObject
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            return collider.size.z;
        }
        else
        {
            Debug.LogWarning("No BoxCollider found on WaterBody GameObject. Returning default length.");
            return 1.0f; // Return a default length if no collider is found
        }
    }

    private float GetColliderDepth()
    {
        // Assuming you have a BoxCollider component attached to this GameObject
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            return collider.size.y;
        }
        else
        {
            Debug.LogWarning("No BoxCollider found on WaterBody GameObject. Returning default depth.");
            return 1.0f; // Return a default depth if no collider is found
        }
    }
}
