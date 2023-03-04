// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShaderLearning"
{
	SubShader
	{
		Tags
		{
			"PreviewType" = "Plane"
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f oa;
	            oa.vertex = UnityObjectToClipPos(v.vertex);
                oa.uv = v.uv;
	            return oa;
			}

			float4 frag(v2f i) : SV_Target
            {
	            //float4 color = float4(i.uv.r,i.uv.g,  0, 1); 
                float4 color = float4(
                    i.uv.r, 
                    i.uv.g, 
                    1, 
                    1);
	            return color;
            }
			ENDCG
		}
	}
}