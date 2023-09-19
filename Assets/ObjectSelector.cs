using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private Material highlightMaterial;
    private Transform currentlySelectedObject;
    private Material hoverMaterial;

    private void Start()
    {
        hoverMaterial = new Material(highlightMaterial);
        hoverMaterial.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
        {
            Transform hoveredObject = hit.transform;

            if (currentlySelectedObject != hoveredObject)
            {
                // Reset the material of the previously selected object or hovered object
                ResetMaterial(currentlySelectedObject);
                ResetMaterial(hoveredObject);

                // Change the material of the hovered object
                SetMaterial(hoveredObject, hoverMaterial);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (currentlySelectedObject != null)
                {
                    // Reset the material of the previously selected object
                    ResetMaterial(currentlySelectedObject);
                }

                // Change the material of the selected object
                SetMaterial(hoveredObject, highlightMaterial);
                currentlySelectedObject = hoveredObject;
            }
        }
        else
        {
            if (currentlySelectedObject != null)
            {
                // Reset the material of the previously selected object
                ResetMaterial(currentlySelectedObject);
                currentlySelectedObject = null;
            }
        }
    }

    private void SetMaterial(Transform objTransform, Material material)
    {
        if (objTransform == null) return;
        Renderer renderer = objTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    private void ResetMaterial(Transform objTransform)
    {
        if (objTransform == null) return;
        Renderer renderer = objTransform.GetComponent<Renderer>();
        if (renderer != null && renderer.material != highlightMaterial)
        {
            renderer.material = highlightMaterial;
        }
    }
}
