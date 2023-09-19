using UnityEngine;

public class GridOnSurface : MonoBehaviour
{
    public Transform substrate; // Reference to the substrate (uneven mesh) GameObject
    public float gridSpacing = 1.0f; // Spacing between grid lines
    public int gridSizeX = 10; // Number of grid lines in the X-axis
    public int gridSizeY = 10; // Number of grid lines in the Y-axis

    private MeshFilter meshFilter;
    private Mesh gridMesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        gridMesh = new Mesh();
        meshFilter.mesh = gridMesh;

        GenerateGrid();
    }

    void GenerateGrid()
    {
        int numVerticesX = gridSizeX + 1;
        int numVerticesY = gridSizeY + 1;

        Vector3[] vertices = new Vector3[numVerticesX * numVerticesY];
        int[] triangles = new int[gridSizeX * gridSizeY * 2 * 3]; // Each grid square has 2 triangles

        for (int x = 0; x < numVerticesX; x++)
        {
            for (int y = 0; y < numVerticesY; y++)
            {
                Vector3 gridPoint = new Vector3(x * gridSpacing, 0.0f, y * gridSpacing);
                RaycastHit hit;
                if (Physics.Raycast(gridPoint + Vector3.up * 100.0f, Vector3.down, out hit))
                {
                    gridPoint = hit.point; // Project grid point onto the substrate
                }

                vertices[x + y * numVerticesX] = gridPoint;

                // Generate triangles for the grid square (forming 2 triangles per square)
                if (x < gridSizeX && y < gridSizeY)
                {
                    int squareIndex = (x + y * gridSizeX) * 2 * 3; // 2 triangles per grid square

                    // First triangle
                    triangles[squareIndex] = x + y * numVerticesX;
                    triangles[squareIndex + 1] = (x + 1) + y * numVerticesX;
                    triangles[squareIndex + 2] = x + (y + 1) * numVerticesX;

                    // Second triangle
                    triangles[squareIndex + 3] = (x + 1) + y * numVerticesX;
                    triangles[squareIndex + 4] = (x + 1) + (y + 1) * numVerticesX;
                    triangles[squareIndex + 5] = x + (y + 1) * numVerticesX;
                }
            }
        }

        gridMesh.vertices = vertices;
        gridMesh.triangles = triangles;

        // Update normals to ensure correct lighting (optional)
        gridMesh.RecalculateNormals();
    }
}
