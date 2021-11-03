
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

#ifndef IMAGE_EFFECT_MOSAIC
#define IMAGE_EFFECT_MOSAIC

void MosaicUV_float(
	float2 uv,
	float mosaicScale,
	out float2 mosaicUV)
{
	float2 delta = saturate(mosaicScale / _ScreenSize.xy);
	mosaicUV = (floor(uv / delta) + 0.5) * delta;
}

void MosaicCircle_float(
	float2 uv,
	float mosaicScale,
	out float circle)
{
	float2 delta = saturate(mosaicScale / _ScreenSize.xy);
	float2 uv2 = frac(uv / delta);
	float len = 0.5 - length(uv2 - 0.5);
	circle = smoothstep(0.0, 0.05, len);
}


#endif