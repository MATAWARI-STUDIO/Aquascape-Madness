using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CausticGenerator : MonoBehaviour
{
    public RegularLiquidSurface surface;
    public ComputeShader computeShader;

    public Material materialForNormalMap;
    public Material materialForCaustics;
    
    public Light LightToApplyCookie;
    //public int blurTextureSize=128;
    public int causticTextureSize=128;
    public float depthOfLiquid=1f;
    public float refractiveIndex=0.9f;
    public float brightness=0.5f;
    public float gamma=1.0f;
    public Vector3 lightDirection=new Vector3(0,-1,0);

    private RenderTexture normalTexture;
    private RenderTexture causticsTexture;
    private RenderTexture blurCausticsTexture;
    private ComputeBuffer causticHits;

    private int setTextureKernel;
    private int mNumThreadGroupsPerAxis;
    private int renderCausticsKernel;
    private int blurCausticsKernel;

    // Start is called before the first frame update
    void Start()
    {
        normalTexture = new RenderTexture(causticTextureSize, causticTextureSize, 16);
        normalTexture.enableRandomWrite = true;
        normalTexture.Create();

        causticsTexture = new RenderTexture(causticTextureSize, causticTextureSize, 16);
        causticsTexture.enableRandomWrite = true;
        causticsTexture.Create();

        blurCausticsTexture = new RenderTexture(causticTextureSize, causticTextureSize, 24);
        blurCausticsTexture.enableRandomWrite = true;
        blurCausticsTexture.Create();

        causticHits = new ComputeBuffer(causticTextureSize*causticTextureSize, sizeof(float) );

        setTextureKernel = computeShader.FindKernel("SetTextureFromMesh");
        computeShader.SetTexture(setTextureKernel,"NormalTexture",normalTexture);
        computeShader.SetTexture(setTextureKernel,"CausticsTexture",causticsTexture);
        computeShader.SetBuffer(setTextureKernel, "CausticHits", causticHits);

        renderCausticsKernel = computeShader.FindKernel("RenderCaustics");
        computeShader.SetTexture(renderCausticsKernel,"NormalTexture",normalTexture);
        computeShader.SetTexture(renderCausticsKernel,"CausticsTexture",causticsTexture);
        computeShader.SetBuffer(renderCausticsKernel, "CausticHits", causticHits);

        blurCausticsKernel = computeShader.FindKernel("BlurCaustics");
        computeShader.SetTexture(blurCausticsKernel,"CausticsTexture",causticsTexture);
        computeShader.SetTexture(blurCausticsKernel,"BlurCausticsTexture",blurCausticsTexture);
        computeShader.SetBuffer(blurCausticsKernel, "CausticHits", causticHits);

        
        uint threadGroupSize;
        computeShader.GetKernelThreadGroupSizes(setTextureKernel, out threadGroupSize, out _, out _);
        mNumThreadGroupsPerAxis = Mathf.CeilToInt ((float) causticTextureSize / (float) threadGroupSize);
    }

    void OnDestroy()
    {
        normalTexture.Release();
        causticsTexture.Release();
        blurCausticsTexture.Release();
    }

    // Update is called once per frame
    void Update()
    {
        //compute normal map from mesh
        ComputeBuffer normals=surface.getMeshNormals();
        float meshSize=Mathf.Sqrt(normals.count);
        float scaling=meshSize/(float)causticTextureSize;
        //Debug.Log("Scaling="+scaling);
        computeShader.SetBuffer(setTextureKernel,"Normals",normals);
        computeShader.SetInt("NormalsSize",(int)meshSize);
        computeShader.SetFloat("Scaling",scaling);

        computeShader.Dispatch(setTextureKernel, mNumThreadGroupsPerAxis, mNumThreadGroupsPerAxis,1 );
        if (materialForNormalMap!=null) materialForNormalMap.SetTexture("_MainTex", normalTexture);

        //compute caustics from normal map
        computeShader.SetFloat("DepthOfLiquid",depthOfLiquid);
        computeShader.SetFloat("RefractiveIndex",refractiveIndex);
        computeShader.SetFloat("Brightness",brightness*0.001f);
        computeShader.SetFloat("Gamma",gamma);
        Vector3 ld=lightDirection;//.normalized;
        computeShader.SetVector("LightDirection",new Vector4(ld.x,ld.z,ld.y,0f));
        //float cs=(float)causticTextureSize/(float)blurTextureSize;
        //Debug.Log("caustic scaling="+cs);
        //computeShader.SetFloat("CausticScaling",cs);
        

        computeShader.Dispatch(renderCausticsKernel, mNumThreadGroupsPerAxis, mNumThreadGroupsPerAxis,1 );
        //materialForCaustics.SetTexture("_MainTex", causticsTexture);

        computeShader.SetInt("TextureSize",causticTextureSize);
        computeShader.Dispatch(blurCausticsKernel, mNumThreadGroupsPerAxis, mNumThreadGroupsPerAxis,1 );
        if (materialForCaustics!=null) materialForCaustics.SetTexture("_MainTex", blurCausticsTexture);


        if (LightToApplyCookie!=null)
        {
            LightToApplyCookie.cookie=blurCausticsTexture;
        }

    }
}
