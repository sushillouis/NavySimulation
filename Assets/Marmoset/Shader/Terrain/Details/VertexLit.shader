// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/TerrainEngine/Details/Vertexlit" {
	Properties {
		_Color   ("Diffuse Color", Color) = (1,1,1,1)
		_MainTex ("Diffuse(RGB) Alpha(A)", 2D) = "white" {  }
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
		_DiffCubeIBL ("Custom Diffuse Cube", Cube) = "black" {}		
	}
	SubShader {
		Tags {
			"Queue" = "AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="Opaque"
		}
		LOD 200
		Pass {
			Tags {
				"LightMode" = "Vertex" 
			}
			ZWrite On ZTest LEqual Cull Off
			AlphaTest Greater [_Cutoff]
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			//gamma-correct sampling permutations
			#pragma multi_compile MARMO_LINEAR MARMO_GAMMA
					
			#define MARMO_HQ
			#define MARMO_SKY_ROTATION
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "../../MarmosetCore.cginc"
			
		
			float4 		_MainTex_ST;
			sampler2D	_MainTex;			
			float4		_Color;
			float		_Cutoff;
			
			float4 		ExposureIBL;
			float4x4	SkyMatrix;
			samplerCUBE _DiffCubeIBL;
						
			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 texcoord : TEXCOORD0;
			};
		
			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0; 
				float3 skyNormal : TEXCOORD1;
				float3 vlight : TEXCOORD2;
			};
			
			inline float3 toSkySpace(float3 vec) {
				#ifdef MARMO_SKY_ROTATION
					return mulVec3(SkyMatrix, vec);
				#else
					return vec;
				#endif
			}
			
			inline float3 lambert(float3 worldP, float3 worldN) {
				float3 worldL = _WorldSpaceLightPos0.xyz - worldP.xyz*_WorldSpaceLightPos0.w;
				float lengthSq = dot(worldL, worldL);
				float atten = 1.0 / (1.0 + lengthSq * unity_LightAtten[0].z);
				worldL = normalize(worldL);
				float diff = dot(worldN, worldL)*0.5 + 0.5;
				diff *= diff;
				return unity_LightColor[0].rgb * (diff * atten);
			}
			
			v2f vert(appdata_t v) {
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				
				float3 worldP = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 worldN = normalize( mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL) );
				float3 worldL = normalize(_WorldSpaceLightPos0.xyz - worldP.xyz*_WorldSpaceLightPos0.w);
				o.skyNormal = toSkySpace(worldN);
				
				o.vlight = lambert(worldP, worldN);
				//o.vlight.rgb += ShadeSH9 (float4(worldN,1.0));
				return o;
			}
		
			half4 frag(v2f IN) : COLOR {
				half4 albedo = tex2D(_MainTex, IN.texcoord);
				#if MARMO_LINEAR
					//HACK: prior to Unity 4.3, detail meshes are not sRGB sampled. Uncomment if stuff's too bright.
					//albedo.rgb = toLinearFast3(albedo.rgb);
				#endif
				albedo *= _Color;
				clip(albedo.a - _Cutoff);
				
				half3 ibl = diffCubeLookup(_DiffCubeIBL, IN.skyNormal);
				ibl *= ExposureIBL.x;
				
				half3 diff = IN.vlight.rgb;
				
				half4 col;
				col.rgb = (diff + ibl) * albedo.rgb * ExposureIBL.w;
				col.a = albedo.a;
				return col;
			}
			ENDCG 
		}
	}
	
	Fallback "Transparent/Cutout/VertexLit"
}
