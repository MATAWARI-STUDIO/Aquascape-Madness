using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float rotationSpeed = 2f;

    private bool isRotating = false;
    private float mouseX;

    private void Update()
    {
        // Check for RMB input
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            Cursor.lockState = CursorLockMode.None;
        }

        // Handle camera rotation while RMB is held down
        if (isRotating)
        {
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            freeLookCamera.m_XAxis.Value = mouseX;
        }
    }
}
