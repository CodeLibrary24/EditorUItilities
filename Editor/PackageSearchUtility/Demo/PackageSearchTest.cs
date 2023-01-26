using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSearchTest : MonoBehaviour
{
    [SerializeField] private string packageName;
    [ContextMenu("Does Package Exist ?")]
    public void SearchPackage()
    {
        bool result = PackageSearchUtility.DoesPackageExist(packageName);
        Debug.Log("Package exist: "+result);
    }
}