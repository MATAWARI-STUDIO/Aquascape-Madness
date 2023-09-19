using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public GameObject baseObject; // Reference to the Base GameObject (attach it in the Inspector)
    public GameObject glass; // Reference to the Glass GameObject (attach it in the Inspector)
    public float meshHeight = 0.1f; // Adjust this value to set the maximum height of the substrate
    public int gridSize = 10; // Adjust this value to set the grid size of the substrate

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Start()
    {
        meshFilter = baseObject.GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        GenerateRandomMesh();
    }

    void GenerateRandomMesh()
    {
        Vector3[] vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        int[] triangles = new int[gridSize * gridSize * 6];

        float halfSize = glass.transform.localScale.x / 2f;
        Vector3 startPos = new Vector3(-halfSize, 0f, -halfSize);

        for (int z = 0, i = 0; z <= gridSize; z++)
        {
            for (int x = 0; x <= gridSize; x++, i++)
            {
                float y = Random.Range(0f, meshHeight);
                vertices[i] = new Vector3(startPos.x + x * gridSize, y, startPos.z + z * gridSize);
            }
        }

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                triangles[tris] = vert + 0;
                triangles[tris + 1] = vert + gridSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + gridSize + 1;
                triangles[tris + 5] = vert + gridSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
