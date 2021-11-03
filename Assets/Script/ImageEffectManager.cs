using System.Collections;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

using Cinema.PostProcessing;
using Cinema.PostProcessing.KMath;

namespace Cinema.PostProcessing.Manager {

    public class ImageEffectManager : MonoBehaviour // : SingletonMonoBehaviour<ImageEffectManager>
    {
        #region public

        public VolumeProfile  volumeProfile;
        private Mosaic mosaic;
        private Negative negative;
        private Reflection reflection;
        //private EdgeDetection edgeDetection;
        private RadiationBlur radiationBlur;
        private RectBlockGlitch glitch;
        private Distortion distorion;
        private RGBShift rgbShift;
        private RandomInvert randomInvert;

        [SerializeField]
        KeyCode mosaicKey = KeyCode.F1;
        [SerializeField]
        KeyCode radiationBlurKey = KeyCode.F2;
        [SerializeField]
        KeyCode glitchKey = KeyCode.F3;
        [SerializeField]
        KeyCode distortionKey = KeyCode.F4;
        [SerializeField]
        KeyCode rgbShiftKey = KeyCode.F5;
        [SerializeField]
        KeyCode randomInvertKey = KeyCode.F6;
        [SerializeField]
        KeyCode negativeKey = KeyCode.F7;

        [SerializeField]
        KeyCode reflectionLRKey = KeyCode.F8;
        [SerializeField]
        KeyCode reflectionTBKey = KeyCode.F9;
        //[SerializeField]
        //KeyCode edgeDetectionKey = KeyCode.F5;

        public float effectTime = 0.25f;
        public float negativeEffectTime = 0.125f;

        public float maxMosaiceScale = 64;
        public float maxRadiationBlurPower = 64;
        public float maxGlitchIntensity = 0.95f;
        public float maxDistortionPower = 0.15f;
        public float maxRGBShiftPower = 54;
        #endregion

        bool isNegative = false;
        //bool isEdgeDetect = false;

        public void ResetEffect()
        {
            mosaic.isCircle.value = false;
            negative.negativeRatio.value = 0f;
            isNegative = false;
            reflection.isHorizontal.value = false;
            reflection.isVertical.value = false;
            //edgeDetection.enabled = true;
            //edgeDetection.blend = 1;
            //isEdgeDetect = false;
            radiationBlur.power.value = 0;
            glitch.intensity.value = 0;
            distorion.power.value = 0;
            rgbShift.power.value = 0;
            randomInvert.startInvert.value = false;
            randomInvert.isInvert = false;
        }

        IEnumerator ActionMosaic()
        {
            float duration = effectTime;
            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                mosaic.scale.value = Easing.Ease(EaseType.QuadOut, maxMosaiceScale, 1, 1f - duration / effectTime);
                yield return null;
            }
        }

        IEnumerator ActionNegative()
        {
            float duration = negativeEffectTime;
            float start = isNegative ? 1 : 0;
            float end = 1f - start;
            isNegative = !isNegative;

            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                negative.negativeRatio.value = Easing.Ease(EaseType.QuadOut, start, end, 1f - duration / negativeEffectTime);
                yield return null;
            }

        }

        void ActionReflectionLR()
        {
            reflection.isHorizontal.value = !reflection.isHorizontal.value;
        }

        void ActionReflectionTB()
        {
            reflection.isVertical.value = !reflection.isVertical.value;
        }

/*
        IEnumerator ActionEdgeDetection()
        {
            float duration = effectTime;
            float start = isEdgeDetect ? 0 : 1;
            float end = 1f - start;
            isEdgeDetect = !isEdgeDetect;
            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                edgeDetection.blend = Easing.Ease(EaseType.QuadOut, start, end, 1f - duration / effectTime);
                yield return null;
            }
        }
*/

        IEnumerator ActionRadiationBlur()
        {
            float duration = effectTime;
            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                radiationBlur.power.value = Easing.Ease(EaseType.QuadOut, maxRadiationBlurPower, 1, 1f - duration / effectTime);
                yield return null;
            }
        }
        
        IEnumerator ActionGlitch()
        {
            float duration = effectTime;
            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                glitch.intensity.value = Easing.Ease(EaseType.QuadOut, maxGlitchIntensity, 0, 1f - duration / effectTime);
                yield return null;
            }
        }

        IEnumerator ActionDistortion()
        {
            float duration = effectTime;
            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                distorion.power.value = Easing.Ease(EaseType.QuadOut, maxDistortionPower, 0, 1f - duration / effectTime);
                yield return null;
            }
        }

        IEnumerator ActionRGBShift()
        {
            float duration = effectTime;
            while (duration > 0f)
            {
                duration = Mathf.Max(duration - Time.deltaTime, 0);
                rgbShift.power.value = Easing.Ease(EaseType.QuadOut, maxRGBShiftPower, 0, 1f - duration / effectTime);
                yield return null;
            }
        }

        void KeyCheck()
        {
            if (Input.GetKeyDown(mosaicKey))
            {
                StartCoroutine(ActionMosaic());
            }
            if (Input.GetKeyDown(negativeKey))
            {
                StartCoroutine(ActionNegative());
            }

            if (Input.GetKeyDown(reflectionLRKey))
            {
                ActionReflectionLR();
            }
            if (Input.GetKeyDown(reflectionTBKey))
            {
                ActionReflectionTB();
            }

/*
            if (Input.GetKeyDown(edgeDetectionKey))
            {
                StartCoroutine(ActionEdgeDetection());
            }
*/
            if (Input.GetKeyDown(radiationBlurKey))
            {
                StartCoroutine(ActionRadiationBlur());
            }
            if (Input.GetKeyDown(glitchKey))
            {
                StartCoroutine(ActionGlitch());
            }
            if (Input.GetKeyDown(distortionKey))
            {
                StartCoroutine(ActionDistortion());
            }
            if (Input.GetKeyDown(rgbShiftKey))
            {
                StartCoroutine(ActionRGBShift());
            }
            if (Input.GetKeyDown(randomInvertKey))
            {
                randomInvert.startInvert.value = !randomInvert.startInvert.value;
            }
            
        }

        private void SetImageEffects()
        {
            if (volumeProfile == null)
            {
                Debug.LogError("Please set a volume profile on ImageEffectManager.");
                return;
            }
            if (mosaic == null) volumeProfile.TryGet<Mosaic>(out mosaic);
            if (negative == null) volumeProfile.TryGet<Negative>(out negative);
            if (reflection == null) volumeProfile.TryGet<Reflection>(out reflection);
            if (radiationBlur == null) volumeProfile.TryGet<RadiationBlur>(out radiationBlur);
            if (glitch == null) volumeProfile.TryGet<RectBlockGlitch>(out glitch);
            if (distorion == null) volumeProfile.TryGet<Distortion>(out distorion);
            if (rgbShift == null) volumeProfile.TryGet<RGBShift>(out rgbShift);
            if (randomInvert == null) volumeProfile.TryGet<RandomInvert>(out randomInvert);
        }

        private void Start()
        {
            this.SetImageEffects();
            this.ResetEffect();
        }

        private void Update()
        {
            this.KeyCheck();
        }
        
        private void OnDestroy()
        {
            this.ResetEffect();
        }
    }
}