using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    public float deformationStrength = 0.1f; // The strength of the deformation
    public float maxDeformationDepth = 1.0f; // The maximum depth the substrate can be deformed

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button is held down
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (meshCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Get the point of collision with the substrate
                Vector3 point = hit.point;
                // Apply deformation to the substrate mesh
                DeformMesh(point);
            }
        }
    }

    private void DeformMesh(Vector3 point)
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(vertices[i], point);
            float influence = Mathf.Clamp01(1.0f - distance / maxDeformationDepth);

            vertices[i] += Vector3.up * deformationStrength * influence;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;
    }
}
