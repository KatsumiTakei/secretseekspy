using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace FilterTemplateAssetPostprocessor.Editor.AssetPostProcess
{
    public static class AssetPostprocessSettingSelector
    {
        public enum ImporterType
        {
            Model,
            Texture
        }
        
        private static List<AssetPostprocessSetting> assetPostprocessSettings;

        public static void Initialize()
        {
            if (assetPostprocessSettings != null) return;
            var guids = AssetDatabase.FindAssets("t:AssetPostprocessSetting");
            if (guids.Length == 0) return;

            assetPostprocessSettings = guids.Select(x =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(x);
                    return AssetDatabase.LoadAssetAtPath<AssetPostprocessSetting>(path);
                })
                .Where(x => !string.IsNullOrEmpty(x.TargetPath) && x.TargetPath.StartsWith("Assets"))
                .ToList();
        }

        public static void ClearInternalData()
        {
            if (assetPostprocessSettings == null) return;

            assetPostprocessSettings.Clear();
            assetPostprocessSettings = null;
        }
        
        public static AssetPostprocessSetting SelectSetting(string assetPath, ImporterType importerType)
        {
            if (assetPostprocessSettings == null || assetPostprocessSettings.Count == 0) return null;
            
            // 1.パスの深い順に変換(深いもの最優先)
            var group = assetPostprocessSettings
                .Where(x => importerType == ImporterType.Model ? x.fbxImportSettingEnabled : x.textureImportSettingEnabled)
                .ToLookup(x => x.TargetPath.Length)
                .OrderByDescending(x => x.Key)
                .Select(x => x.ToList())
                .ToList();

            // 2.同階層複数フィルタの優先順位を決定する
            foreach (var settings in group)
            {
                // 3.優先1 除外設定・ファイル名個別設定がされているもの
                var priority1 = settings
                    .Where(x => x.TargetFileNames.Length > 0 && x.ExcludePath.Length > 0)
                    .ToList(); // names and excludes

                // 4.優先2 除外設定がされているもの
                var priority2 = settings
                    .Where(x => x.TargetFileNames.Length == 0 && x.ExcludePath.Length > 0)
                    .ToList(); // excludes

                // 5.優先3 ファイル名個別設定がされているもの
                var priority3 = settings
                    .Where(x => x.TargetFileNames.Length > 0 && x.ExcludePath.Length == 0)
                    .ToList(); // names

                // 6.優先4 上記以外のもの
                var priority4 = settings
                    .Where(x => x.TargetFileNames.Length == 0 && x.ExcludePath.Length == 0)
                    .ToList(); // others


                // priority1
                var setting = SelectPriority1(assetPath, priority1);
                if (setting != null) return setting;

                // priority2
                setting = SelectPriority2(assetPath, priority2);
                if (setting != null) return setting;

                // priority3
                setting = SelectPriority3(assetPath, priority3);
                if (setting != null) return setting;

                // priority4
                setting = SelectPriority4(assetPath, priority4);
                if (setting != null) return setting;
            }

            return null;
        }

        private static AssetPostprocessSetting SelectPriority4(string assetPath,
            IEnumerable<AssetPostprocessSetting> priority4)
        {
            return (from assetPostprocessSetting in priority4
                let containsTargetPath = assetPath.Contains(assetPostprocessSetting.TargetPath)
                let isPriority4 = containsTargetPath
                where isPriority4
                select assetPostprocessSetting).FirstOrDefault();
        }

        private static AssetPostprocessSetting SelectPriority3(string assetPath,
            IEnumerable<AssetPostprocessSetting> priority3)
        {
            return (from assetPostprocessSetting in priority3
                let containsTargetPath = assetPath.Contains(assetPostprocessSetting.TargetPath)
                let fileName = Path.GetFileName(assetPath)
                let containsFileName =
                    assetPostprocessSetting.TargetFileNames.Any(x => Regex.IsMatch(fileName, x.Replace("*", ".*")))
                let isPriority3 = containsTargetPath && containsFileName
                where isPriority3
                select assetPostprocessSetting).FirstOrDefault();
        }

        private static AssetPostprocessSetting SelectPriority2(string assetPath,
            IEnumerable<AssetPostprocessSetting> priority2)
        {
            return (from assetPostprocessSetting in priority2
                let containsTargetPath = assetPath.Contains(assetPostprocessSetting.TargetPath)
                let replace = assetPath.Replace(assetPostprocessSetting.TargetPath, "")
                let directory = Path.GetDirectoryName(replace)
                let containsExcludePath = assetPostprocessSetting.ExcludePath.Any(directory.Contains)
                let isPriority2 = containsTargetPath && !containsExcludePath
                where isPriority2
                select assetPostprocessSetting).FirstOrDefault();
        }

        private static AssetPostprocessSetting SelectPriority1(string assetPath,
            IEnumerable<AssetPostprocessSetting> priority1)
        {
            return (from assetPostprocessSetting in priority1
                let containsTargetPath = assetPath.Contains(assetPostprocessSetting.TargetPath)
                let replace = assetPath.Replace(assetPostprocessSetting.TargetPath, "")
                let directory = Path.GetDirectoryName(replace)
                let containsExcludePath = assetPostprocessSetting.ExcludePath.Any(directory.Contains)
                let fileName = Path.GetFileName(assetPath)
                let containsFileName =
                    assetPostprocessSetting.TargetFileNames.Any(x => Regex.IsMatch(fileName, x.Replace("*", ".*")))
                let isPriority1 = containsTargetPath && !containsExcludePath && containsFileName
                where isPriority1
                select assetPostprocessSetting).FirstOrDefault();
        }
    }
}