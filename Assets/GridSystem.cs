using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject substrate; // Reference to the substrate GameObject
    public int gridSizeX = 10; // Number of cells in the X-axis
    public int gridSizeY = 10; // Number of cells in the Y-axis

    private Vector3[,] grid; // Store the positions of each cell

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        // Access the mesh information from the substrate GameObject
        Mesh mesh = substrate.GetComponent<MeshFilter>().sharedMesh;

        grid = new Vector3[gridSizeX, gridSizeY];

        // Iterate over the grid cells
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calculate the normalized position within the grid (0 to 1)
                float u = (float)x / (gridSizeX - 1);
                float v = (float)y / (gridSizeY - 1);

                // Calculate the corresponding vertex index on the mesh
                int vertexIndex = GetVertexIndex(u, v, mesh);

                // Get the vertex position from the mesh
                Vector3 vertexPosition = mesh.vertices[vertexIndex];

                // Transform the vertex position to world space
                Vector3 cellPosition = substrate.transform.TransformPoint(vertexPosition);

                // Store the cell position
                grid[x, y] = cellPosition;

                // Instantiate a visual representation of the cell (optional)
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.position = cellPosition;
                cell.transform.localScale = Vector3.one * 0.1f; // Adjust the cell size as desired
                cell.transform.parent = transform;
            }
        }
    }

    private int GetVertexIndex(float u, float v, Mesh mesh)
    {
        // Find the corresponding vertex index based on the normalized grid position
        int vertexIndex = 0;

        // Find the vertex closest to the grid position
        float minDistance = float.MaxValue;
        Vector2 gridPosition = new Vector2(u, v);

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector2 vertexPosition = new Vector2(mesh.uv[i].x, mesh.uv[i].y);
            float distance = Vector2.Distance(gridPosition, vertexPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                vertexIndex = i;
            }
        }

        return vertexIndex;
    }
}
