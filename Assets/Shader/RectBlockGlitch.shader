//
// Cinema
//
// MIT License
// Copyright (c) 2021 Tasuku TAKAHASHI
// Copyright (c) 2018 kaiware007
//     UnityVJShaderSlide20181108, https://github.com/kaiware007/UnityVJShaderSlide20181108
// Copyright (C) 2011 by Ashima Arts (Simplex noise)
// Copyright (C) 2011-2016 by Stefan Gustavson (Classic noise and others)
//
Shader "Hidden/Cinema/PostProcess/RectBlockGlitch"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
                
                return output;
            }

            TEXTURE2D_X(_InputTexture);
            //SAMPLER(sampler_InputTexture);
            TEXTURE2D(_NoiseTexture);
			float _Intensity;
            float2 _NoiseTextureSize;

            float4 Fragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;

				float4 noise = LOAD_TEXTURE2D(_NoiseTexture, uv * _NoiseTextureSize);
				
				float thresh = 1.001 - _Intensity * 1.001;
				
				float slide = step(thresh, pow(noise.b, 2.5));
				float ref_a = step(thresh, pow(noise.a, 2.5));
				float ref_g = step(thresh, pow(noise.r, 2.5));
				float ref_b = step(thresh, pow(noise.g, 2.5));
				float d = step(thresh, pow(noise.b, 3.5));

				float2 uv_slide = (uv + noise.xy * slide) % 1;
				float2 uv_ref_r = (uv + noise.xy * ref_a) % 1;
				float2 uv_ref_g = (uv + noise.xy * ref_g) % 1;
				float2 uv_ref_b = (uv + noise.xy * ref_b) % 1;
                
				float4 col1 = LOAD_TEXTURE2D_X(_InputTexture, uv * _ScreenSize.xy);
				float3 col2 = float3(
                    LOAD_TEXTURE2D_X(_InputTexture, uv_ref_r * _ScreenSize.xy).r,
                    LOAD_TEXTURE2D_X(_InputTexture, uv_ref_g * _ScreenSize.xy).g,
                    LOAD_TEXTURE2D_X(_InputTexture, uv_ref_b * _ScreenSize.xy).b
                );

				return float4(lerp(col1, col2, d), 1);
            }


            ENDHLSL
        }
    }
    Fallback Off
}
