// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentGradation"
{
	Properties
	{
		_Color("StartColor", Color) = (1,1,1,1)
		_Color2("EndColor", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{
			"Queue" = "TransParent"
			"IgnoreProjector" = "True"
			"RenderType" = "TransParent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex	: POSITION;
				float4 color	: COLOR;
				float2 texcoord	: TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex	: SV_POSITION;
				fixed4 color	: COLOR;
				float2 texcoord	: TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed4 _Color2;
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * lerp(_Color, _Color2, IN.texcoord.x);
				return OUT;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = IN.color;
				return c;
			}
			ENDCG
		}
	}
}