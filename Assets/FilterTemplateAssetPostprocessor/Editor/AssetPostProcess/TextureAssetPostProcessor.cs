using UnityEditor;
using UnityEngine;

namespace FilterTemplateAssetPostprocessor.Editor.AssetPostProcess
{
    public class TextureAssetPostProcessor : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            AssetPostprocessSettingSelector.ClearInternalData();
        }
        
        private void OnPreprocessTexture()
        {
            var importer = this.assetImporter as TextureImporter;
            if(importer == null) return;
            
            AssetPostprocessSettingSelector.Initialize();
            var setting = AssetPostprocessSettingSelector.SelectSetting(this.assetPath, AssetPostprocessSettingSelector.ImporterType.Texture);
            if(setting == null || !setting.textureImportSettingEnabled) return;
            
            importer.textureType = setting.TextureType;
            importer.textureShape = !CheckTextureShapeDisabled(setting) ? setting.TextureShape : TextureImporterShape.Texture2D;

            SpriteSetting(setting, importer);
            CubeMapSetting(setting, importer);

            if (setting.TextureType == TextureImporterType.NormalMap)
            {
                importer.convertToNormalmap = setting.CreateFromGrayscale;
            }

            AlphaSetting(setting, importer);

            importer.isReadable = setting.ReadWriteEnabled2;

            MipMapSetting(setting, importer);
            
            TextureSetting(setting, importer);

            var settings = new TextureImporterPlatformSettings
            {
                maxTextureSize = setting.MaxSize,
                resizeAlgorithm = setting.ResizeAlgorithm,
                format = setting.Format,
                textureCompression = setting.Compression,
                crunchedCompression = setting.UseCrunchCompression,
                // name = "Standalone"
            };

            if (setting.UseCrunchCompression)
            {
                setting.CompressorQuality = setting.CompressorQuality;
            }
            
            importer.SetPlatformTextureSettings(settings);
            
            
        }

        private static void TextureSetting(AssetPostprocessSetting setting, TextureImporter importer)
        {
            if (setting.WrapMode == -1)
            {
                importer.wrapModeU = setting.UAxis;
                importer.wrapModeV = setting.VAxis;
                importer.wrapModeW = setting.WAxis;
            }
            else
            {
                importer.wrapMode = (TextureWrapMode) setting.WrapMode;
            }

            importer.filterMode = setting.FilterMode;

            if (setting.FilterMode != FilterMode.Point && setting.GenerateMipMaps &&
                setting.TextureShape != TextureImporterShape.TextureCube)
            {
                importer.anisoLevel = setting.AnisoLevel;
            }
        }

        private static void MipMapSetting(AssetPostprocessSetting setting, TextureImporter importer)
        {
            if (setting.TextureType == TextureImporterType.Default || setting.TextureType == TextureImporterType.NormalMap ||
                setting.TextureType == TextureImporterType.Lightmap || setting.TextureType == TextureImporterType.SingleChannel)
            {
                importer.streamingMipmaps = setting.StreamingMipMaps;
                if (setting.StreamingMipMaps)
                {
                    importer.streamingMipmapsPriority = setting.MipMapPriority;
                }

                importer.mipmapEnabled = setting.GenerateMipMaps;
                if (setting.GenerateMipMaps)
                {
                    importer.borderMipmap = setting.BorderMipMaps;
                    importer.mipmapFilter = setting.MipmapFiltering;
                    importer.mipMapsPreserveCoverage = setting.MipMapsPreserveCoverage;
                    if (setting.MipMapsPreserveCoverage)
                    {
                        importer.alphaTestReferenceValue = setting.AlphaCutoffValue;
                    }

                    importer.fadeout = setting.FadeoutMipMaps;
                    if (setting.FadeoutMipMaps)
                    {
                        importer.mipmapFadeDistanceStart = setting.MipMapsFadeStart;
                        importer.mipmapFadeDistanceEnd = setting.MipMapsFadeEnd;
                    }
                }
            }
        }

        private static void AlphaSetting(AssetPostprocessSetting setting, TextureImporter importer)
        {
            if (setting.TextureType == TextureImporterType.Default)
            {
                importer.sRGBTexture = setting.IssRGB;
            }

            if (setting.TextureType == TextureImporterType.Default ||
                setting.TextureType == TextureImporterType.Cookie ||
                setting.TextureType == TextureImporterType.SingleChannel)
            {
                importer.alphaSource = setting.AlphaSource;

                if (setting.AlphaSource != TextureImporterAlphaSource.None)
                {
                    importer.alphaIsTransparency = setting.AlphaIsTransparent;
                }
            }
        }

        private static void CubeMapSetting(AssetPostprocessSetting setting, TextureImporter importer)
        {
            if (setting.TextureShape == TextureImporterShape.TextureCube && !CheckTextureShapeDisabled(setting) ||
                setting.TextureType == TextureImporterType.Cookie && setting.LightType == 2)
            {
                var textureImporterSettings = new TextureImporterSettings();
                importer.ReadTextureSettings(textureImporterSettings);

                importer.generateCubemap = setting.Mapping;

                if (setting.TextureType == TextureImporterType.Default)
                {
                    textureImporterSettings.cubemapConvolution = setting.ConvolutionType;
                }

                textureImporterSettings.seamlessCubemap = setting.FixupEdgeSeams;

                importer.SetTextureSettings(textureImporterSettings);
            }
        }

        private static void SpriteSetting(AssetPostprocessSetting setting, TextureImporter importer)
        {
            if (setting.TextureType != TextureImporterType.Sprite) return;

            importer.spriteImportMode = setting.SpriteMode;
            importer.spritePackingTag = setting.PackingTag;
            importer.spritePixelsPerUnit = setting.PixelsPerUnit;
            var textureImporterSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(textureImporterSettings);

            if (setting.SpriteMode != SpriteImportMode.Polygon)
            {
                textureImporterSettings.spriteMeshType = setting.MeshType;
                textureImporterSettings.spriteGenerateFallbackPhysicsShape = setting.GeneratePhysicsShape;
            }

            textureImporterSettings.spriteExtrude = setting.ExtrudeEdges;


            if (setting.SpriteMode == SpriteImportMode.Single)
            {
                importer.spritePivot = GetPivotValue((SpriteAlignment) setting.PivotSelectedValue, setting.Pivot);
            }

            importer.SetTextureSettings(textureImporterSettings);
        }

        private static bool CheckTextureShapeDisabled(AssetPostprocessSetting setting)
        {
            return setting.TextureType == TextureImporterType.GUI ||
                   setting.TextureType == TextureImporterType.Sprite ||
                   setting.TextureType == TextureImporterType.Cursor ||
                   setting.TextureType == TextureImporterType.Cookie ||
                   setting.TextureType == TextureImporterType.Lightmap;
        }

        private static Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset)
        {
            switch (alignment)
            {
                case SpriteAlignment.Center:
                    return new Vector2(0.5f, 0.5f);
                case SpriteAlignment.TopLeft:
                    return new Vector2(0.0f, 1f);
                case SpriteAlignment.TopCenter:
                    return new Vector2(0.5f, 1f);
                case SpriteAlignment.TopRight:
                    return new Vector2(1f, 1f);
                case SpriteAlignment.LeftCenter:
                    return new Vector2(0.0f, 0.5f);
                case SpriteAlignment.RightCenter:
                    return new Vector2(1f, 0.5f);
                case SpriteAlignment.BottomLeft:
                    return new Vector2(0.0f, 0.0f);
                case SpriteAlignment.BottomCenter:
                    return new Vector2(0.5f, 0.0f);
                case SpriteAlignment.BottomRight:
                    return new Vector2(1f, 0.0f);
                case SpriteAlignment.Custom:
                    return customOffset;
                default:
                    return Vector2.zero;
            }
        }
    }
}