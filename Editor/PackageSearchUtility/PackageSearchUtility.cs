using System.IO;
using UnityEngine;

public class PackageSearchUtility
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