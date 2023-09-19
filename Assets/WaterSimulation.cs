using UnityEngine;

public class WaterSimulation : MonoBehaviour
{
    public int numParticles = 64;
    public ComputeShader particleSimulationCS;

    private ComputeBuffer particleBuffer;
    private int kernelIndex;

    private struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
    }

    private void Start()
    {
        InitializeParticles();
        kernelIndex = particleSimulationCS.FindKernel("CSMain");
    }

    private void Update()
    {
        SimulateParticles();
        RenderParticles();
    }

    private void InitializeParticles()
    {
        Particle[] particles = new Particle[numParticles];

        for (int i = 0; i < numParticles; i++)
        {
            particles[i].position = new Vector3(i, 0, 0);
            particles[i].velocity = Vector3.zero;
        }

        particleBuffer = new ComputeBuffer(numParticles, sizeof(float) * 6);
        particleBuffer.SetData(particles);
        particleSimulationCS.SetBuffer(kernelIndex, "particleBuffer", particleBuffer);
    }

    private void SimulateParticles()
    {
        particleSimulationCS.Dispatch(kernelIndex, numParticles / 64, 1, 1);
    }

    private void RenderParticles()
    {
        // Set particle positions to the shader
        Shader.SetGlobalBuffer("particleBuffer", particleBuffer);

        // Dispatch draw command for particles
        Graphics.DrawProceduralNow(MeshTopology.Points, numParticles);
    }

    private void OnDestroy()
    {
        particleBuffer.Release();
    }
}
