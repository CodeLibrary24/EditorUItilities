using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
// using Unity.EditorCoroutines.Editor; // TODO: Is this needed
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

        public static void ScrollToLast(ScrollView scrollView, float delayTime = 0.1f)
        {
            // TODO: Is this needed?
            // EditorCoroutineUtility.StartCoroutine(DelayedScrollWithOffset(scrollView, delayTime, 0, int.MaxValue), scrollView);
        }

        public static void ScrollToTop(ScrollView scrollView, float delayTime = 0.1f)
        {
            // TODO: Is this needed?
            // EditorCoroutineUtility.StartCoroutine(DelayedScrollWithOffset(scrollView, delayTime, 0, 0), scrollView);
        }

        // TODO: Is this needed?
        // private static IEnumerator DelayedScrollWithOffset(ScrollView scrollView, float delayTime, int horizontalOffset, int verticalOffset)
        // {
        //     yield return new EditorWaitForSeconds(delayTime);
        //     if (scrollView != null)
        //     {
        //         scrollView.scrollOffset = new Vector2(horizontalOffset, verticalOffset);
        //     }
        // }

        public static void SetElementOnFocus(VisualElement element)
        {
            SetOnAbsoluteFocus(element);
            // element.AddToClassList(EditorConstants.ELEMENT_HIGHLIGHTED_STYLE_CLASS);
        }

        public static void RemoveFocusFromElement(VisualElement element)
        {
            EnableAll(FindRootVisualElement(element));
            // element.RemoveFromClassList(EditorConstants.ELEMENT_HIGHLIGHTED_STYLE_CLASS);
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

            if (prefabStage.assetPath == AssetDatabase.GetAssetPath(gameObject))
            {
                return true;
            }

            return false;
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
            // Utils.EditorUtils.FocusOnObjectInSceneView(gameObject); //TODO: Focus on object in scene view
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

        public static void FocusObjectWithDelay(GameObject gameObject, float delayTime = 0.08f)
        {
            // EditorCoroutineUtility.StartCoroutine(DelayedFocus(gameObject, delayTime), gameObject); // TODO: delayed focus here
        }

        // TODO: Delayed focus
        // static IEnumerator DelayedFocus(GameObject gameObject, float delayTime)
        // {
        //     yield return new EditorWaitForSeconds(delayTime);
        //     Utils.EditorUtils.FocusOnObjectInSceneView(gameObject);
        // }

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

    }
}