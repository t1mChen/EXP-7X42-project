// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// the code is inspired by workshops materials and extended from it
// https://github.com/COMP30019/Workshop-9
// https://github.com/COMP30019/Workshop-8
// https://github.com/COMP30019/Workshop-8-Solution
// https://github.com/COMP30019/Workshop-9-Solution

Shader "Custom/notesShader"
{
    Properties
    {

		// passed in by materials
        _Color ("Color", Color) = (1,0.8,0,0.3)
        _Glossiness ("Smoothness", Range(0,1)) = 0.9
		_MainTex ("Texture", 2D) = "yellow" {}

		// lighting information passed in by C# script
		_PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
		_Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            Cull Off

            CGPROGRAM

            #pragma vertex vert
		    #pragma fragment frag

		    #include "UnityCG.cginc"

		    uniform sampler2D _MainTex;

			uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;
			uniform float _Ka;
			uniform float _Kd;
			uniform float _Ks;
			uniform float _fAtt;
			uniform float _specN;

            struct Input
            {
                float2 uv_MainTex;
            };

            struct vertIn
		    {
			    float4 vertex : POSITION;
			    float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
		    };

		    struct vertOut
		    {
			    float4 vertex : SV_POSITION;
			    float2 uv : TEXCOORD0;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
		    };

            half _Glossiness;
            fixed4 _Color;

			// Implementation of the vertex shader
            vertOut vert(vertIn v)
		    {
				vertOut o;
				// record the vertex information of the world
				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				// brings a wave effect to the notes
			    float4 displacement = float4(0.0f, sin(v.vertex.z+_Time.y)+sin(_Time.y), 0.0f, 0.0f); 
			    v.vertex += displacement;

				// MVP transformation
			    o.vertex = UnityObjectToClipPos(v.vertex);
			    o.uv = v.uv;
			    return o;
		    }
			
		    // Implementation of the fragment shader
		    fixed4 frag(vertOut v) : SV_Target
		    {
				
				float4 unlitColor = tex2D(_MainTex, v.uv);
				float3 interpNormal = normalize(v.worldNormal);

				// implementation of illumination using Phong shader as in workshops

				// ambient
				float Ka = _Ka;
				float3 amb = unlitColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;
				
				float fAtt = _fAtt;
				// diffuse
				float Kd = _Kd;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
				float LdotN = dot(L, interpNormal);
				float3 dif = fAtt * _PointLightColor.rgb * Kd * unlitColor.rgb * saturate(LdotN);

				// specular
				float Ks = _Ks;
				float specN = _specN; 
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);

				// using the Blinn-Phong approximation method:
				specN = _specN; 
				float3 H = normalize(V + L);
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

				// we also gives ambient a breath effect
				amb *= (1+0.5*sin(_Time.y));

				// Combine Phong illumination model components
				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = unlitColor.a;
				
				// apply glossiness info to the output colour
			    fixed4 col = returnColor;
				col.a *= _Glossiness;				
			    return col;
		    }
            ENDCG
        }
    }
}
