using UnityEngine;

public class FreeLookCameraController : MonoBehaviour
{
    public Transform target; // The target object the camera will orbit around
    public float rotationSpeed = 3f; // Speed of camera rotation
    public float upperViewAngle = 60f; // Angle for the upper view
    public float bottomViewAngle = 30f; // Angle for the bottom view
    public float zoomSpeed = 5f; // Speed of camera zoom
    public float maxZoomInDistance = 0.5f; // Maximum zoom-in distance
    public float maxZoomOutDistance = 10f; // Maximum zoom-out distance

    private float mouseX; // Mouse X position for rotation
    private float mouseY; // Mouse Y position for upper/bottom view
    private float currentZoomDistance; // Current distance of the camera from the target

    private void Start()
    {
        currentZoomDistance = Vector3.Distance(transform.position, target.position); // Set initial zoom distance
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            mouseX = 0f;
            mouseY = 0f;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Zoom in
        if (scroll > 0)
        {
            currentZoomDistance -= zoomSpeed * Time.deltaTime;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, maxZoomInDistance, maxZoomOutDistance);
        }
        // Zoom out
        else if (scroll < 0)
        {
            currentZoomDistance += zoomSpeed * Time.deltaTime;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, maxZoomInDistance, maxZoomOutDistance);
        }
    }

    private void UpdateCameraPosition()
    {
        float rotationAmountX = -mouseY * rotationSpeed;
        float rotationAmountY = mouseX * rotationSpeed;

        Vector3 currentRotation = transform.eulerAngles;
        float newRotationX = currentRotation.x + rotationAmountX;

        if (newRotationX > 180)
            newRotationX -= 360;

        float clampedRotationX = Mathf.Clamp(newRotationX, -bottomViewAngle, upperViewAngle);

        // Update the regular camera's position and rotation instead of the Cinemachine camera
        Camera.main.transform.rotation = Quaternion.Euler(clampedRotationX, currentRotation.y + rotationAmountY, currentRotation.z);
        Camera.main.transform.position = target.position - Camera.main.transform.forward * currentZoomDistance;
    }
}
