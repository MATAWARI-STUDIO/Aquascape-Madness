using UnityEngine;

public class SubstrateAccumulator : MonoBehaviour
{
    public int gridSize = 11;
    public float cellSize = 0.5f;
    public MeshFilter tankBottomMeshFilter;
    public GameObject smokeParticlePrefab;
    private Mesh tankBottomMesh;
    private float[,] substrateHeight;

    private void Start()
    {
        if (tankBottomMeshFilter == null)
        {
            Debug.LogError("Tank Bottom Mesh Filter is not assigned!");
            return;
        }

        substrateHeight = new float[gridSize, gridSize];
        tankBottomMesh = tankBottomMeshFilter.mesh;
    }

    public void Accumulate(Vector3 position)
    {
        Debug.Log($"Accumulating at position: {position}");

        if (smokeParticlePrefab == null)
        {
            Debug.LogError("Smoke particle prefab is not assigned!");
            return;
        }

        Instantiate(smokeParticlePrefab, position + new Vector3(0, 0.1f, 0), Quaternion.identity);

        int x = Mathf.FloorToInt(position.x / cellSize);
        int z = Mathf.FloorToInt(position.z / cellSize);
        Debug.Log($"Grid coordinates: x = {x}, z = {z}");

        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
        {
            substrateHeight[x, z] += 0.01f;

            if (tankBottomMesh != null)
            {
                Vector3[] vertices = tankBottomMesh.vertices;
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 localPos = tankBottomMeshFilter.transform.InverseTransformPoint(vertices[i]);
                    int vertexX = Mathf.FloorToInt(localPos.x / cellSize);
                    int vertexZ = Mathf.FloorToInt(localPos.z / cellSize);

                    if (vertexX == x && vertexZ == z)
                    {
                        vertices[i].y += 0.01f;
                    }
                }
                tankBottomMesh.vertices = vertices;
                tankBottomMesh.RecalculateBounds();
                tankBottomMesh.RecalculateNormals();

                MeshCollider meshCollider = tankBottomMeshFilter.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.sharedMesh = null;
                    meshCollider.sharedMesh = tankBottomMesh;
                }
            }
            else
            {
                Debug.LogError("Tank bottom mesh is null.");
            }
        }
        else
        {
            Debug.LogWarning("Coordinates out of grid bounds.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }

}
