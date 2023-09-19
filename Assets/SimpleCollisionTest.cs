using UnityEngine;

public class SimpleCollisionTest : MonoBehaviour
{
    public GameObject tank; // Assign the tank GameObject in the Unity Editor
    public Material collidedMaterial; // Assign the material to change to when collision detected
    private Material originalMaterial; // To store the original material

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Simple movement controls for testing
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime * 5f;
        transform.Translate(new Vector3(moveX, 0, moveY));

        // Check for collision
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        bool hasCollided = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == tank)
            {
                hasCollided = true;
                break;
            }
        }

        // Change material if collided
        if (hasCollided)
        {
            GetComponent<Renderer>().material = collidedMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
    }
}
