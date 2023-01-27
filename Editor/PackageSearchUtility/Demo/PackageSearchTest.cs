using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public class PackageSearchTest : MonoBehaviour
    {
        [SerializeField] private string packageName;

        [ContextMenu("SearchPackage")]
        public void SearchPackage()
        {
            bool result = PackageSearchUtility.DoesPackageExist(packageName);
            Debug.Log("Package exist: " + result);
        }
    }
}