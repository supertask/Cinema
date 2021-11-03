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
using Cinema.PostProcessing.KMath;

namespace Cinema.PostProcessing
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Cinema/RandomInvert")]
    public sealed class RandomInvert : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        //public ClampedFloatParameter scale = new ClampedFloatParameter(0.5f, 0, 1.0f);
        //public Bool​Parameter isCircle = new Bool​Parameter(false);
        //public Vector2Parameter center = new Vector2Parameter(new Vector3(0.5f, 0.5f));
        public ClampedFloatParameter fadeTime = new ClampedFloatParameter(0.25f, 0f, 3f);
        public ClampedFloatParameter noiseScale = new ClampedFloatParameter(250f, 0f, 500f);
        public Bool​Parameter startInvert = new Bool​Parameter(false);


        private Material _material;
        private float fadeDuration = 0;
        private float threshold = 0;
        private float startTime = 0;
        public EaseType easeType = EaseType.QuintOut;
        public bool isInvert = false;
        private bool cachedStartInvert = false;

        static class ShaderIDs
        {
            internal static readonly int Threshold = Shader.PropertyToID("_Threshold");
            internal static readonly int Invert = Shader.PropertyToID("_Invert");
            internal static readonly int StartTime = Shader.PropertyToID("_StartTime");
            internal static readonly int NoiseScale = Shader.PropertyToID("_NoiseScale");
            internal static readonly int NegativeRatio = Shader.PropertyToID("_NegativeRatio");

            internal static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
        }

        public bool IsActive() => _material != null && (fadeTime.value > 0);

        public override CustomPostProcessInjectionPoint injectionPoint =>
            CustomPostProcessInjectionPoint.AfterPostProcess;

        public override void Setup()
        {
            _material = CoreUtils.CreateEngineMaterial("Hidden/Cinema/PostProcess/RandomInvert");
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle srcRT, RTHandle destRT)
        {
            if (_material == null) return;

            if (fadeDuration > 0f)
            {
                fadeDuration -= Time.deltaTime;
                float d = Mathf.Clamp01(fadeDuration / fadeTime.value);
                threshold = Easing.Ease(easeType, 1f, 0f, d);

                if(d <= 0f)
                {
                    isInvert = !isInvert;
                    threshold = 0;
                }
            }
            
            //TODO(Tasuku): 後でマウス操作にする
            if (startInvert.value != cachedStartInvert)
            {
                StartInvert();
            }

            _material.SetFloat(ShaderIDs.Threshold, threshold);
            _material.SetInt(ShaderIDs.Invert, (isInvert ? 1 : 0));
            _material.SetFloat(ShaderIDs.StartTime, startTime);
            _material.SetFloat(ShaderIDs.NoiseScale, noiseScale.value);
            _material.SetTexture(ShaderIDs.InputTexture, srcRT);

            // Shader pass number
            var pass = 0;

            // Blit
            HDUtils.DrawFullScreen(cmd, _material, destRT, null, pass);
            
            cachedStartInvert = startInvert.value;
        }

        public void StartInvert()
        {
            fadeDuration = fadeTime.value;
            startTime = Time.time;
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}
