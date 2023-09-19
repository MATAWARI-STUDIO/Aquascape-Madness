using UnityEngine;

public class RotatePrefabs : MonoBehaviour
{
    public float rotationSpeed = 10f; // The rotation speed for the prefab
    private Quaternion initialRotation; // The initial rotation of the prefab
    private bool isEnabled = true; // Flag to track whether the rotation is enabled

    private void Awake()
    {
        // Store the initial rotation of the prefab
        initialRotation = transform.rotation;
    }

    private void OnEnable()
    {
        // Set the prefab's rotation to the initial rotation when it becomes active
        transform.rotation = initialRotation;
    }

    private void Update()
    {
        // Rotate the prefab continuously around the Y-axis only when enabled
        if (isEnabled && gameObject.activeInHierarchy)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.unscaledDeltaTime);
        }
    }

    public void EnableRotation()
    {
        // Enable the rotation
        isEnabled = true;
    }

    public void DisableRotation()
    {
        // Disable the rotation
        isEnabled = false;
    }
}
