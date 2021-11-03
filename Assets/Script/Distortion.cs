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
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Cinema.PostProcessing
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Cinema/Distortion")]
    public sealed class Distortion : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter scale = new ClampedFloatParameter(0.5f, 0, 1.0f);
        public Vector3Parameter position = new Vector3Parameter(new Vector3(0,0,1));
        public ClampedFloatParameter power = new ClampedFloatParameter(0, 0, 1.0f);
        public ClampedFloatParameter timeScale = new ClampedFloatParameter(5, 0, 10.0f);


        Material _material;

        static class ShaderIDs
        {
            internal static readonly int DistortionNoiseScale = Shader.PropertyToID("_DistortionNoiseScale");
            internal static readonly int DistortionNoisePosition = Shader.PropertyToID("_DistortionNoisePosition");
            internal static readonly int DistortionPower = Shader.PropertyToID("_DistortionPower");
            internal static readonly int TimeScale = Shader.PropertyToID("_TimeScale");
            internal static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
        }

        public bool IsActive() => _material != null && (power.value > 0);

        public override CustomPostProcessInjectionPoint injectionPoint =>
        //    CustomPostProcessInjectionPoint.BeforePostProcess;
            CustomPostProcessInjectionPoint.AfterPostProcess;

        public override void Setup()
        {
            _material = CoreUtils.CreateEngineMaterial("Hidden/Cinema/PostProcess/Distortion");
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle srcRT, RTHandle destRT)
        {
            if (_material == null) return;

            // Invoke the shader.
            _material.SetFloat(ShaderIDs.DistortionNoiseScale, scale.value);
            _material.SetVector(ShaderIDs.DistortionNoisePosition, position.value);
            _material.SetFloat(ShaderIDs.DistortionPower, power.value);
            _material.SetFloat(ShaderIDs.TimeScale, timeScale.value);
            _material.SetTexture(ShaderIDs.InputTexture, srcRT);

            // Shader pass number
            var pass = 0;

            // Blit
            HDUtils.DrawFullScreen(cmd, _material, destRT, null, pass);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}
