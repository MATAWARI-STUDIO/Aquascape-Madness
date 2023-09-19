// SelectionManager.cs

using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string selectableTag = "Selectable"; // Set the selectable tag in the inspector
    public Color outlineColor = Color.white;
    public float outlineWidth = 0.1f;

    private GameObject currentlySelectedObject;
    private Material outlineMaterial;
    private Material originalMaterial; // Store the original material of the selected object

    private bool isPlacingObject;

    private void Start()
    {
        outlineMaterial = new Material(Shader.Find("Standard"));
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
    }

    private void Update()
    {
        if (isPlacingObject)
        {
            // If we are placing an object, skip the selection process
            return;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(selectableTag))
            {
                if (currentlySelectedObject != hitObject)
                {
                    DeselectCurrentObject();
                    SelectObject(hitObject);
                }
            }
            else
            {
                DeselectCurrentObject();
            }
        }
        else
        {
            DeselectCurrentObject();
        }
    }

    private void SelectObject(GameObject obj)
    {
        currentlySelectedObject = obj;
        Renderer renderer = obj.GetComponent<Renderer>();
        originalMaterial = renderer.material; // Store the original material

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        mpb.SetColor("_OutlineColor", outlineColor);
        mpb.SetFloat("_OutlineWidth", outlineWidth);
        renderer.SetPropertyBlock(mpb);

        renderer.material = outlineMaterial;
    }

    private void DeselectCurrentObject()
    {
        if (currentlySelectedObject != null)
        {
            Renderer renderer = currentlySelectedObject.GetComponent<Renderer>();
            renderer.material = originalMaterial; // Restore the original material
            currentlySelectedObject = null;
        }
    }

    // Call this method from your ObjectPlacementController script when you start placing an object
    public void StartPlacingObject()
    {
        isPlacingObject = true;
    }

    // Call this method from your ObjectPlacementController script when you finish placing an object
    public void FinishPlacingObject()
    {
        isPlacingObject = false;
    }
}
