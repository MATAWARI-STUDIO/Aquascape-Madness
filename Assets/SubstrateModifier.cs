using System.IO;
using UnityEngine;

public class SubstrateModifier : MonoBehaviour
{
    public float modificationStrength = 0.1f;
    public float radius = 0.1f;
    public ParticleSystem particleSystem; // Reference to your particle system
    public string saveFilePath = "Assets/SavedMesh.dat"; // File path to save the mesh

    private void Start()
    {
        ParticleSystem.CollisionModule collisionModule = particleSystem.collision;
        collisionModule.enabled = true;
        collisionModule.SetPlane(0, transform);

        // Load saved mesh if exists
        LoadMesh();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!TimeController.IsGamePausedOrFastForwarded())
        {
            ModifySubstrate(other.transform.position);
        }
    }

    private void ModifySubstrate(Vector3 collisionPoint)
    {
        if (!TimeController.IsGamePausedOrFastForwarded())
        {
            RaycastHit hit;

            if (Physics.Raycast(collisionPoint, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Substrate")))
            {
                Mesh mesh = hit.collider.GetComponent<MeshFilter>().mesh;
                Vector3[] vertices = mesh.vertices;

                Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);

                // Define a minimum y-value for vertices to be modified
                float minYValue = 0f;

                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].y >= minYValue)
                    {
                        float distance = Vector3.Distance(vertices[i], localPoint);

                        if (distance < radius)
                        {
                            float falloff = 1f - Mathf.Clamp01(distance / radius);
                            vertices[i].y += modificationStrength * falloff * Time.deltaTime;
                        }
                    }
                }

                mesh.vertices = vertices;
                mesh.RecalculateNormals();

                // Save the modified mesh
                SaveMesh(mesh);
            }
        }
    }



    private void SaveMesh(Mesh mesh)
    {
        // Implement your mesh serialization logic here
        // Write the mesh data to 'saveFilePath'
    }

    private void LoadMesh()
    {
        // Implement your mesh deserialization logic here
        // Read the mesh data from 'saveFilePath' and apply it
    }
}
