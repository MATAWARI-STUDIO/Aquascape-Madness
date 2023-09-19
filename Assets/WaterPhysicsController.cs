using UnityEngine;

public class WaterPhysicsController : MonoBehaviour
{
    public ComputeShader WaterPhysicsShader;
    private RenderTexture waterHeightTexture;
    private RenderTexture prevWaterHeightTexture;
    private float deltaTime;

    // Dimensions of the water surface mesh
    public int meshWidth = 256; // Adjust as needed
    public int meshHeight = 256; // Adjust as needed

    void Start()
    {
        waterHeightTexture = new RenderTexture(meshWidth, meshHeight, 0, RenderTextureFormat.RFloat);
        waterHeightTexture.enableRandomWrite = true; // Set UAV flag
        waterHeightTexture.Create();

        prevWaterHeightTexture = new RenderTexture(meshWidth, meshHeight, 0, RenderTextureFormat.RFloat);
        prevWaterHeightTexture.enableRandomWrite = true; // Set UAV flag
        prevWaterHeightTexture.Create();

        // Initialize other variables
        deltaTime = Time.deltaTime;
    }

    void Update()
    {
        int kernelHandle = WaterPhysicsShader.FindKernel("CSMain");
        Debug.Log("Kernel Handle: " + kernelHandle);
        WaterPhysicsShader.SetTexture(kernelHandle, "Result", waterHeightTexture);
        WaterPhysicsShader.SetTexture(kernelHandle, "PrevResult", prevWaterHeightTexture);
        WaterPhysicsShader.SetFloat("deltaTime", deltaTime);

        // Set thread group parameters
        int threadGroupsX = Mathf.CeilToInt(meshWidth / 16f); // Adjust as needed
        int threadGroupsY = Mathf.CeilToInt(meshHeight / 16f); // Adjust as needed
        int threadGroupsZ = 1;

        WaterPhysicsShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, threadGroupsZ);

        // Swap current and previous textures
        RenderTexture temp = waterHeightTexture;
        waterHeightTexture = prevWaterHeightTexture;
        prevWaterHeightTexture = temp;
    }
}
