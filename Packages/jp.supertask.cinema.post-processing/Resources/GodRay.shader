//
// Cinema
//
// MIT License
// Copyright (c) 2021 Tasuku TAKAHASHI
// Copyright (c) 2015-2017  hiroakioishi
// 
// [Shadertoy] Evil Britney	
// https://www.shadertoy.com/view/llS3WG
//
Shader "Hidden/Cinema/PostProcess/GodRay"
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
			float4  _InputTexture_TexelSize;

            float _Power;
            int _NumOfIterations;
            float _GodRayMicroDistance; //GodRayの微小距離


            // Triangle wave for ping pong uv
            // Usecase: PingPong(mirror) texture
            float fukuokaTriangleWave(float x) {
                //return abs(fmod(x, 2.0) - 1) * 0.995 + 0.003; //original version
                return abs(fmod(x + 1 + 100, 2.0) - 1);
            }

            float2 fukuokaTriangleWave2D(float2 uv) {
                return float2(fukuokaTriangleWave(uv.x), fukuokaTriangleWave(uv.y));
            }


            float4 Fragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;
                
                float3 p = float3(uv - 0.5, 0.0);
                //p.xy *= 0.98;
                float3 o = LOAD_TEXTURE2D_X(_InputTexture,
                    (float2(0.5, 0.5) + p.xy) * _ScreenSize.xy).rgb;
                
                // default:
                //     radiationDownScale = 0.98 (it means _GodRayMicroDistance = 0.4),
                //     _NumOfIterations = 50
                float radiationDownScale = (0.95 + 0.05 * (1 - _GodRayMicroDistance));
                for (int index = 0; index < (int) clamp(_NumOfIterations, 1, 250); index++) {
                    p.xy = p.xy * radiationDownScale;

                    float3 col = LOAD_TEXTURE2D_X(_InputTexture,
                        (float2(0.5, 0.5) + p.xy) * _ScreenSize.xy);
                    //return length(col.rgb) > 1.7 ? 1 : 0;
                    
                    // Brightness
                    // length(col.rgb): if color is bright, the god ray color will be brighter
                    // pow & exp: if color is bright, the god ray color will be exponentially bright
                    // https://www.shadertoy.com/view/flcGzS
                    p.z += length(col.rgb) * 
                        pow(max(0.0, 0.5 - length(1.0 - col.rgb)), 2.0) *
                        exp(-index * 0.1);
                }
                return lerp(LOAD_TEXTURE2D_X(_InputTexture, uv * _ScreenSize.xy),
                    float4(o * o + p.z, 1.0), _Power);
            }

            //#include "Distortion.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}
