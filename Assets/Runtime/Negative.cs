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
    [System.Serializable, VolumeComponentMenu("Post-processing/Cinema/Negative")]
    public sealed class Negative : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        //public ClampedFloatParameter scale = new ClampedFloatParameter(0.5f, 0, 1.0f);
        //public Bool​Parameter isCircle = new Bool​Parameter(false);
        //public Vector2Parameter center = new Vector2Parameter(new Vector3(0.5f, 0.5f));
        public ClampedFloatParameter negativeRatio = new ClampedFloatParameter(0, 0f, 1f);

        private Material _material;

        static class ShaderIDs
        {
            internal static readonly int NegativeRatio = Shader.PropertyToID("_NegativeRatio");
            internal static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
        }

        public bool IsActive() => _material != null && (negativeRatio.value > 0);

        public override CustomPostProcessInjectionPoint injectionPoint =>
            CustomPostProcessInjectionPoint.AfterPostProcess;

        public override void Setup()
        {
            _material = CoreUtils.CreateEngineMaterial("Hidden/Cinema/PostProcess/Negative");
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle srcRT, RTHandle destRT)
        {
            if (_material == null) return;

            _material.SetFloat(ShaderIDs.NegativeRatio, negativeRatio.value);
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
