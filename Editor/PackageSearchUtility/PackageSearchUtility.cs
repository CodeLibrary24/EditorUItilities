using System.IO;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public static class PackageSearchUtility
    {
        private const string PackageManifestFilePath = "Packages/manifest.json";

        public static bool DoesPackageExist(string packageName)
        {
            if (!File.Exists(PackageManifestFilePath))
            {
                Debug.LogError("Package Manifest file is missing!");
                return false;
            }

            string jsonText = File.ReadAllText(PackageManifestFilePath);
            return jsonText.Contains(packageName);
        }
    }
}