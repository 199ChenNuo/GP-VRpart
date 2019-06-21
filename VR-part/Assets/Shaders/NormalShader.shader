Shader "Unlit/NormalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

			struct VertexData {
				float4 position : POSITION;
				float3 normal : NORMAL;
			};

			struct FragmentData {
				float4 position : SV_POSITION;
				float3 normal : TEXCOORD0;
			};

			FragmentData vert(VertexData v) {
				FragmentData i;
				i.position = UnityObjectToClipPos(v.position);
				i.normal = UnityObjectToWorldNormal(v.normal);
				return i;
			}

			float4 frag(FragmentData i) : SV_TARGET{
				return float4(i.normal, 1);
			}
            ENDCG
        }
    }
}
