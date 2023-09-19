using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook freeLookCamera;
    public float rotationSpeed = 2f;
    public float moveSpeed = 10f;

    private bool isCameraMoving = false;

    private void Update()
    {
        // Check if the right mouse button is pressed
        bool isRightMouseButtonDown = Input.GetMouseButton(1);

        // Check if the right mouse button was pressed in the previous frame
        bool wasRightMouseButtonDownPrevFrame = isCameraMoving;

        if (isRightMouseButtonDown && !wasRightMouseButtonDownPrevFrame)
        {
            // Enable camera movement
            isCameraMoving = true;
        }
        else if (!isRightMouseButtonDown && wasRightMouseButtonDownPrevFrame)
        {
            // Disable camera movement
            isCameraMoving = false;
        }

        RotateCamera();
        MoveCamera();
    }

    private void RotateCamera()
    {
        // If the camera is moving, rotate the camera based on mouse movement
        if (isCameraMoving)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            freeLookCamera.m_XAxis.Value += mouseX;
            freeLookCamera.m_YAxis.Value -= mouseY;
        }
    }

    private void MoveCamera()
    {
        // If the camera is moving, move the camera based on WASD or arrow key input
        if (isCameraMoving)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput) * moveSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }
    }
}
