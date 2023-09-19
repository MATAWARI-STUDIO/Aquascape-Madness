Shader "Custom/WaterRefractionShader" {
    Properties{
        _RefractionIndex("Refraction Index", Range(1, 2)) = 1.33
        _CubeMap("Cubemap Environment", Cube) = "" {}
    }

        SubShader{
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float3 worldNormal : TEXCOORD0;
                    float3 viewDir : TEXCOORD1;
                };

                float _RefractionIndex;
                samplerCUBE _CubeMap;

                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float3 refractedDir = refract(i.viewDir, i.worldNormal, _RefractionIndex);
                    fixed4 refractedColor = texCUBE(_CubeMap, refractedDir);
                    return refractedColor;
                }
                ENDCG
            }
    }
}
