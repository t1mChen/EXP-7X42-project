// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// the code is inspired by workshops materials and extended from it
// https://github.com/COMP30019/Workshop-9
// https://github.com/COMP30019/Workshop-8
// https://github.com/COMP30019/Workshop-8-Solution
// https://github.com/COMP30019/Workshop-9-Solution

Shader "Custom/bossJump"
{
    Properties
    {
        // texture from material
		_MainTex ("Texture", 2D) = "white" {}
        // time passed in by C# script
        _instantiateTime ("_instantiateTime", Range(0, 4)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Pass
        {
            Cull Off

            CGPROGRAM

            #pragma vertex vert
		    #pragma fragment frag

		    #include "UnityCG.cginc"

		    uniform sampler2D _MainTex;
            uniform float _instantiateTime;

            struct Input
            {
                float2 uv_MainTex;
            };

            struct vertIn
		    {
			    float4 vertex : POSITION;
			    float2 uv : TEXCOORD0;
		    };

		    struct vertOut
		    {
			    float4 vertex : SV_POSITION;
			    float2 uv : TEXCOORD0;
		    };



            vertOut vert(vertIn v)
		    {
                float scale = _instantiateTime*_instantiateTime;
                float validScale = min(max(scale,0.1),1.5);
                // scale up the texture (magnify) based on time
                v.vertex.xyz *= validScale;
			    vertOut o;

                // create a irregular wave affected by both itself and camera angle
                // MVP transformation
                // update in view space
			    v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				v.vertex += float4(0.0f, cos(v.vertex.x+
scale)+
sin(v.vertex.z+scale), 0.0f, 0.0f);
				o.vertex = mul(UNITY_MATRIX_P, v.vertex);
			    o.uv = v.uv;
			    return o;
		    }
			
		    // Implementation of the fragment shader
		    fixed4 frag(vertOut v) : SV_Target
		    {

			    float spin = 10 * _instantiateTime;
                float2 center = float2(0.5, 0.5);
                // rotate the texture constantly
                float sinAngle = sin(spin * 3.14 / 180);
                float cosineAngle = cos(spin * 3.14 / 180);
                // centre the texture and update change in rotation
                float2 spinning = (v.uv - center) * float2(sinAngle, cosineAngle) + center;
                fixed4 col = tex2D(_MainTex, spinning);
                return col;
		    }
            ENDCG
        }
    }
}
