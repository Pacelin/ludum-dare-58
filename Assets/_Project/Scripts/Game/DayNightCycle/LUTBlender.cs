using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Game.Core.Scripts
{
    [ExecuteAlways]
    public class LUTBlender : MonoBehaviour
    {
        // Config parameters
        [Header("Settings")]
        [SerializeField] Volume volume = default;
        [SerializeField] Material lutBlendMaterial = default;
        [Space]
        [SerializeField] bool setStartingLutFromVolume = true;
        [SerializeField] Texture startingLut = default;
        [SerializeField] Texture noEffectLut = default;

        // Cached references
        ColorLookup effect;
        Material intensityLutBlendMaterial;

        // State variables
        RenderTexture mainRenderTexture;
        RenderTexture intensifiedLut1;
        RenderTexture intensifiedLut2;
        
        private static readonly int _MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int _LUT = Shader.PropertyToID("_LUT");
        private static readonly int _LUT2 = Shader.PropertyToID("_LUT2");
        private static readonly int _Contribution = Shader.PropertyToID("_Contribution");

        private void OnEnable()
        {
            // Copy LUT blend material and create render textures for creating intensified pre-blends
            intensityLutBlendMaterial = new Material(lutBlendMaterial);
            intensifiedLut1 = new RenderTexture(1024, 32, 0)
            {
                stencilFormat = GraphicsFormat.None,
                depthStencilFormat = GraphicsFormat.None,
                graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm,
                format = RenderTextureFormat.ARGB32,
            };
            intensifiedLut2 = new RenderTexture(1024, 32, 0)
            {
                stencilFormat = GraphicsFormat.None,
                depthStencilFormat = GraphicsFormat.None,
                graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm,
                format = RenderTextureFormat.ARGB32,
            };

            // Get color lookup effect from volume
            ColorLookup tmp;
            if (volume.profile.TryGet(out tmp))
                effect = tmp;
            else
                Debug.LogWarning("There is no Color Lookup effect in Volume. Please add one.");

            // Warn if no effect LUT is not assigned
            if (noEffectLut == null) Debug.LogWarning("No effect LUT is not assigned in LUT Blender. Please assign one.");
            
            // Set render texture and starting LUT
            mainRenderTexture = new RenderTexture(1024, 32, 0)
            {
                stencilFormat = GraphicsFormat.None,
                depthStencilFormat = GraphicsFormat.None,
                graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm,
                format = RenderTextureFormat.ARGB32,
            };
            if (setStartingLutFromVolume) startingLut = effect.texture.value;

            // Configure LUT blend material
            SetDefaultConfiguration();
        }

        public void CreateBlend(Texture lut1, float lut1Intensity, Texture lut2, float lut2Intensity, float startingBlendRatio)
        {
            lutBlendMaterial.SetTexture(_MainTex, startingLut);
            lutBlendMaterial.SetTexture(_LUT, GetTextureWithSetIntensity(lut1, lut1Intensity));
            lutBlendMaterial.SetTexture(_LUT2, GetTextureWithSetIntensity2(lut2, lut2Intensity));
            lutBlendMaterial.SetFloat(_Contribution, startingBlendRatio);
        }

        public void SetPostProcessing(float blendRatio)
        {
            lutBlendMaterial.SetFloat(_Contribution, blendRatio);
            Graphics.Blit(null, mainRenderTexture, lutBlendMaterial);
            effect.active = true;
            effect.texture.value = mainRenderTexture;
        }

        #region Utilities
        void SetDefaultConfiguration()
        {
            lutBlendMaterial.SetTexture(_MainTex, startingLut);
            lutBlendMaterial.SetTexture(_LUT, noEffectLut);
            lutBlendMaterial.SetTexture(_LUT2, noEffectLut);
        }

        // TODO: Resolve the garbage collection issue due to creating new render textures and combine these methods
        Texture GetTextureWithSetIntensity(Texture lut, float intensity)
        {
            intensityLutBlendMaterial.SetTexture(_MainTex, noEffectLut);
            intensityLutBlendMaterial.SetTexture(_LUT, noEffectLut);
            intensityLutBlendMaterial.SetTexture(_LUT2, lut);
            intensityLutBlendMaterial.SetFloat(_Contribution, intensity);

            Graphics.Blit(null, intensifiedLut1, intensityLutBlendMaterial);

            return intensifiedLut1;
        }
        
        Texture GetTextureWithSetIntensity2(Texture lut, float intensity)
        {
            intensityLutBlendMaterial.SetTexture(_MainTex, noEffectLut);
            intensityLutBlendMaterial.SetTexture(_LUT, noEffectLut);
            intensityLutBlendMaterial.SetTexture(_LUT2, lut);
            intensityLutBlendMaterial.SetFloat(_Contribution, intensity);

            Graphics.Blit(null, intensifiedLut2, intensityLutBlendMaterial);

            return intensifiedLut2;
        }
        #endregion
    }
}