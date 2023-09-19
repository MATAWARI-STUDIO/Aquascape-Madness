using UnityEngine;

public class DecalRaycatser : MonoBehaviour
{
    public GameObject decalPrefab; // Assign the decal prefab to this variable
    public LayerMask substrateLayer; // Set this layer in the Inspector to ensure raycast only hits the substrate

    private GameObject activeDecal;
    private Transform cursorTransform;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, substrateLayer))
        {
            if (activeDecal == null)
            {
                CreateDecal(hit.point, hit.normal);
            }
            else
            {
                MoveDecal(hit.point, hit.normal);
            }

            cursorTransform = transform;
        }
        else
        {
            DestroyDecal();
        }
    }

    private void CreateDecal(Vector3 position, Vector3 normal)
    {
        if (decalPrefab != null)
        {
            activeDecal = Instantiate(decalPrefab, position + (normal * 0.01f), Quaternion.LookRotation(-normal));
        }
    }

    private void MoveDecal(Vector3 position, Vector3 normal)
    {
        if (activeDecal != null)
        {
            activeDecal.transform.position = position + (normal * 0.01f);
            activeDecal.transform.rotation = Quaternion.LookRotation(-normal);
        }
    }

    private void DestroyDecal()
    {
        if (activeDecal != null)
        {
            Destroy(activeDecal);
            activeDecal = null;
        }
    }

    private void LateUpdate()
    {
        if (cursorTransform != null)
        {
            transform.position = cursorTransform.position;
        }
    }
}
