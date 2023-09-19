using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    private void Update()
    {
        // Move the CursorFollower to the mouse cursor position
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = cursorPosition;
    }
}
