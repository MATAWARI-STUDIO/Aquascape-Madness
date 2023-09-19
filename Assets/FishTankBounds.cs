using UnityEngine;

public class FishTankBounds : MonoBehaviour
{
    private BoxCollider tankBounds;

    public float minX, maxX, minY, maxY, minZ, maxZ;

    void Start()
    {
        tankBounds = GetComponent<BoxCollider>();
        if (tankBounds == null)
        {
            Debug.LogError("Box Collider component not found on the tank GameObject. Please add a Box Collider to the tank.");
        }
        else
        {
            Vector3 center = transform.position + tankBounds.center;
            Vector3 size = tankBounds.size;

            // Calculate the min and max bounds of the tank
            minX = center.x - size.x / 2f;
            maxX = center.x + size.x / 2f;
            minY = center.y - size.y / 2f;
            maxY = center.y + size.y / 2f;
            minZ = center.z - size.z / 2f;
            maxZ = center.z + size.z / 2f;
        }
    }

    public Vector3 GetRandomPositionWithinBounds()
    {
        // Calculate a random position within the bounds of the tank
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        float z = Random.Range(minZ, maxZ);

        return new Vector3(x, y, z);
    }
}
