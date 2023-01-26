using System.IO;
using UnityEngine;

public class PackageSearchUtility
{
    private const string PACKAGE_MANIFEST_FILE_PATH = "Packages/manifest.json";
    public static bool DoesPackageExist(string packageName)
    {
        if (!File.Exists(PACKAGE_MANIFEST_FILE_PATH))
        {
            Debug.LogError("Package Manifest file is missing!");
            return false;
        }
        
        string jsonText = File.ReadAllText(PACKAGE_MANIFEST_FILE_PATH);
        return jsonText.Contains(packageName);
    }
}