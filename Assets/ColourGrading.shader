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
		ZTest Always
		Cull Off
		ZWrite Off

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
				UNITY_VERTEX_OUTPUT_STEREO
			};

			UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
			sampler2D _LUT;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				fixed4 col = 
					UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);

				float2 uv = 
					float2(
						floor(col.b * 15.0) / 16.0, 
						col.g * 15.0 / 16.0 + 1.0 / 32.0) + 
							float2(col.r * 15.0 / 256.0 + 1.0 / 512.0, 0);

				col.rgb = tex2D(_LUT, uv).rgb;
				return col;
			}
			ENDCG
		}
	}
}
