using UnityEngine;

public class BrushGizmo : MonoBehaviour
{
    public LayerMask substrateLayer; // Set this layer in the Inspector to ensure raycast only hits the substrate
    public float gizmoSize = 1.0f; // Adjust the size of the gizmo

    private MeshRenderer gizmoRenderer;

    private void Awake()
    {
        gizmoRenderer = GetComponent<MeshRenderer>();
        gizmoRenderer.enabled = false;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform a raycast only against objects on the specified layer for the substrate
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, substrateLayer))
        {
            gizmoRenderer.enabled = true;
            transform.position = hit.point + (hit.normal * 0.01f); // Offset slightly from the hit point to avoid Z-fighting
            transform.forward = -hit.normal; // Point the gizmo towards the hit surface normal

            // Scale the gizmo to match the specified size
            transform.localScale = Vector3.one * gizmoSize;
        }
        else
        {
            gizmoRenderer.enabled = false;
        }
    }
}
