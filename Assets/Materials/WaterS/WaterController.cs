using UnityEngine;

public class WaterController : MonoBehaviour
{
    public ComputeShader WaterComputeShader;
    private RenderTexture renderTexture;

    void Start()
    {
        renderTexture = new RenderTexture(256, 256, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
    }

    void Update()
    {
        int kernelHandle = WaterComputeShader.FindKernel("CSMain");
        WaterComputeShader.SetTexture(kernelHandle, "Result", renderTexture);
        WaterComputeShader.Dispatch(kernelHandle, 256 / 8, 256 / 8, 1);
    }
}
