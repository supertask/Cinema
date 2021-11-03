
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

#ifndef IMAGE_EFFECT_DISTORTION
#define IMAGE_EFFECT_DISTORTION

#include "./Constant.hlsl"
#include "./Noise.hlsl"

float2 getRotationUV(float2 uv, float angle, float power) {
	float2 v = (float2)0;
	float rad = angle * PI;
	
	v.x = uv.x + cos(rad) * power;
	v.y = uv.y + sin(rad) * power;

	return v;
}

// Triangle wave for ping pong uv
// Usecase: PingPong texture
float fukuokaTriangleWave(float x) {
    //return abs(fmod(x, 2.0) - 1) * 0.995 + 0.003; //original version
    return abs(fmod(x + 1 + 100, 2.0) - 1);
}

float2 fukuokaTriangleWave2D(float2 uv) {
	return float2(fukuokaTriangleWave(uv.x), fukuokaTriangleWave(uv.y));
}

void DistortionUV_float(
	float2 uv,
	float distortionNoiseScale,
	float3 distortionNoisePosition,
	float distortionPower,
	float timeScale,
	out float2 distortedUV)
{
	float3 uv1 = float3(uv * distortionNoiseScale, _Time.x * 5);
	float3 noise = snoise_grad(uv1 + distortionNoisePosition);

	distortedUV = getRotationUV(uv, noise.x, noise.y * distortionPower);
	distortedUV = fukuokaTriangleWave2D(distortedUV);
}


#endif