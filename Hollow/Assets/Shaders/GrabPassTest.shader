// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GrabPassTest" 
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Colour("Colour", Color) = (1,1,1,1)

		_Magnitude("Magnitude", Range(0,1)) = 0.05

		_DisplTex("Displacement Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
		ZWrite On Lighting Off Cull Off Fog{ Mode Off } Blend One Zero

		GrabPass{ "_GrabTexture" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;

			sampler2D _MainTex;
			fixed4 _Colour;

			float  _Magnitude;

			sampler2D _DisplTex;
			float _DisplAmount;

			struct vin_vct
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;

				float4 uvgrab : TEXCOORD1;
			};

			// Vertex function 
			v2f_vct vert(vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;

				o.texcoord = v.texcoord;

				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			// Fragment function
			half4 frag(v2f_vct i) : COLOR
			{
				half4 mainColour = tex2D(_MainTex, i.texcoord);

				float2 changingUV = i.uvgrab + _Time.x * 2;
				float2 displ = tex2D(_DisplTex, changingUV).xy;
				displ = (((displ * 2) - 1) * _Magnitude) * -1;

				fixed4 col = tex2D(_GrabTexture, i.uvgrab + displ);
				return col * mainColour * _Colour;
			}
			ENDCG
		}
	}
}