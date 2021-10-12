using System.Linq;
using FilterTemplateAssetPostprocessor.Layout;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace FilterTemplateAssetPostprocessor.Editor.AssetPostProcess
{
    [CustomEditor(typeof(AssetPostprocessSetting))]
    public class AssetPostprocessSettingEditor : UnityEditor.Editor
    {
        private ReorderableList excludePathReorderableList;
        private ReorderableList targetFileNamesReorderableList;
        
        private bool fbxImportSettingFold = true;
        private bool secondaryUVAdvancedOptions;
        private bool textureImportSettingFold = true;
        private bool advancedFold = true;

        private SerializedProperty targetPathProperty;
        private SerializedProperty targetFileNamesProperty;
        private SerializedProperty excludePathProperty;
        
        #region FBX
        private SerializedProperty scaleFactorProperty;
        private SerializedProperty convertUnitsProperty;
        private SerializedProperty importBlendShapeProperty;
        private SerializedProperty importVisibilityProperty;
        private SerializedProperty importCamerasProperty;
        private SerializedProperty importLightsProperty;
        private SerializedProperty preserveHierarchyProperty;
        
        private SerializedProperty meshCompressionProperty;
        private SerializedProperty readWriteEnabledProperty;
        private SerializedProperty optimizeMeshProperty;
        private SerializedProperty generateColliderProperty;
        
        private SerializedProperty keepQuadsProperty;
        private SerializedProperty weldVerticesProperty;
        private SerializedProperty indexFormatProperty;
        private SerializedProperty legacyBlendShapeNormalsProperty;
        private SerializedProperty normalsProperty;
        private SerializedProperty blendShapeNormalsProperty;
        private SerializedProperty normalsModeProperty;
        private SerializedProperty smoothnessSourceProperty;
        private SerializedProperty smoothingAngleProperty;
        private SerializedProperty tangentsProperty;
        private SerializedProperty swapUVsProperty;
        private SerializedProperty generateLightmapUVsProperty;
        private SerializedProperty secondaryUVHardAngleProperty;
        private SerializedProperty secondaryUVPackMarginProperty;
        private SerializedProperty secondaryUVAngleDistortionProperty;
        private SerializedProperty secondaryUVAreaDistortionProperty;
        
        private SerializedProperty animationTypeProperty;
        private SerializedProperty unsetJawBoneProperty;
        private SerializedProperty optimizeGameObjectProperty;
        
        private SerializedProperty importConstraintsProperty;
        private SerializedProperty importAnimationProperty;
        private SerializedProperty importMaterialsProperty;
        private SerializedProperty issRGBAlbedoColor;

        #endregion

        #region Texture

        private SerializedProperty textureTypeProperty;
        private SerializedProperty textureShapeProperty;
        
        private SerializedProperty issRGBProperty;
        private SerializedProperty alphaSourceProperty;
        private SerializedProperty alphaIsTransparentProperty;
        
        private SerializedProperty lightTypeProperty;
        
        private SerializedProperty mappingProperty;
        private SerializedProperty convolutionTypeProperty;
        private SerializedProperty fixupEdgeSeamsProperty;
        
        private SerializedProperty createFromGrayscaleProperty;
        
        private SerializedProperty spriteModeProperty;
        private SerializedProperty packingTagProperty;
        private SerializedProperty pixelsPerUnitProperty;
        private SerializedProperty meshTypeProperty;
        private SerializedProperty extrudeEdgesProperty;
        private SerializedProperty pivotSelectedValueProperty;
        private SerializedProperty pivotProperty;
        private SerializedProperty generatePhysicsShapeProperty;
        
        private SerializedProperty readWriteEnabled2Property;
        private SerializedProperty streamingMipMapsProperty;
        private SerializedProperty mipMapPriorityProperty;
        
        private SerializedProperty generateMipMapsProperty;
        private SerializedProperty borderMipMapsMapsProperty;
        private SerializedProperty mipmapFilteringProperty;
        private SerializedProperty mipMapsPreserveCoverageProperty;
        private SerializedProperty alphaCutoffValueProperty;
        private SerializedProperty fadeoutMipMapsProperty;
        private SerializedProperty mipMapsFadeStartProperty;
        private SerializedProperty mipMapsFadeEndMapsProperty;
        
        private SerializedProperty wrapModeProperty;
        private SerializedProperty uAxisProperty;
        private SerializedProperty vAxisProperty;
        private SerializedProperty wAxisProperty;
        private SerializedProperty filterModeProperty;
        private SerializedProperty anisoLevelProperty;
        
        private SerializedProperty maxSizeProperty;
        private SerializedProperty resizeAlgorithmProperty;
        private SerializedProperty formatProperty;
        private SerializedProperty compressionProperty;
        private SerializedProperty useCrunchCompressionProperty;
        private SerializedProperty compressorQualityProperty;

        #endregion

        private void OnEnable()
        {
            this.targetPathProperty = this.serializedObject.FindProperty("TargetPath");
            this.targetFileNamesProperty = this.serializedObject.FindProperty("TargetFileNames");
            this.excludePathProperty = this.serializedObject.FindProperty("ExcludePath");
            
            this.targetFileNamesReorderableList = new ReorderableList(this.serializedObject, this.targetFileNamesProperty);
            this.targetFileNamesReorderableList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Target File Name Filters");
            this.targetFileNamesReorderableList.drawElementCallback = (rect, index, isActive, isFocused) => 
            {
                var elementProperty = this.targetFileNamesProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                elementProperty.stringValue = EditorGUI.TextField(rect, elementProperty.stringValue);
            };
            
            this.excludePathReorderableList = new ReorderableList(this.serializedObject, this.excludePathProperty);
            this.excludePathReorderableList.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Exclude Path");
            this.excludePathReorderableList.drawElementCallback = (rect, index, isActive, isFocused) => 
            {
                var elementProperty = this.excludePathProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                elementProperty.stringValue = EditorGUI.TextField(rect, elementProperty.stringValue);
            };

            #region FBX

            this.scaleFactorProperty = this.serializedObject.FindProperty("ScaleFactor");
            this.convertUnitsProperty = this.serializedObject.FindProperty("ConvertUnits");
            this.importBlendShapeProperty = this.serializedObject.FindProperty("ImportBlendShapes");
            this.importVisibilityProperty = this.serializedObject.FindProperty("ImportVisibility");
            this.importCamerasProperty = this.serializedObject.FindProperty("ImportCameras");
            this.importLightsProperty = this.serializedObject.FindProperty("ImportLights");
            this.preserveHierarchyProperty = this.serializedObject.FindProperty("PreserveHierarchy");

            this.meshCompressionProperty = this.serializedObject.FindProperty("MeshCompression");
            this.readWriteEnabledProperty = this.serializedObject.FindProperty("ReadWriteEnabled");
            this.optimizeMeshProperty = this.serializedObject.FindProperty("OptimizeMesh");
            this.generateColliderProperty = this.serializedObject.FindProperty("GenerateColliders");

            this.keepQuadsProperty = this.serializedObject.FindProperty("KeepQuads");
            this.weldVerticesProperty = this.serializedObject.FindProperty("WeldVertices");
            this.indexFormatProperty = this.serializedObject.FindProperty("IndexFormat");
            this.legacyBlendShapeNormalsProperty = this.serializedObject.FindProperty("LegacyBlendShapeNormals");
            this.normalsProperty = this.serializedObject.FindProperty("Normals");
            this.blendShapeNormalsProperty = this.serializedObject.FindProperty("BlendShapeNormals");
            this.normalsModeProperty = this.serializedObject.FindProperty("NormalsMode");
            this.smoothnessSourceProperty = this.serializedObject.FindProperty("SmoothnessSource");
            this.smoothingAngleProperty = this.serializedObject.FindProperty("SmoothingAngle");
            this.tangentsProperty = this.serializedObject.FindProperty("Tangents");
            this.swapUVsProperty = this.serializedObject.FindProperty("SwapUVs");
            this.generateLightmapUVsProperty = this.serializedObject.FindProperty("GenerateLightmapUVs");
            this.secondaryUVHardAngleProperty = this.serializedObject.FindProperty("SecondaryUVHardAngle");
            this.secondaryUVPackMarginProperty = this.serializedObject.FindProperty("SecondaryUVPackMargin");
            this.secondaryUVAngleDistortionProperty = this.serializedObject.FindProperty("SecondaryUVAngleDistortion");
            this.secondaryUVAreaDistortionProperty = this.serializedObject.FindProperty("SecondaryUVAreaDistortion");

            this.animationTypeProperty = this.serializedObject.FindProperty("AnimationType");
            this.unsetJawBoneProperty = this.serializedObject.FindProperty("UnsetJawBone");
            this.optimizeGameObjectProperty = this.serializedObject.FindProperty("OptimizeGameObject");

            this.importConstraintsProperty = this.serializedObject.FindProperty("ImportConstraints");
            this.importAnimationProperty = this.serializedObject.FindProperty("ImportAnimation");

            this.importMaterialsProperty = this.serializedObject.FindProperty("ImportMaterials");
            this.issRGBAlbedoColor = this.serializedObject.FindProperty("IssRGBAlbedoColor");

            #endregion

            #region Texture

            this.textureTypeProperty = this.serializedObject.FindProperty("TextureType");
            this.textureShapeProperty = this.serializedObject.FindProperty("TextureShape");

            this.issRGBProperty = this.serializedObject.FindProperty("IssRGB");
            this.alphaSourceProperty = this.serializedObject.FindProperty("AlphaSource");
            this.alphaIsTransparentProperty = this.serializedObject.FindProperty("AlphaIsTransparent");
            
            this.lightTypeProperty = this.serializedObject.FindProperty("LightType");

            this.mappingProperty = this.serializedObject.FindProperty("Mapping");
            this.convolutionTypeProperty = this.serializedObject.FindProperty("ConvolutionType");
            this.fixupEdgeSeamsProperty = this.serializedObject.FindProperty("FixupEdgeSeams");

            this.createFromGrayscaleProperty = this.serializedObject.FindProperty("CreateFromGrayscale");

            this.spriteModeProperty = this.serializedObject.FindProperty("SpriteMode");
            this.packingTagProperty = this.serializedObject.FindProperty("PackingTag");
            this.pixelsPerUnitProperty = this.serializedObject.FindProperty("PixelsPerUnit");
            this.meshTypeProperty = this.serializedObject.FindProperty("MeshType");
            this.extrudeEdgesProperty = this.serializedObject.FindProperty("ExtrudeEdges");
            this.pivotSelectedValueProperty = this.serializedObject.FindProperty("PivotSelectedValue");
            this.pivotProperty = this.serializedObject.FindProperty("Pivot");
            this.generatePhysicsShapeProperty = this.serializedObject.FindProperty("GeneratePhysicsShape");

            this.readWriteEnabled2Property = this.serializedObject.FindProperty("ReadWriteEnabled2");

            this.streamingMipMapsProperty = this.serializedObject.FindProperty("StreamingMipMaps");
            this.mipMapPriorityProperty = this.serializedObject.FindProperty("MipMapPriority");
            this.generateMipMapsProperty = this.serializedObject.FindProperty("GenerateMipMaps");
            this.borderMipMapsMapsProperty = this.serializedObject.FindProperty("BorderMipMaps");
            this.mipmapFilteringProperty = this.serializedObject.FindProperty("MipmapFiltering");
            this.mipMapsPreserveCoverageProperty = this.serializedObject.FindProperty("MipMapsPreserveCoverage");
            this.alphaCutoffValueProperty = this.serializedObject.FindProperty("AlphaCutoffValue");
            this.fadeoutMipMapsProperty = this.serializedObject.FindProperty("FadeoutMipMaps");
            this.mipMapsFadeStartProperty = this.serializedObject.FindProperty("MipMapsFadeStart");
            this.mipMapsFadeEndMapsProperty = this.serializedObject.FindProperty("MipMapsFadeEnd");

            this.wrapModeProperty = this.serializedObject.FindProperty("WrapMode");
            this.uAxisProperty = this.serializedObject.FindProperty("UAxis");
            this.vAxisProperty = this.serializedObject.FindProperty("VAxis");
            this.wAxisProperty = this.serializedObject.FindProperty("WAxis");
            this.filterModeProperty = this.serializedObject.FindProperty("FilterMode");
            this.anisoLevelProperty = this.serializedObject.FindProperty("AnisoLevel");
            
            this.maxSizeProperty = this.serializedObject.FindProperty("MaxSize");
            this.resizeAlgorithmProperty = this.serializedObject.FindProperty("ResizeAlgorithm");
            this.formatProperty = this.serializedObject.FindProperty("Format");
            this.compressionProperty = this.serializedObject.FindProperty("Compression");
            this.useCrunchCompressionProperty = this.serializedObject.FindProperty("UseCrunchCompression");
            this.compressorQualityProperty = this.serializedObject.FindProperty("CompressorQuality");

            #endregion
        }

        public override void OnInspectorGUI()
        {
            var setting = (AssetPostprocessSetting) this.target;
            this.serializedObject.Update();

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(this.targetPathProperty);
                this.targetPathProperty.stringValue = this.targetPathProperty.stringValue.Replace("\\", "/");
                if (GUILayout.Button("...", GUILayout.MaxWidth(30f)))
                {
                    var selectFolder = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                    this.targetPathProperty.stringValue = FileUtil.GetProjectRelativePath(selectFolder);
                }
            }
            this.targetFileNamesReorderableList.DoLayoutList();

            EditorGUILayout.Space();
            
            this.excludePathReorderableList.DoLayoutList();

            EditorGUILayout.Space();

            EditorGUICustomLayout.PropertyToggleFoldGroup("Model Import Setting", ref this.fbxImportSettingFold,
                ref setting.fbxImportSettingEnabled, () =>
                {
                    this.OnModelGUI();
                    this.OnRigGUI();
                    EditorGUILayout.PropertyField(this.importConstraintsProperty);
                    EditorGUILayout.PropertyField(this.importAnimationProperty);
                    EditorGUILayout.PropertyField(this.importMaterialsProperty);
                    EditorGUILayout.PropertyField(this.issRGBAlbedoColor, Styles.IssRGBAlbedoColorStyle);
                });

            EditorGUICustomLayout.PropertyToggleFoldGroup("Texture Import Setting", ref this.textureImportSettingFold,
                ref setting.textureImportSettingEnabled, () =>
                {
                    var newTextureType = EditorGUILayout.IntPopup(Styles.TextureTypeTitle,
                        this.textureTypeProperty.intValue,
                        Styles.TextureTypeOptions, Styles.TextureTypeValues);
                    
                    if (this.CheckTextureShapeDisabled())
                    {
                        this.textureShapeProperty.intValue = 1;
                    }
                    
                    this.textureTypeProperty.intValue = newTextureType;
                    
                    using (new EditorGUI.DisabledScope(this.CheckTextureShapeDisabled()))
                    {
                        this.textureShapeProperty.intValue = EditorGUILayout.IntPopup(Styles.TextureShapeTitle,
                            this.textureShapeProperty.intValue, Styles.TextureShapeOptions, Styles.TextureShapeValues);
                    }
                    
                    EditorGUILayout.Space();
                    
                    var importerType = (TextureImporterType) this.textureTypeProperty.intValue;
                    var importerShape = this.textureShapeProperty.intValue;
                    
                    this.SpriteGUI(importerType);
                    this.CubemapGUI(importerShape, importerType);

                    if (importerType == TextureImporterType.Cookie)
                    {
                        EditorGUILayout.IntPopup(this.lightTypeProperty, Styles.LightTypeOptions, new []{0, 1, 2});
                        this.SetCookieMode(this.lightTypeProperty.intValue);
                    }
                    
                    if (importerType == TextureImporterType.NormalMap)
                    {
                        EditorGUILayout.PropertyField(this.createFromGrayscaleProperty);
                    }
                    
                    this.AlphaSettingGUI(importerType);
                    
                    EditorGUILayout.Space();
                    
                    this.AdvancedGUI(importerType);
                    
                    EditorGUILayout.Space();
                    
                    this.TextureSettingGUI();
                    
                    EditorGUILayout.Space();
                    
                    this.BaseTextureImportPlatformSettingsGUI();
                });

            this.serializedObject.ApplyModifiedProperties();
        }
        
        private void SetCookieMode(int cm)
        {
            switch (cm)
            {
                case 0:
                    this.borderMipMapsMapsProperty.intValue = 1;
                    this.uAxisProperty.intValue = 1;
                    this.wAxisProperty.intValue = 1;
                    this.vAxisProperty.intValue = 1;
                    this.mappingProperty.intValue = 6;
                    this.textureShapeProperty.intValue = 1;
                    break;
                case 1:
                    this.borderMipMapsMapsProperty.intValue = 0;
                    this.uAxisProperty.intValue = 0;
                    this.wAxisProperty.intValue = 0;
                    this.vAxisProperty.intValue = 0;
                    this.mappingProperty.intValue = 6;
                    this.textureShapeProperty.intValue = 1;
                    break;
                case 2:
                    this.borderMipMapsMapsProperty.intValue = 0;
                    this.uAxisProperty.intValue = 1;
                    this.wAxisProperty.intValue = 1;
                    this.vAxisProperty.intValue = 1;
                    this.mappingProperty.intValue = 1;
                    this.textureShapeProperty.intValue = 2;
                    break;
            }
        }

        private void BaseTextureImportPlatformSettingsGUI()
        {
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                var values = Enumerable.Range(5, 9).Select(x => Mathf.RoundToInt(Mathf.Pow(2, x))).ToArray();
                var guiContents = values.Select(x => EditorGUIUtility.TrTextContent(x.ToString())).ToArray();
                EditorGUILayout.IntPopup(this.maxSizeProperty, guiContents, values);
                EditorGUILayout.PropertyField(this.resizeAlgorithmProperty);
                EditorGUILayout.PropertyField(this.formatProperty);
                EditorGUILayout.IntPopup(this.compressionProperty, Styles.CompressionOptions, new []{0, 3, 2, 4});
                EditorGUILayout.PropertyField(this.useCrunchCompressionProperty);

                if (this.useCrunchCompressionProperty.boolValue)
                {
                    EditorGUILayout.IntSlider(this.compressorQualityProperty, 0, 100);
                }
            }
        }

        private void TextureSettingGUI()
        {
            EditorGUILayout.IntPopup(this.wrapModeProperty, Styles.WrapModeContents, Styles.WrapModeValues);
            if (this.wrapModeProperty.intValue == -1)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(this.uAxisProperty, Styles.WrapU);
                EditorGUILayout.PropertyField(this.vAxisProperty, Styles.WrapV);
                EditorGUILayout.PropertyField(this.wAxisProperty, Styles.WrapW);
                --EditorGUI.indentLevel;
            }

            if (this.filterModeProperty.intValue == -1)
            {
                this.filterModeProperty.intValue = this.fadeoutMipMapsProperty.intValue > 0 ? (int)FilterMode.Trilinear :(int) FilterMode.Bilinear;
            }
            EditorGUILayout.PropertyField(this.filterModeProperty);

            using (new EditorGUI.DisabledScope(this.filterModeProperty.intValue == 0 ||
                                               this.generateMipMapsProperty.intValue <= 0 ||
                                               this.textureShapeProperty.intValue == 2))
            {
                EditorGUILayout.IntSlider(this.anisoLevelProperty, 0, 10);
            }
        }

        private void AdvancedGUI(TextureImporterType importerType)
        {
            ++EditorGUI.indentLevel;
            this.advancedFold = EditorGUILayout.Foldout(this.advancedFold,
                "Advanced", true, EditorStyles.foldout);

            if (!this.advancedFold) return;
            ++EditorGUI.indentLevel;

            if (importerType == TextureImporterType.GUI || importerType == TextureImporterType.Sprite || importerType == TextureImporterType.Cursor)
            {
                if (importerType == TextureImporterType.Sprite)
                {
                    EditorGUILayout.PropertyField(this.issRGBProperty, Styles.IssRGBPropertyStyle);
                }
                
                this.alphaSourceProperty.intValue = EditorGUILayout.IntPopup(Styles.AlphaSourceTitle,
                    this.alphaSourceProperty.intValue, Styles.AlphaSourceOptions, Styles.AlphaSourceValues);

                using (new EditorGUI.DisabledScope(this.alphaSourceProperty.intValue == 0))
                {
                    EditorGUILayout.PropertyField(this.alphaIsTransparentProperty);
                }
            }
            
            
            EditorGUILayout.PropertyField(this.readWriteEnabled2Property, Styles.ReadWriteEnabled);
            
            this.MipMapGUI(importerType);

            --EditorGUI.indentLevel;
            --EditorGUI.indentLevel;
        }

        private void MipMapGUI(TextureImporterType importerType)
        {
            if (importerType == TextureImporterType.Default || importerType == TextureImporterType.NormalMap || importerType == TextureImporterType.Lightmap || importerType == TextureImporterType.SingleChannel)
            {
                EditorGUILayout.PropertyField(this.streamingMipMapsProperty);
                if (this.streamingMipMapsProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(this.mipMapPriorityProperty);
                }
            }

            EditorGUILayout.PropertyField(this.generateMipMapsProperty);
            if (this.generateMipMapsProperty.boolValue)
            {
                ++EditorGUI.indentLevel;

                EditorGUILayout.PropertyField(this.borderMipMapsMapsProperty);
                EditorGUILayout.PropertyField(this.mipmapFilteringProperty);
                
                EditorGUILayout.PropertyField(this.mipMapsPreserveCoverageProperty);
                if (this.mipMapsPreserveCoverageProperty.boolValue)
                {
                    ++EditorGUI.indentLevel;
                    EditorGUILayout.PropertyField(this.alphaCutoffValueProperty);
                    --EditorGUI.indentLevel;
                }

                EditorGUILayout.PropertyField(this.fadeoutMipMapsProperty);
                if (this.fadeoutMipMapsProperty.boolValue)
                {
                    ++EditorGUI.indentLevel;
                    var intValue1 = (float) this.mipMapsFadeStartProperty.intValue;
                    var intValue2 = (float) this.mipMapsFadeEndMapsProperty.intValue;

                    EditorGUILayout.MinMaxSlider(Styles.MipmapFadeOutStyle, ref intValue1, ref intValue2, 0.0f, 10f);

                    this.mipMapsFadeStartProperty.intValue = Mathf.RoundToInt(intValue1);
                    this.mipMapsFadeEndMapsProperty.intValue = Mathf.RoundToInt(intValue2);
                    --EditorGUI.indentLevel;
                }

                --EditorGUI.indentLevel;
            }
        }

        private void SpriteGUI(TextureImporterType importerType)
        {
            if (importerType != TextureImporterType.Sprite) return;

            EditorGUILayout.IntPopup(this.spriteModeProperty, Styles.SpriteModeOptions, new[] {1, 2, 3},
                Styles.SpriteModeTitle);
            var spriteMode = this.spriteModeProperty.intValue;

            ++EditorGUI.indentLevel;

            EditorGUILayout.PropertyField(this.packingTagProperty);
            EditorGUILayout.PropertyField(this.pixelsPerUnitProperty);
            if (spriteMode != 3)
            {
                EditorGUILayout.PropertyField(this.meshTypeProperty);
            }

            EditorGUILayout.PropertyField(this.extrudeEdgesProperty);

            if (spriteMode == 1)
            {
                EditorGUILayout.IntPopup(
                    this.pivotSelectedValueProperty, Styles.SpritePivotOptions,
                    new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                    Styles.SpritePivotTitle);

                if (this.pivotSelectedValueProperty.intValue == 9)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(this.pivotProperty);
                    GUILayout.EndHorizontal();
                }
            }

            if (spriteMode != 3)
            {
                EditorGUILayout.PropertyField(this.generatePhysicsShapeProperty);
            }

            --EditorGUI.indentLevel;
        }

        private void CubemapGUI(int importerShape, TextureImporterType importerType)
        {
            if (importerShape == 2 && !this.CheckTextureShapeDisabled() || importerType == TextureImporterType.Cookie && this.lightTypeProperty.intValue == 2)
            {
                this.mappingProperty.intValue = EditorGUILayout.IntPopup(Styles.MappingTitle,
                    this.mappingProperty.intValue, Styles.MappingOptions, Styles.MappingValues);
                ++EditorGUI.indentLevel;

                if (importerType == TextureImporterType.Default)
                {
                    this.convolutionTypeProperty.intValue = EditorGUILayout.IntPopup(
                        Styles.ConvolutionTypeTitle,
                        this.convolutionTypeProperty.intValue, Styles.ConvolutionTypeOptions,
                        Styles.ConvolutionTypeValues);
                }

                EditorGUILayout.PropertyField(this.fixupEdgeSeamsProperty);

                --EditorGUI.indentLevel;

                EditorGUILayout.Space();
            }
        }

        private void AlphaSettingGUI(TextureImporterType importerType)
        {
            if (importerType == TextureImporterType.Default)
            {
                EditorGUILayout.PropertyField(this.issRGBProperty, Styles.IssRGBPropertyStyle);
            }

            if (importerType == TextureImporterType.Default || importerType == TextureImporterType.Cookie ||
                importerType == TextureImporterType.SingleChannel)
            {
                this.alphaSourceProperty.intValue = EditorGUILayout.IntPopup(Styles.AlphaSourceTitle,
                    this.alphaSourceProperty.intValue, Styles.AlphaSourceOptions, Styles.AlphaSourceValues);

                using (new EditorGUI.DisabledScope(this.alphaSourceProperty.intValue == 0))
                {
                    EditorGUILayout.PropertyField(this.alphaIsTransparentProperty);
                }
            }
        }

        private bool CheckTextureShapeDisabled()
        {
            return this.textureTypeProperty.intValue == (int) TextureImporterType.GUI ||
                   this.textureTypeProperty.intValue == (int) TextureImporterType.Sprite ||
                   this.textureTypeProperty.intValue == (int) TextureImporterType.Cursor ||
                   this.textureTypeProperty.intValue == (int) TextureImporterType.Cookie ||
                   this.textureTypeProperty.intValue == (int) TextureImporterType.Lightmap;
        }

        //----------------//
        
        private void OnRigGUI()
        {
            EditorGUILayout.PropertyField(this.animationTypeProperty);

            if (this.animationTypeProperty.intValue == 3)
            {
                EditorGUILayout.PropertyField(this.unsetJawBoneProperty);
            }

            if (this.animationTypeProperty.intValue >= 2)
            {
                EditorGUILayout.PropertyField(this.optimizeGameObjectProperty);
            }
        }

        private void OnModelGUI()
        {
            // Scene
            EditorGUILayout.PropertyField(this.scaleFactorProperty);
            EditorGUILayout.PropertyField(this.convertUnitsProperty);
            EditorGUILayout.PropertyField(this.importBlendShapeProperty);
            EditorGUILayout.PropertyField(this.importVisibilityProperty);
            EditorGUILayout.PropertyField(this.importCamerasProperty);
            EditorGUILayout.PropertyField(this.importLightsProperty);
            EditorGUILayout.PropertyField(this.preserveHierarchyProperty);

            // Meshes
            EditorGUILayout.PropertyField(this.meshCompressionProperty);
            EditorGUILayout.PropertyField(this.readWriteEnabledProperty, Styles.ReadWriteEnabled);
            EditorGUILayout.PropertyField(this.optimizeMeshProperty);
            EditorGUILayout.PropertyField(this.generateColliderProperty);

            // Geometry
            EditorGUILayout.PropertyField(this.keepQuadsProperty);
            EditorGUILayout.PropertyField(this.weldVerticesProperty);
            EditorGUILayout.PropertyField(this.indexFormatProperty);
            EditorGUILayout.PropertyField(this.legacyBlendShapeNormalsProperty);
            EditorGUILayout.PropertyField(this.normalsProperty);

            var normals = (ModelImporterNormals) this.normalsProperty.intValue;

            if (normals != ModelImporterNormals.None)
            {
                if (this.legacyBlendShapeNormalsProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(this.blendShapeNormalsProperty);
                }

                EditorGUILayout.PropertyField(this.normalsModeProperty);

                if (this.legacyBlendShapeNormalsProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(this.smoothnessSourceProperty);
                }

                EditorGUILayout.Slider(this.smoothingAngleProperty, 0.0f, 180f, Styles.SmoothingAngleStyle);
                EditorGUILayout.PropertyField(this.tangentsProperty);
            }

            EditorGUILayout.PropertyField(this.swapUVsProperty, Styles.SwapUVsStyle);
            EditorGUILayout.PropertyField(this.generateLightmapUVsProperty, Styles.GenerateLightmapUVsStyle);

            if (this.generateLightmapUVsProperty.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    this.secondaryUVAdvancedOptions = EditorGUILayout.Foldout(this.secondaryUVAdvancedOptions,
                        "Lightmap UVs settings", true, EditorStyles.foldout);
                    if (this.secondaryUVAdvancedOptions)
                    {
                        using (new EditorGUI.IndentLevelScope())
                        {
                            EditorGUILayout.Slider(this.secondaryUVHardAngleProperty, 0.0f, 180f,
                                Styles.SecondaryUVHardAngleStyle);
                            EditorGUILayout.Slider(this.secondaryUVPackMarginProperty, 0.0f, 180f,
                                Styles.SecondaryUVPackMarginStyle);
                            EditorGUILayout.Slider(this.secondaryUVAngleDistortionProperty, 0.0f, 180f,
                                Styles.SecondaryUVAngleDistortionStyle);
                            EditorGUILayout.Slider(this.secondaryUVAreaDistortionProperty, 0.0f, 180f,
                                Styles.SecondaryUVAreaDistortionStyle);
                        }
                    }
                }
            }
        }

        private static class Styles
        {
            public static readonly GUIContent ReadWriteEnabled = EditorGUIUtility.TrTextContent("Read/Write Enabled");
            public static readonly GUIContent SmoothingAngleStyle = EditorGUIUtility.TrTextContent("Smoothing Angle");
            public static readonly GUIContent SwapUVsStyle = EditorGUIUtility.TrTextContent("Swap UVs");
            public static readonly GUIContent GenerateLightmapUVsStyle = EditorGUIUtility.TrTextContent("Generate Lightmap UVs");
            public static readonly GUIContent SecondaryUVHardAngleStyle = EditorGUIUtility.TrTextContent("Hard Angle");
            public static readonly GUIContent SecondaryUVPackMarginStyle = EditorGUIUtility.TrTextContent("Pack Margin");
            public static readonly GUIContent SecondaryUVAngleDistortionStyle = EditorGUIUtility.TrTextContent("Angle Error");
            public static readonly GUIContent SecondaryUVAreaDistortionStyle = EditorGUIUtility.TrTextContent("Area Error");
            public static readonly GUIContent IssRGBAlbedoColorStyle = EditorGUIUtility.TrTextContent("sRGB Albedo Color");
            
            public static readonly GUIContent TextureTypeTitle = EditorGUIUtility.TrTextContent("Texture Type");
            public static readonly GUIContent[] TextureTypeOptions =
            {
                EditorGUIUtility.TrTextContent("Default"),
                EditorGUIUtility.TrTextContent("Normal map"),
                EditorGUIUtility.TrTextContent("Editor GUI and Legacy GUI"),
                EditorGUIUtility.TrTextContent("Sprite (2D and UI)"),
                EditorGUIUtility.TrTextContent("Cursor"),
                EditorGUIUtility.TrTextContent("Cookie"),
                EditorGUIUtility.TrTextContent("Lightmap"),
                EditorGUIUtility.TrTextContent("Single Channel")
            };
            
            public static readonly int[] TextureTypeValues =
            {
                (int)TextureImporterType.Default,
                (int)TextureImporterType.NormalMap,
                (int)TextureImporterType.GUI,
                (int)TextureImporterType.Sprite,
                (int)TextureImporterType.Cursor,
                (int)TextureImporterType.Cookie,
                (int)TextureImporterType.Lightmap,
                (int)TextureImporterType.SingleChannel
            };
            
            public static readonly GUIContent TextureShapeTitle = EditorGUIUtility.TrTextContent("Texture Shape");
            public static readonly GUIContent[] TextureShapeOptions =
            {
                EditorGUIUtility.TrTextContent("2D"),
                EditorGUIUtility.TrTextContent("Cube")
            };

            public static readonly int[] TextureShapeValues = {1, 2};
            
            public static readonly GUIContent IssRGBPropertyStyle = EditorGUIUtility.TrTextContent("sRGB (Color Texture)");
            
            public static readonly GUIContent AlphaSourceTitle = EditorGUIUtility.TrTextContent("Alpha Source");

            public static readonly GUIContent[] AlphaSourceOptions = 
            {
                EditorGUIUtility.TrTextContent("None"),
                EditorGUIUtility.TrTextContent("Input Texture Alpha"),
                EditorGUIUtility.TrTextContent("From Gray Scale")
            };
            public static readonly int[] AlphaSourceValues = {0, 1, 2};
            
            public static readonly GUIContent[] LightTypeOptions =
            {
                EditorGUIUtility.TrTextContent("Spotlight"),
                EditorGUIUtility.TrTextContent("Directional"),
                EditorGUIUtility.TrTextContent("Point")
            };
            
            public  static readonly GUIContent MappingTitle = EditorGUIUtility.TrTextContent("Mapping");
            public static  readonly GUIContent[] MappingOptions = 
            {
                EditorGUIUtility.TrTextContent("Auto"),
                EditorGUIUtility.TrTextContent("6 Frames Layout (Cubic Environment)"),
                EditorGUIUtility.TrTextContent("Latitude-Longitude Layout (Cylindrical)"),
                EditorGUIUtility.TrTextContent("Mirrored Ball (Spheremap)")
            };

            public static readonly int[] MappingValues = {6, 5, 2, 2};
            
            public static readonly GUIContent ConvolutionTypeTitle = EditorGUIUtility.TrTextContent("Convolution Type");
            public static readonly GUIContent[] ConvolutionTypeOptions =
            {
                EditorGUIUtility.TrTextContent("None"),
                EditorGUIUtility.TrTextContent("Specular (Glossy Reflection)"),
                EditorGUIUtility.TrTextContent("Diffuse (Irradiance)")
            };
            public static readonly int[] ConvolutionTypeValues = {0, 1, 2};
            
            public static readonly GUIContent SpriteModeTitle = EditorGUIUtility.TrTextContent("Sprite Mode");
            public static readonly GUIContent[] SpriteModeOptions =
            {
                EditorGUIUtility.TrTextContent("Single"),
                EditorGUIUtility.TrTextContent("Multiple"),
                EditorGUIUtility.TrTextContent("Polygon")
            };
            
            public static readonly GUIContent SpritePivotTitle = EditorGUIUtility.TrTextContent("Pivot");
            public static readonly GUIContent[] SpritePivotOptions =
            {
                EditorGUIUtility.TrTextContent("Center"),
                EditorGUIUtility.TrTextContent("Top Left"),
                EditorGUIUtility.TrTextContent("Top"),
                EditorGUIUtility.TrTextContent("Top Right"),
                EditorGUIUtility.TrTextContent("Left"),
                EditorGUIUtility.TrTextContent("Right"),
                EditorGUIUtility.TrTextContent("Bottom Left"),
                EditorGUIUtility.TrTextContent("Bottom"),
                EditorGUIUtility.TrTextContent("Bottom Right"),
                EditorGUIUtility.TrTextContent("Custom")
            };
            
            public static readonly GUIContent MipmapFadeOutStyle = EditorGUIUtility.TrTextContent("Fade Range");
            
            public static readonly GUIContent WrapU = EditorGUIUtility.TrTextContent("U axis");
            public static readonly GUIContent WrapV = EditorGUIUtility.TrTextContent("V axis");
            public static readonly GUIContent WrapW = EditorGUIUtility.TrTextContent("W axis");
            public static readonly GUIContent[] WrapModeContents =
            {
                EditorGUIUtility.TrTextContent("Repeat"),
                EditorGUIUtility.TrTextContent("Clamp"),
                EditorGUIUtility.TrTextContent("Mirror"),
                EditorGUIUtility.TrTextContent("Mirror Once"),
                EditorGUIUtility.TrTextContent("Per-axis")
            };
            public static readonly int[] WrapModeValues =
            {
                0,
                1,
                2,
                3,
                -1
            };
            
            public static readonly GUIContent[] CompressionOptions =
            {
                EditorGUIUtility.TrTextContent("None"),
                EditorGUIUtility.TrTextContent("Low Quality"),
                EditorGUIUtility.TrTextContent("Normal Quality"),
                EditorGUIUtility.TrTextContent("High Quality")
            };
        }
    }
}