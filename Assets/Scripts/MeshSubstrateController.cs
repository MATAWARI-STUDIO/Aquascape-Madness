using UnityEngine;
using UnityEngine.UI;

public class MeshSubstrateController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Button modifyVerticesButton;

    public float raiseLowerSpeed = 1f;
    public float maxHeight = 5f;
    public float minHeight = 0f;
    public float cursorRadius = 0.5f;

    public GameObject cursor;
    public Color raiseColor = Color.green;
    public Color lowerColor = Color.red;
    public float cursorCutOffset = 0.05f;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private bool isRaising;
    private bool isLowering;
    private bool raiseToggle = true;
    private bool lowerToggle;
    private Renderer cursorRenderer;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        cursorRenderer = cursor.GetComponent<Renderer>();

        if (modifyVerticesButton != null)
        {
            modifyVerticesButton.onClick.AddListener(OnModifyVerticesButtonClick);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            raiseToggle = true;
            lowerToggle = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            lowerToggle = true;
            raiseToggle = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isRaising = raiseToggle;
            isLowering = lowerToggle;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRaising = false;
            isLowering = false;
        }

        MoveCursor();

        if (raiseToggle)
        {
            cursorRenderer.material.color = raiseColor;
        }
        else if (lowerToggle)
        {
            cursorRenderer.material.color = lowerColor;
        }
    }

    private void MoveCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            cursor.transform.position = raycastHit.point;

            if (isRaising || isLowering)
            {
                ModifyVertices();
            }
        }
    }

private void ModifyVertices()
{
    // Get the mesh from the MeshFilter
    Mesh mesh = meshFilter.mesh;

    // Get the world position of the cursor
    Vector3 cursorWorldPosition = cursor.transform.position;

    // Transform the cursor position to local space
    Vector3 cursorLocalPosition = transform.InverseTransformPoint(cursorWorldPosition);

    // Modify the vertices based on the cursor position and input
    Vector3[] vertices = mesh.vertices;

    for (int i = 0; i < vertices.Length; i++)
    {
        Vector3 vertexLocalPosition = vertices[i];

        // Calculate the distance from the vertex to the cursor
        float distance = Vector2.Distance(new Vector2(vertexLocalPosition.x, vertexLocalPosition.z), new Vector2(cursorLocalPosition.x, cursorLocalPosition.z));

        // Calculate the height modification based on the distance from the cursor and input
        float normalizedDistance = distance / cursorRadius;
        float heightModification = (isRaising ? 1f : -1f) * raiseLowerSpeed * Time.deltaTime * (1f - Mathf.Clamp01(Mathf.Pow(normalizedDistance, 2f)));

        // Apply the height modification to the vertex
        vertexLocalPosition.y += heightModification;

        // If the vertex is at the sides (x or z position equals to the minimum or maximum bounds),
        // set the y position to 0 to keep the vertex at the bottom
        if (Mathf.Approximately(vertexLocalPosition.x, mesh.bounds.min.x) ||
            Mathf.Approximately(vertexLocalPosition.x, mesh.bounds.max.x) ||
            Mathf.Approximately(vertexLocalPosition.z, mesh.bounds.min.z) ||
            Mathf.Approximately(vertexLocalPosition.z, mesh.bounds.max.z))
        {
            vertexLocalPosition.y = 0f;
        }
        else if (Mathf.Approximately(vertexLocalPosition.x, 0f) && Mathf.Approximately(vertexLocalPosition.z, 0f))
        {
            // If the vertex is at the center, raise it with a round shape
            float angle = Mathf.Atan2(vertexLocalPosition.z, vertexLocalPosition.x);
            float radius = Mathf.Sqrt(vertexLocalPosition.x * vertexLocalPosition.x + vertexLocalPosition.z * vertexLocalPosition.z);
            float centerHeight = Mathf.Lerp(minHeight, maxHeight, radius / cursorRadius);
            vertexLocalPosition.y = centerHeight * Mathf.Sin(angle);
        }

        // Clamp the height within the specified range
        vertexLocalPosition.y = Mathf.Clamp(vertexLocalPosition.y, minHeight, maxHeight);

        vertices[i] = vertexLocalPosition;
    }

    mesh.vertices = vertices;

    // Refresh the mesh and collider to reflect the modified vertices
    mesh.RecalculateBounds();
    mesh.RecalculateNormals();
    meshCollider.sharedMesh = mesh;
}


    public void OnModifyVerticesButtonClick()
    {
        ModifyVertices();
    }
}
