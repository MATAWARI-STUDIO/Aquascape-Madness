Shader "RefractLiquidSurface" {
	//show values to edit in inspector
	Properties{
		_EnvTex ("Environment", Cube) = "gray" {}
        _Refraction ("Refraction Index", float) = 0.9
        _Fresnel ("Fresnel Coefficient", float) = 5.0
        _Reflectance ("Reflectance", float) = 1.0
	}

	SubShader{
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			#pragma surface surf Standard vertex:vert
			#pragma editor_sync_compilation
			#pragma target 3.0

			//buffers
			#if (SHADER_API_METAL || SHADER_API_D3D11)
			uniform StructuredBuffer<int> Triangles;
			uniform StructuredBuffer<float3> Positions;
			uniform StructuredBuffer<float2> UVs;
			uniform StructuredBuffer<float3> Normals;
			#endif

			float gridScale;

			samplerCUBE _EnvTex;
			half _Refraction;
			half _Fresnel;
			half _Reflectance;


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				uint vid : SV_VertexID;
				uint inst : SV_InstanceID;
			};

			struct Input
			{
				float4 color : COLOR;
				float3 worldNormal;
				float3 viewDir;
			};

			float4x4 _LocalToWorld;
        	float4x4 _WorldToLocal;

			void vert(inout appdata v, out Input sin) 
			{
				#if (SHADER_API_METAL || SHADER_API_D3D11)
				
				uint vertex_id = v.vid;
				int positionIndex = Triangles[vertex_id];
				float3 position = Positions[positionIndex]*gridScale;
				v.vertex= float4(position, 1);
				v.normal=Normals[positionIndex];
				sin.worldNormal = v.normal;
				float3 cam=_WorldSpaceCameraPos;
				sin.viewDir=normalize(position-cam);

				#endif
				sin.color=float4(1,0,0,0);
			}
			
			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				
				o.Albedo = fixed4(0,0,0,1);

				half3 n = normalize (IN.worldNormal);
				half3 v = normalize (IN.viewDir);
				half fr = pow(1.0f - dot(v, n), _Fresnel) * _Reflectance;
				
				half3 reflectDir = reflect(-v, n);
				half3 refractDir = refract(-v, n, _Refraction);

				half3 reflectColor = texCUBE (_EnvTex, reflectDir).rgb;
				half3 refractColor = texCUBE (_EnvTex, refractDir).rgb;
				
				o.Emission = reflectColor * fr + refractColor;
			}

			ENDCG
	}
	Fallback "Diffuse"
}
