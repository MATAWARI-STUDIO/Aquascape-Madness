using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectPlacementController : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public Transform spawnPositionEmpty;
    public LayerMask substrateLayer;
    public Camera placementCamera;
    public Material collidedMaterial;
    public Material hoverMaterial;
    public float rotationSpeed = 100f;
    public float scaleSpeed = 1f;
    public float minScale = 0.1f;
    public float maxScale = 10f;
    public GameObject shopPanel;
    public List<Button> plantButtons = new List<Button>();
    public Collider[] tankColliders;
    public static ObjectPlacementController instance;

    private GameObject spawnedObject;
    private bool isObjectSelected = false;
    private bool isLocked = false;
    private bool canInteract = false;
    private Renderer objectRenderer;
    private int selectedPrefabIndex = 0;
    private Material originalMaterial;
    private bool isObjectPlaced = false;
    private bool hasCollided = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsObjectBeingPlaced()
    {
        return isObjectSelected && canInteract && !isLocked;
    }

    public int SelectedPrefabIndex
    {
        get { return selectedPrefabIndex; }
        set { selectedPrefabIndex = value; }
    }

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
        foreach (Button plantButton in plantButtons)
        {
            plantButton.onClick.AddListener(OnPlantButtonClick);
        }
    }

    private void Update()
    {
        if (isObjectSelected && canInteract && !isLocked)
        {
            HandleObjectPlacement();
        }

        hasCollided = hasCollidedWithTank();

        if (objectRenderer != null)
        {
            if (hasCollided)
            {
                objectRenderer.material = collidedMaterial;
            }
            else
            {
                objectRenderer.material = originalMaterial;
            }
        }
        else
        {
            Debug.LogWarning("objectRenderer is null");
        }
    }

    private void HandleObjectPlacement()
    {
        Ray ray = placementCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, substrateLayer))
        {
            Vector3 targetPosition = hitInfo.point;
            spawnedObject.transform.position = targetPosition;
        }

        if (Input.GetMouseButtonDown(0) && !hasCollided)
        {
            isLocked = true;
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            canInteract = false;
            isObjectPlaced = true;
        }

        HandleObjectRotationAndScaling();
    }

    void HandleObjectRotationAndScaling()
    {
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        spawnedObject.transform.Rotate(Vector3.up, rotationY);

        float rotationX = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        spawnedObject.transform.Rotate(Vector3.right, rotationX);

        float scale = 0f;
        if (Input.GetKey(KeyCode.Q))
            scale = -scaleSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.E))
            scale = scaleSpeed * Time.deltaTime;

        Vector3 newScale = spawnedObject.transform.localScale + new Vector3(scale, scale, scale);
        newScale = Vector3.ClampMagnitude(newScale, maxScale);
        newScale = Vector3.Max(newScale, new Vector3(minScale, minScale, minScale));
        spawnedObject.transform.localScale = newScale;
    }

    private bool hasCollidedWithTank()
    {
        if (spawnedObject == null)
        {
            Debug.LogWarning("spawnedObject is null");
            return false;
        }

        Collider[] hitColliders = Physics.OverlapBox(spawnedObject.transform.position, spawnedObject.transform.localScale / 2, Quaternion.identity);
        foreach (var hitCollider in hitColliders)
        {
            foreach (var tankCollider in tankColliders)
            {
                if (hitCollider == tankCollider)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnPlantButtonClick()
    {
        int buttonIndex = plantButtons.IndexOf(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        if (buttonIndex >= 0 && buttonIndex < plantButtons.Count)
        {
            selectedPrefabIndex = buttonIndex;
            SpawnObject(Vector3.one);  // Here, pass the default scale Vector3.one or some other scale
        }
        shopPanel.SetActive(false);
    }


    public void SpawnObject(Vector3 scale)
    {
        if (selectedPrefabIndex < 0 || selectedPrefabIndex >= objectPrefabs.Length)
            return;

        GameObject prefab = objectPrefabs[selectedPrefabIndex];
        Vector3 originalPrefabScale = prefab.transform.localScale;

        Ray ray = placementCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        RaycastHit hitInfo;
        Vector3 spawnPosition = Vector3.zero;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, substrateLayer))
        {
            spawnPosition = spawnPositionEmpty.transform.position;
        }

        GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        newObject.transform.localScale = scale;

        spawnedObject = newObject;
        isObjectSelected = true;
        isLocked = false;
        canInteract = true;
        isObjectPlaced = false;
        hasCollided = false;

        objectRenderer = spawnedObject.GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
        objectRenderer.material = hoverMaterial;

        shopPanel.SetActive(false);
    }
}
