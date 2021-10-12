using System.Linq;
using UnityEditor;

namespace FilterTemplateAssetPostprocessor.Editor.AssetPostProcess
{
    public class ModelAssetPostprocessor : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            AssetPostprocessSettingSelector.ClearInternalData();
        }

        private void OnPreprocessModel()
        {
            var importer = this.assetImporter as ModelImporter;
            if(importer == null) return;
            
            AssetPostprocessSettingSelector.Initialize();
            var setting = AssetPostprocessSettingSelector.SelectSetting(this.assetPath, AssetPostprocessSettingSelector.ImporterType.Model);
            if(setting == null || !setting.fbxImportSettingEnabled) return;

            importer.globalScale = setting.ScaleFactor;
            importer.useFileScale = setting.ConvertUnits;
            importer.importBlendShapes = setting.ImportBlendShapes;
            importer.importVisibility = setting.ImportVisibility;
            importer.importCameras = setting.ImportCameras;
            importer.importLights = setting.ImportLights;
            importer.preserveHierarchy = setting.PreserveHierarchy;

            importer.meshCompression = setting.MeshCompression;
            importer.isReadable = setting.ReadWriteEnabled;
            importer.optimizeMesh = setting.OptimizeMesh;
            importer.addCollider = setting.GenerateColliders;
            
            importer.keepQuads = setting.KeepQuads;
            importer.weldVertices = setting.WeldVertices;
            importer.indexFormat = setting.IndexFormat;
            importer.importNormals = setting.Normals;

            if (setting.Normals != ModelImporterNormals.None)
            {
                if (setting.LegacyBlendShapeNormals)
                {
                    importer.importBlendShapeNormals = setting.BlendShapeNormals;
                    importer.normalSmoothingSource = setting.SmoothnessSource;
                }
                importer.normalCalculationMode = setting.NormalsMode;
                
                importer.normalSmoothingAngle = setting.SmoothingAngle;
                importer.importTangents = setting.Tangents;
            }

            importer.swapUVChannels = setting.SwapUVs;
            importer.generateSecondaryUV = setting.GenerateLightmapUVs;

            if (setting.GenerateLightmapUVs)
            {
                importer.secondaryUVHardAngle = setting.SecondaryUVHardAngle;
                importer.secondaryUVPackMargin = setting.SecondaryUVPackMargin;
                importer.secondaryUVAngleDistortion = setting.SecondaryUVAngleDistortion;
                importer.secondaryUVAreaDistortion = setting.SecondaryUVAreaDistortion;
            }

            importer.animationType = setting.AnimationType;

            if ((int)setting.AnimationType >= 2)
            {
                importer.optimizeGameObjects = setting.OptimizeGameObject;
            }

            importer.importConstraints = setting.ImportConstraints;
            importer.importAnimation = setting.ImportAnimation;
            importer.importMaterials = setting.ImportMaterials;
            importer.useSRGBMaterialColor = setting.IssRGBAlbedoColor;

            if (setting.AnimationType == ModelImporterAnimationType.Human && setting.UnsetJawBone)
            {
                // 顎外し
                var sourceAvatarHumanDescription = importer.humanDescription;
                sourceAvatarHumanDescription.human = sourceAvatarHumanDescription.human.Where(x => x.humanName != "Jaw").ToArray();
                importer.humanDescription = sourceAvatarHumanDescription;
            }
        }
    }
}