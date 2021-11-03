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
    [System.Serializable, VolumeComponentMenu("Post-processing/Cinema/RectBlockGlitch")]
    public sealed class RectBlockGlitch : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter noiseSpeed = new ClampedFloatParameter(0.9f, 0f, 1.0f); //ノイズテクスチャの隣の色になる確率
        public ClampedFloatParameter noiseColorChange = new ClampedFloatParameter(0.85f, 0f, 1.0f); //ノイズの更新頻度
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1.0f); //ずらす強さ
        public ClampedIntParameter glitchScale = new ClampedIntParameter(55, 0, 150); //ずらす高さ

        private Material _material;
        private Texture2D noiseTexture;
        private float cachedGlitchScale;

        static class ShaderIDs
        {
            internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
            internal static readonly int NoiseTextureSize = Shader.PropertyToID("_NoiseTextureSize");
            internal static readonly int NoiseTexture = Shader.PropertyToID("_NoiseTexture");
            internal static readonly int InputTexture = Shader.PropertyToID("_InputTexture");
        }

        public bool IsActive() => _material != null && (intensity.value > 0);

        public override CustomPostProcessInjectionPoint injectionPoint =>
            CustomPostProcessInjectionPoint.AfterPostProcess;

        public override void Setup()
        {
            _material = CoreUtils.CreateEngineMaterial("Hidden/Cinema/PostProcess/RectBlockGlitch");
            this.CreateTexture();
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle srcRT, RTHandle destRT)
        {
            if (_material == null) return;

            if (Random.value > noiseSpeed.value)
            {
                UpdateNoiseTexture();
            }

            _material.SetFloat(ShaderIDs.Intensity, intensity.value);
            _material.SetVector(ShaderIDs.NoiseTextureSize, new Vector2(noiseTexture.width, noiseTexture.height));
            _material.SetTexture(ShaderIDs.NoiseTexture, noiseTexture);
            _material.SetTexture(ShaderIDs.InputTexture, srcRT);

            // Shader pass number
            var pass = 0;

            // Blit
            HDUtils.DrawFullScreen(cmd, _material, destRT, null, pass);
        }
        
        private void CreateTexture()
        {
            noiseTexture = new Texture2D(
                Screen.width / glitchScale.value,
                Screen.height / glitchScale.value,
                TextureFormat.RGBA32, false);
            //noiseTexture = new Texture2D(64, 32, TextureFormat.RGBA32, false);

            noiseTexture.hideFlags = HideFlags.DontSave;
            noiseTexture.wrapMode = TextureWrapMode.Clamp;
            noiseTexture.filterMode = FilterMode.Point;
            this.UpdateNoiseTexture();
        }

        private void UpdateNoiseTexture()
        {
            if (cachedGlitchScale != glitchScale.value)
            {
                this.noiseTexture = ResizeTexture(this.noiseTexture,
                    Screen.width / glitchScale.value, Screen.height / glitchScale.value);
                cachedGlitchScale = glitchScale.value;
            }
            
            Color color = RandomColor();

            for (int y = 0; y < noiseTexture.height; y++)
            {
                for (int x = 0; x < noiseTexture.width; x++)
                {
                    // 確率で隣と同じ色になる
                    if (Random.value > noiseColorChange.value) color = RandomColor();
                    noiseTexture.SetPixel(x, y, color);
                }
            }

            noiseTexture.Apply();
            
        }

        public Texture2D ResizeTexture(Texture2D srcTexture, int newWidth, int newHeight) {
            var resizedTexture = new Texture2D(newWidth, newHeight);
            Graphics.ConvertTexture(srcTexture, resizedTexture);
            return resizedTexture;
        }
    
        private Color RandomColor()
        {
            return new Color(Random.value, Random.value, Random.value, Random.value);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}
