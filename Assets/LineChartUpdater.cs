using UnityEngine;

public class LineChartUpdater : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Start()
    {
        // Call a method here to update the positions of the line
        UpdateLinePositions();
    }

    void UpdateLinePositions()
    {
        // Clear existing positions
        lineRenderer.positionCount = 0;

        // Add new positions
        Vector3[] positions = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),   // Start position
            new Vector3(1f, 0f, 0f),   // Next position
            new Vector3(2f, 1f, 0f),   // Another position
            // ... add more positions as needed
        };

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}
