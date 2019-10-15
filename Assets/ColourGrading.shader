Shader "Unlit/ColourGrading"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LUT ("Colour LUT", 2D) = "white" {}
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _LUT;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 uv = 
					float2(
						floor(col.b * 15.0) / 16.0, 
						col.g * 15.0 / 16.0 + 1.0 / 32.0) + 
					float2(col.r * 15.0 / 256.0 + 1.0 / 512.0, 0);
				return tex2D(_LUT, uv);
			}
			ENDCG
		}
	}
}
