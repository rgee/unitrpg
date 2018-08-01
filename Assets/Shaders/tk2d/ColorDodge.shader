// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "tk2d/ColorDodge" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 110
		
		GrabPass { }

		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_vct
			#pragma fragment frag_mult 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _GrabTexture;
			float4 _MainTex_ST;

			struct vin_vct 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};

			fixed4 Multiply(fixed4 a, fixed4 b)
			{
				fixed4 r = a * b;
				r.a = b.a;
				return r;
			}

			fixed4 ColorDodge(fixed4 a, fixed4 b)
			{
				fixed4 r = a / (1.0 - b);
				r.a = b.a;
				return r;
			}

			v2f_vct vert_vct(vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.screenPos = o.vertex;
				return o;
			}
			fixed4 ColorBurn(fixed4 a, fixed4 b)
			{
				fixed4 r = 1.0 - (1.0 - a) / b;
				r.a = b.a;
				return r;
			}

			fixed4 frag_mult(v2f_vct i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				float2 grabTexCoord = i.screenPos.xy / i.screenPos.w;
				grabTexCoord.x = (grabTexCoord.x + 1.0) * .5;	
				grabTexCoord.y = (grabTexCoord.y + 1.0) * .5;

#ifdef UNITY_UV_STARTS_AT_TOP
				grabTexCoord.y = 1.0 - grabTexCoord.y;
#endif

				fixed4 grabColor = tex2D(_GrabTexture, grabTexCoord);
				return ColorDodge(grabColor, col);
			}
			
			ENDCG
		} 
	}
}
