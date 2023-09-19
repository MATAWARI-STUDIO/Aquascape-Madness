Shader "RegularLiquidSurface" {
	//show values to edit in inspector
	Properties{
		[HDR] _Color ("Tint", Color) = (0, 0, 0, 1)
		//_MainTex ("Texture", 2D) = "white" {}
		_MainTex("Albedo & Alpha", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader{
			//Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"
			//#include "UnityLightingCommon.cginc"

			#pragma surface surf Standard vertex:vert addshadow fullforwardshadows
			// alpha  alpha alpha:fade
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
			float globalOpacity;
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			half _Glossiness;
        	half _Metallic;
			fixed4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				//float4 color : COLOR;
				//float4 texcoord : TEXCOORD1;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				uint vid : SV_VertexID;
				uint inst : SV_InstanceID;
			};

			struct Input
			{
				//float vface : VFACE;
				float4 color : COLOR;
				float2 abc;
			};

			float4x4 _LocalToWorld;
        	float4x4 _WorldToLocal;


			void vert(inout appdata v, out Input sin) //, in uint instance_id: SV_InstanceID
			{
				#if (SHADER_API_METAL || SHADER_API_D3D11)
				
				uint vertex_id = v.vid;
				int positionIndex = Triangles[vertex_id];
				float3 position = Positions[positionIndex]*gridScale;
				//v.vertex= mul(UNITY_MATRIX_VP, float4(position, 1));
				v.vertex= float4(position, 1);
				//v.texcoord1=float4(UVs[positionIndex]*_MainTex_ST.xy,0,0);
				sin.abc=UVs[positionIndex]*_MainTex_ST.xy;
				//sin.abc=float2(0.0001*positionIndex,0);
				v.normal=Normals[positionIndex];
				#endif
				sin.color=float4(1,0,0,0);
				//v.normal=float3(0,1,0);

			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				//o.Albedo = fixed4(1,0,0,0.5);
				fixed4 c=tex2D (_MainTex, IN.abc)* _Color;
				o.Albedo = c.rgb*globalOpacity;
				//o.Albedo = float4(IN.abc.x,IN.abc.y,0,0);
				//o.Metallic = 0.0;
				//o.Smoothness = 0;
				o.Metallic = _Metallic;
            	o.Smoothness = _Glossiness;
				//o.Normal = float3(0, 0, IN.vface < 0 ? -1 : 1); // back face support
				//o.Emission = fixed4(255,255,0,0); //_Emission * IN.color.rgb;
				o.Normal = UnpackNormal (tex2D (_BumpMap, IN.abc));
				//o.Occlusion=1;
				//o.Alpha=0.33;
				//o.Alpha = c.a;
			}
			ENDCG
	}
	Fallback "Diffuse"
}
