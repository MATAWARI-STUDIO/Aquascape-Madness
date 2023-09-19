using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Material waveMaterial;
    public ComputeShader waveCompute;
    public RenderTexture NState, Nm1State, Np1State;
    public Vector2Int resolution;

    public Vector3 effect;


    // Start is called before the first frame update
    void Start()
    {
        InitializeTexture(ref NState);
        InitializeTexture(ref Nm1State);
        InitializeTexture(ref Np1State);

        waveMaterial.mainTexture = NState;
    }

    void InitializeTexture (ref RenderTexture tex)
    {
        tex = new RenderTexture(resolution.x, resolution.y,1,UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SNorm);
        tex.enableRandomWrite = true;
        tex.Create();
    }
    // Update is called once per frame
    void Update()
    {
        Graphics.CopyTexture(NState, Nm1State);
        Graphics.CopyTexture(Np1State, NState);

        // Set the textures before dispatching the compute shader
        waveCompute.SetTexture(0, "NState", NState);
        waveCompute.SetTexture(0, "Nm1State", Nm1State);
        waveCompute.SetTexture(0, "Np1State", Np1State);

        // Set the vector variables
        waveCompute.SetVector("effect", effect);
        waveCompute.SetVector("resolution", new Vector2(resolution.x, resolution.y));

        waveCompute.Dispatch(0, resolution.x / 8, resolution.y / 8, 1);
    }
}
