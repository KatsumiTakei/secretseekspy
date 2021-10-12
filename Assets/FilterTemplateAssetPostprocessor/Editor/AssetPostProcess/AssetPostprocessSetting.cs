using FilterTemplateAssetPostprocessor.Runtime.Attributes;
using UnityEditor;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace FilterTemplateAssetPostprocessor.Editor.AssetPostProcess
{
    [CreateAssetMenu(fileName = "AssetPostprocessSetting", menuName = "ScriptableObjects/AssetPostprocessSetting", order = 1)]
    public class AssetPostprocessSetting : ScriptableObject
    {
        [Contains("^Assets", "Path must be start \"Assets\"")]
        public string TargetPath;
        public string[] TargetFileNames = new string[]{};
        
        public string[] ExcludePath = new string[]{};
        
        [Header("FbxImportSetting")]
        public bool fbxImportSettingEnabled;
        
        [Header("Scene")]
        public float ScaleFactor = 1f;
        public bool ConvertUnits = true;
        public bool ImportBlendShapes = true;
        public bool ImportVisibility = true;
        public bool ImportCameras = true;
        public bool ImportLights = true;
        public bool PreserveHierarchy;
        
        [Header("Meshes")] 
        public ModelImporterMeshCompression MeshCompression = ModelImporterMeshCompression.Off;
        public bool ReadWriteEnabled = true;
        public bool OptimizeMesh = true;
        public bool GenerateColliders;
        
        [Header("Geometry")]
        public bool KeepQuads;
        public bool WeldVertices = true;
        public ModelImporterIndexFormat IndexFormat = ModelImporterIndexFormat.Auto;
        public bool LegacyBlendShapeNormals;
        public ModelImporterNormals Normals = ModelImporterNormals.Import;
        public ModelImporterNormals BlendShapeNormals = ModelImporterNormals.Calculate; //LegacyBlendShapeNormals = true
        public ModelImporterNormalCalculationMode NormalsMode = ModelImporterNormalCalculationMode.AreaAndAngleWeighted;
        public ModelImporterNormalSmoothingSource SmoothnessSource =
            ModelImporterNormalSmoothingSource.PreferSmoothingGroups; //LegacyBlendShapeNormals = true
        
        public float SmoothingAngle = 60f;
        public ModelImporterTangents Tangents = ModelImporterTangents.CalculateMikk;
        public bool SwapUVs;
        public bool GenerateLightmapUVs;
        public float SecondaryUVHardAngle = 88f;
        public float SecondaryUVPackMargin = 4f;
        public float SecondaryUVAngleDistortion = 8f;
        public float SecondaryUVAreaDistortion = 15f;

        [Header("Rig")]
        public ModelImporterAnimationType AnimationType = ModelImporterAnimationType.None;
        public bool UnsetJawBone;
        public bool OptimizeGameObject;

        [Header("Animation")] 
        public bool ImportConstraints;
        public bool ImportAnimation = true;
        
        [Header("Materials")] 
        public bool ImportMaterials = true;
        public bool IssRGBAlbedoColor = true;
        
        [Header("TextureImportSetting")]
        public bool textureImportSettingEnabled;
        public TextureImporterType TextureType = TextureImporterType.Default;
        public TextureImporterShape TextureShape = TextureImporterShape.Texture2D;
        
        // TextureType = Default
        public bool IssRGB = true;
        // TextureType = Default & Cookie : Cookie is None
        public TextureImporterAlphaSource AlphaSource = TextureImporterAlphaSource.FromInput;
        public bool AlphaIsTransparent;

        // TextureType = Sprite(2D and UI)
        public SpriteImportMode SpriteMode = SpriteImportMode.Single;
        public string PackingTag;
        public int PixelsPerUnit = 100;
        public SpriteMeshType MeshType = SpriteMeshType.Tight;
        public uint ExtrudeEdges = 1;
        public int PivotSelectedValue;
        public Vector2 Pivot = new Vector2(0.5f, 0.5f);
        public bool GeneratePhysicsShape = true;
        
        // TextureType = Cookie
        public int LightType = 2;
        public TextureImporterGenerateCubemap Mapping;
        public bool FixupEdgeSeams;

        // TextureType = Single Channel
        public int Channel;

        public TextureImporterCubemapConvolution ConvolutionType;

        public bool CreateFromGrayscale; // TextureType = Normal

        public bool ReadWriteEnabled2;
        public bool StreamingMipMaps;
        public int MipMapPriority; // StreamingMipMaps = true
        public bool GenerateMipMaps = true;
        public bool BorderMipMaps;
        public TextureImporterMipFilter MipmapFiltering = TextureImporterMipFilter.BoxFilter;
        public bool MipMapsPreserveCoverage;
        public float AlphaCutoffValue = 0.5f; //MipMapsPreserveCoverage = true
        public bool FadeoutMipMaps;
        public int MipMapsFadeStart = 1;
        public int MipMapsFadeEnd = 3;

        public int WrapMode = 1;
        public TextureWrapMode UAxis = TextureWrapMode.Mirror;
        public TextureWrapMode VAxis = TextureWrapMode.Mirror;
        public TextureWrapMode WAxis = TextureWrapMode.Mirror;
        public FilterMode FilterMode = FilterMode.Bilinear;
        public int AnisoLevel = 1;

        public int MaxSize = 2048;
        public TextureResizeAlgorithm ResizeAlgorithm = TextureResizeAlgorithm.Mitchell;
        public TextureImporterFormat Format = TextureImporterFormat.Automatic;
        public TextureImporterCompression Compression = TextureImporterCompression.Compressed;
        public bool UseCrunchCompression;
        public int CompressorQuality = 50;
    }
}