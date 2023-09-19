using UnityEngine;

public class GizmoController : MonoBehaviour
{
    public Material gizmoMaterial; // Assign the gizmo material in the Inspector

    private Renderer substrateRenderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private void Start()
    {
        substrateRenderer = GetComponent<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // Set the gizmo material to the substrate mesh at the hit point
                materialPropertyBlock.SetTexture("_MainTex", gizmoMaterial.GetTexture("_MainTex"));
                substrateRenderer.SetPropertyBlock(materialPropertyBlock);
            }
            else
            {
                // Reset the material property block when not hovering over the substrate
                substrateRenderer.SetPropertyBlock(null);
            }
        }
    }

    private void OnDestroy()
    {
        // Reset the material property block when the GameObject is destroyed
        substrateRenderer.SetPropertyBlock(null);
    }
}
