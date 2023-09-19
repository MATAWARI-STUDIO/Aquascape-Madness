using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
 
public class RegularLiquidSurface : MonoBehaviour
{
    public ComputeShader computeShader;
    public Material material; 

    [Header ("Grid")]
    public int gridSize = 256;
    public float gridScale=1.0f;

    [Header ("Surface")]
    [Range(0.001f, 0.25f)]
    public float elasticity=0.05f;
    [Range(0.001f, 0.1f)]
    public float damping=0.001f;
    [Range(0f, 1f)]
    public float amplitudeScale=1f;

    [Header ("Impulse")]
    [Range(3f, 100f)]
    public float impulseRadius=30f;

    [Range(0f, 50f)]
    public float radiusVariance=0f;

    [Range(0f, 0.2f)]
    public float impulseEnergy=0.1f;


    [Range(0f, 1f)]
    public float directionVariance=0f;
    [Range(0f, 1f)]
    public float directionOffset=0f;

    [Range(0f, 1f)]
    public float impulseProbability=0.005f;

    //public bool syncToBeat=false;

    [Range(1, 20)]
    public int impulseSpread=10;

    ComputeBuffer mMeshTriangles;
    ComputeBuffer mMeshPositions;
    ComputeBuffer mInitialMeshPositions;
    ComputeBuffer mMeshVelocities;
    ComputeBuffer mMeshUVs;
    ComputeBuffer mMeshNormals;

    CommandBuffer mRenderCommandBuffer;
    CommandBuffer mDepthCommandBuffer;

    int mComputeLiquidKernel;
    int mRecalculateNormalsKernel;
    int mApplyVelocitiesKernel;
    int mImpulseKernel;
    uint mThreadGroupSize;
    Bounds mBounds;
    int mNumThreadGroupsPerAxis;
    int mTriangles;

    int mImpulseCountdown;
    int mImpulseX;
    int mImpulseZ;
    int mImpulseRadius;
    Vector4 mImpulseVector;
    bool mImpulseTriggered;

    private int mCurrentBPMMultiplier;
    private float mLFOOffset;

    public ComputeBuffer getMeshNormals() { return mMeshNormals; }


    // Start is called before the first frame update
    void Start()
    {
        mComputeLiquidKernel = computeShader.FindKernel("LiquidCompute");
        mRecalculateNormalsKernel = computeShader.FindKernel("RecalculateNormals");
        mApplyVelocitiesKernel = computeShader.FindKernel("ApplyVelocity");
        mImpulseKernel = computeShader.FindKernel("Impulse");
        computeShader.GetKernelThreadGroupSizes(mComputeLiquidKernel, out mThreadGroupSize, out _, out _);
        mNumThreadGroupsPerAxis = Mathf.CeilToInt (gridSize / (float) mThreadGroupSize);
        //Debug.Log(mThreadGroupSize+" "+mNumThreadGroupsPerAxis);
        mBounds = new Bounds(Vector3.zero, Vector3.one * 200);
        initialiseMesh();
        computeShader.SetBuffer(mComputeLiquidKernel, "positions", mMeshPositions);
        computeShader.SetBuffer(mComputeLiquidKernel, "velocities", mMeshVelocities);

        computeShader.SetBuffer(mRecalculateNormalsKernel, "positions", mMeshPositions);
        computeShader.SetBuffer(mRecalculateNormalsKernel, "normals", mMeshNormals);

        computeShader.SetBuffer(mApplyVelocitiesKernel, "positions", mMeshPositions);
        computeShader.SetBuffer(mApplyVelocitiesKernel, "velocities", mMeshVelocities);

        computeShader.SetBuffer(mImpulseKernel, "velocities", mMeshVelocities);
    
        initialiseMaterial();
        mImpulseCountdown=0;

        mLFOOffset=Time.time;
    }

    void initialiseMesh () {
        int size=gridSize*gridSize;
        Vector3[] positions=new Vector3[size];
        Vector2[] uvs = new Vector2[size];

        //velocity
        int i=0;
        Vector3 zero=new Vector3(0,0,0);
        for (i=0; i<size; i++) {
            positions[i]=zero;
        }
        mMeshVelocities = new ComputeBuffer(size, sizeof(float) * 3);
        mMeshVelocities.SetData(positions);

        // Positions and UVs
        i=0;
        Vector2 c=new Vector2(gridSize/2,gridSize/2);
        for (int z = 0; z < gridSize; z++) {
            for (int x = 0; x < gridSize; x++) {
                /*
                Vector2 h=c-new Vector2(x,z);
                float d=Mathf.Sqrt(h.x*h.x+h.y*h.y)/(gridSize*0.5f);
                float r=0.2f;
                if (d>r) d=r;
                float v=(r-d)*4f;
                positions[i] = new Vector3((x/(gridSize - 1f))*2f-1f,v,(z/(gridSize - 1f))*2f-1f);
                */
                positions[i] = new Vector3((x/(gridSize - 1f))*2f-1f,0,(z/(gridSize - 1f))*2f-1f);

                uvs[i]=new Vector2(x/(gridSize-1f),z/(gridSize-1f));
                i++;
            }
        }
        mMeshPositions = new ComputeBuffer(size, sizeof(float) * 3);
        mMeshPositions.SetData(positions);
        mInitialMeshPositions = new ComputeBuffer(size, sizeof(float) * 3);
        mInitialMeshPositions.SetData(positions);
        mMeshUVs = new ComputeBuffer(size, sizeof(float)*2 );
        mMeshUVs.SetData(uvs);

        // Normals
        Vector3 up=new Vector3(0,1,0);
        for (i=0; i<size; i++) {
            positions[i]=up;
        }
        mMeshNormals = new ComputeBuffer(size, sizeof(float)*3 );
        mMeshNormals.SetData(positions);

        // Triangles
        int triangleSize=(gridSize - 1) * (gridSize - 1) * 6;
        int[] triangles = new int[triangleSize];
        int t=0;
        i=0;
        for (int z = 0; z < gridSize-1; z++) {
            for (int x = 0; x < gridSize-1; x++) {
                bool f=Random.value>0.5f;
                if (f )
                {
                    triangles[t + 0] = i + gridSize;
                    triangles[t + 1] = i + gridSize + 1;
                    triangles[t + 2] = i;

                    triangles[t + 3] = i + gridSize + 1;
                    triangles[t + 4] = i + 1;
                    triangles[t + 5] = i;
                }
                else
                {
                    triangles[t + 0] = i + gridSize + 1;
                    triangles[t + 1] = i  + 1;
                    triangles[t + 2] = i + gridSize;

                    triangles[t + 3] = i + 1;
                    triangles[t + 4] = i;
                    triangles[t + 5] = i + gridSize;
                }
                t += 6;
                i++;
            }
        }
    
        mMeshTriangles = new ComputeBuffer(triangleSize, sizeof(int) );
        mMeshTriangles.SetData(triangles);
        mTriangles=triangleSize;
    }

    void initialiseMaterial() {
        material.SetBuffer("Triangles", mMeshTriangles);
        material.SetBuffer("Positions", mMeshPositions);
        //material.SetBuffer("InitialPositions", mInitialMeshPositions);
        material.SetBuffer("UVs", mMeshUVs);
        material.SetBuffer("Normals", mMeshNormals);
        material.SetFloat("gridScale",gridScale);

        material.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
        material.SetMatrix("_WorldToLocal", transform.worldToLocalMatrix);
        material.SetFloat("globalOpacity",1.0f);

    }

    void OnDestroy()
    {
        mMeshTriangles.Dispose();
        mMeshPositions.Dispose();
        mInitialMeshPositions.Dispose();
        mMeshUVs.Dispose();
        mMeshNormals.Dispose();
    }

    void ImpulseInit() {
        int s=gridSize*gridSize;
        int px=(int)(Random.value*gridSize);
        int pz=(int)(Random.value*gridSize);
        float variance=(radiusVariance*Random.value-radiusVariance*0.5f);
        int k=(int)(impulseRadius+variance);
        if (k<2) k=2;
        float height=impulseEnergy/Mathf.Sqrt(k);

        
        float verticalOffsetAngle=(directionOffset+((1-directionOffset)*directionVariance)*Random.value)*0.5f*Mathf.PI; // 0=vertical, PI/2=horizontal
	    float rotation=Random.value*Mathf.PI*2;
	    float impulseYScale=Mathf.Cos(verticalOffsetAngle)*height;
	    float hScale=Mathf.Sin(verticalOffsetAngle)*height;
	    float impulseXScale=Mathf.Sin(rotation)*hScale;
	    float impulseZScale=Mathf.Cos(rotation)*hScale;
        if (Random.value>0.5f) impulseYScale=-impulseYScale;
        Vector3 impulse=new Vector3(impulseXScale,impulseYScale,impulseZScale)/impulseSpread;

        mImpulseX=px;
        mImpulseZ=pz;
        mImpulseVector=new Vector4(impulse.x,impulse.y,impulse.z,0);
        mImpulseRadius=k;
    }

    // Update is called once per frame
    void Update()
    {
        computeShader.SetInt("gridSize",gridSize);
        computeShader.SetFloat("gridScale",gridScale);
        computeShader.SetFloat("idealDistance",1f/(gridSize-1));
        computeShader.SetFloat("invDamp",1-damping);
        computeShader.SetFloat("elasticity",elasticity);
 
        if (mImpulseCountdown>0) {
            computeShader.SetFloat("impulseCountdown",mImpulseCountdown);
            computeShader.SetInt("impulseX",mImpulseX);
            computeShader.SetInt("impulseZ",mImpulseZ);
            computeShader.SetInt("impulseRadius",mImpulseRadius);
            computeShader.SetVector("impulseVector",mImpulseVector);

            computeShader.Dispatch(mImpulseKernel, mNumThreadGroupsPerAxis, 1, mNumThreadGroupsPerAxis);
            mImpulseCountdown--;
        }
        if (mImpulseCountdown==0 && Random.value<impulseProbability) {
            ImpulseInit();
            mImpulseCountdown=impulseSpread;
        }

        computeShader.Dispatch(mComputeLiquidKernel, mNumThreadGroupsPerAxis, 1, mNumThreadGroupsPerAxis);
        computeShader.Dispatch(mApplyVelocitiesKernel, mNumThreadGroupsPerAxis, 1, mNumThreadGroupsPerAxis);
        computeShader.Dispatch(mRecalculateNormalsKernel, mNumThreadGroupsPerAxis, 1, mNumThreadGroupsPerAxis);
    
        Graphics.DrawProcedural(material, mBounds, MeshTopology.Triangles, mMeshTriangles.count, 1,
            null, null
            //,ShadowCastingMode.TwoSided, true, gameObject.layer
        );
        
    }

}


