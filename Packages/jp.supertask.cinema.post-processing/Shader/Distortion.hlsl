
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

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
#include "./DistortionUvSG.hlsl"

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

float _NoiseDistortionScale;
float3 _NoiseDistortionPosition;
float _NoiseDistortionPower;
float _NoiseDistortionTimeScale;
float2 _BarrelDistortionPower;

TEXTURE2D_X(_InputTexture);
//SAMPLER(sampler_InputTexture);


float4 Fragment(Varyings input) : SV_Target
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

	float2 uv = input.texcoord;
#ifdef DISTORTION_NOISE_UV
	NoiseDistortionUV_float(uv, _NoiseDistortionScale, _NoiseDistortionPosition, _NoiseDistortionPower,
		_NoiseDistortionTimeScale, uv);
	//return float4(uv, 0, 1);
#endif
	//return float4(noiseDistortedUV, 0, 1);
	
#ifdef DISTORTION_BARREL_UV
	BarrelDistortionUV_float(uv, _BarrelDistortionPower, uv);
#endif
	
	float4 color = LOAD_TEXTURE2D_X(_InputTexture, uv * _ScreenSize.xy);

	return color;
}
