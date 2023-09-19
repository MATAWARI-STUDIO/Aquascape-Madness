using UnityEngine;

public class DecalProjectorController : MonoBehaviour
{
    public LayerMask substrateLayer; // Set this layer in the Inspector to ensure raycast only hits the substrate

    private Projector projectorComponent;

    private void Start()
    {
        projectorComponent = GetComponent<Projector>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, substrateLayer))
        {
            projectorComponent.enabled = true;
            Vector3 decalPosition = hit.point + (hit.normal * 0.01f); // Offset slightly from the hit point to avoid Z-fighting
            Quaternion decalRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);

            transform.position = decalPosition;
            transform.rotation = decalRotation;
        }
        else
        {
            projectorComponent.enabled = false;
        }
    }
}
