Shader "_Custom_Shaders/Weathering_Shader"
{
	Properties
	{
		_Base ("Base", 2D) = "white" {}
		_WeatherTex("Wrecked", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "white" {}
		_Damage("Damage Map", 2D) = "white" {}
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent" }
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _Base;
			sampler2D _WeatherTex;
			sampler2D _Alpha;
			sampler2D _Damage;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 base = tex2D(_Base, i.uv);
				fixed4 weight = tex2D(_Damage, i.uv);
				fixed4 wreck = tex2D(_WeatherTex, i.uv);

				fixed4 col = base * (1 - weight) + (wreck * weight);

				return col;
			}
			ENDCG
		}
	}
}
