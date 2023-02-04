using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;

namespace CodeLibrary24.EditorUtilities
{
    public static class EditorUtils
    {
        public static List<T> GetAllInstances<T>() where T : ScriptableObject
        {
            return AssetDatabase.FindAssets($"t: {typeof(T).Name}").ToList().Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>).ToList();
        }

        public static List<string> GetNamesFromListOfScriptableObjects<T>(IEnumerable<T> list) where T : ScriptableObject
        {
            return list.Select(item => item.name).ToList();
        }

        private static readonly List<int> PreviouslyDisabledUIElements = new List<int>();

        public static void DisableAll(VisualElement root)
        {
            foreach (var child in root.hierarchy.Children())
            {
                if (!child.enabledSelf)
                {
                    PreviouslyDisabledUIElements.Add(child.GetHashCode());
                }

                child.SetEnabled(false);
                DisableAll(child);
            }
        }

        public static void SetOnAbsoluteFocus(VisualElement visualElement)
        {
            List<VisualElement> parents = new List<VisualElement>();
            DisableAll(FindRootVisualElement(visualElement, parents));
            foreach (var parent in parents)
            {
                parent.SetEnabled(true);
            }

            EnableAll(visualElement);
        }

        public static void EnableAll(VisualElement root)
        {
            foreach (var child in root.hierarchy.Children())
            {
                if (PreviouslyDisabledUIElements.Contains(child.GetHashCode()))
                {
                    PreviouslyDisabledUIElements.Remove(child.GetHashCode());
                }
                else
                {
                    child.SetEnabled(true);
                }

                EnableAll(child);
            }
        }

        public static VisualElement FindRootVisualElement(VisualElement parent, List<VisualElement> parents = null)
        {
            if (parent != null && parent.hierarchy.parent != null)
            {
                parents?.Add(parent);
                return FindRootVisualElement(parent.hierarchy.parent, parents);
            }

            return parent;
        }


        public static void MarkDirty()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage == null)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
            else
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        public static void SaveScene()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage == null)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
            else
            {
                PrefabUtility.SaveAsPrefabAsset(prefabStage.prefabContentsRoot, prefabStage.assetPath);
                prefabStage.ClearDirtiness();
            }
        }

        public static bool IsPrefabOpen(GameObject gameObject)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage == null)
            {
                return false;
            }

            return prefabStage.assetPath == AssetDatabase.GetAssetPath(gameObject);
        }

        public static void OpenPrefabView(GameObject gameObject)
        {
            string prefabPath = AssetDatabase.GetAssetPath(gameObject);
            GameObject assetGameObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            AssetDatabase.OpenAsset(assetGameObject);
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            GameObject instanceGameobject = prefabStage.prefabContentsRoot;
            Selection.activeGameObject = instanceGameobject;
        }

        public static void ShowInIsolation(GameObject gameObject)
        {
            SceneVisibilityManager.instance.Isolate(gameObject, true);
        }

        public static void ClosePrefabView(GameObject gameObject)
        {
            StageUtility.GoToMainStage();
        }

        public static void ExitIsolationView(GameObject gameObject)
        {
            SceneVisibilityManager.instance.ExitIsolation();
        }

        public static Bounds GetBounds(this GameObject gameObject)
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            bool hasBounds = false;
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer render in renderers)
            {
                if (hasBounds)
                {
                    bounds.Encapsulate(render.bounds);
                }
                else
                {
                    bounds = render.bounds;
                    hasBounds = true;
                }
            }

            return bounds;
        }

        public static void SaveScriptableObject<T>(T asset) where T : ScriptableObject
        {
            if (asset != null)
            {
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
            }
            else
            {
                Debug.LogError("Asset to save is null");
            }
        }
    }
}