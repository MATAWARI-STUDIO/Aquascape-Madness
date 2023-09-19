using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
// from https://docs.unity3d.com/ScriptReference/Camera.RenderToCubemap.html
public class CubeMapRefraction : MonoBehaviour
{
    // Attach this script to an object that uses a Reflective shader.
    // Realtime reflective cubemaps!

    int cubemapSize = 4096;
    bool oneFacePerFrame = true;
    Camera cam;
    RenderTexture renderTexture;


    void Start()
    {
		//Debug.Log("CubeMapRefraction");
        renderTexture=null;
        cam=null;
        // render all six faces at startup
        UpdateCubemap(63);
    }

    void OnDisable()
    {
        DestroyImmediate(cam);
        DestroyImmediate(renderTexture);
    }

    void LateUpdate()
    {
        if (oneFacePerFrame)
        {
            //var clock = Time.frameCount % 2;
			//var faceToRender = (clock==0)?3:4;    
            //var faceMask = 1 << (faceToRender);
            UpdateCubemap(1 << 3 );
            UpdateCubemap(1 << 4 );
        }
        else
        {
            UpdateCubemap(63); // all six faces
        }
    }

    void UpdateCubemap(int faceMask)
    {
        if (!cam)
        {
            GameObject obj = new GameObject("CubemapCamera", typeof(Camera));
            obj.hideFlags = HideFlags.HideAndDontSave;
            obj.transform.position = transform.position;
			//obj.transform.localPosition+=Vector3.up*0.1f;
            obj.transform.rotation = Quaternion.identity;
            cam = obj.GetComponent<Camera>();
            cam.farClipPlane = 100; // don't render very far into cubemap
            cam.enabled = false;
        }

        if (!renderTexture)
        {
            renderTexture = new RenderTexture(cubemapSize, cubemapSize, 32);
            renderTexture.dimension = UnityEngine.Rendering.TextureDimension.Cube;
            renderTexture.hideFlags = HideFlags.HideAndDontSave;

            GetComponent<RegularLiquidSurface>().material.SetTexture("_EnvTex", renderTexture);
			
        }

        cam.transform.position = transform.position;
        cam.RenderToCubemap(renderTexture, faceMask);
    }
}
