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
    [System.Serializable, VolumeComponentMenu("Post-processing/Cinema/GodRay")]
    public sealed class GodRay : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        //public ClampedFloatParameter scale = new ClampedFloatParameter(0.5f, 0, 1.0f);
        //public Bool​Parameter isCircle = new Bool​Parameter(false);

        public ClampedFloatParameter power = new ClampedFloatParameter(0, 0, 1);
        public ClampedIntParameter iteration = new ClampedIntParameter(50, 0, 150);
        public ClampedFloatParameter rayDistance = new ClampedFloatParameter(0.4f, 0, 1);


        Material _material;

        static class ShaderIDs
        {
            internal static readonly int Power = Shader.PropertyToID("_Power");
            internal static readonly int NumOfIterations = Shader.PropertyToID("_NumOfIterations");
            internal static readonly int GodRayMicroDistance = Shader.PropertyToID("_GodRayMicroDistance");

            internal static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
        }

        public bool IsActive() => _material != null && (power.value > 0);

        public override CustomPostProcessInjectionPoint injectionPoint =>
            CustomPostProcessInjectionPoint.AfterPostProcess;

        public override void Setup()
        {
            _material = CoreUtils.CreateEngineMaterial("Hidden/Cinema/PostProcess/GodRay");
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle srcRT, RTHandle destRT)
        {
            if (_material == null) return;

            // Invoke the shader.
            _material.SetFloat(ShaderIDs.Power, power.value);
            _material.SetInt(ShaderIDs.NumOfIterations, iteration.value);
            _material.SetFloat(ShaderIDs.GodRayMicroDistance, rayDistance.value);

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
