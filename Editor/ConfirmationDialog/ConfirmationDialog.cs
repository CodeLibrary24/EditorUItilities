using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UIToolKitUtilities
{
    public class ConfirmationDialog : EditorWindow
    {
        private const string _ASSET_PATH = "Assets/UIToolkitUtilities/Editor/ConfirmationDialog";
        private const string _UXML_PATH = _ASSET_PATH + "/ConfirmationDialog.uxml";
        private const string _USS_PATH = _ASSET_PATH + "/ConfirmationDialog.uss";

        private VisualElement _mainContainer;

        private static event Action<bool> _onConfirmationClick;
        private static string _message;

        public static Vector2 WindowSize = new(300, 150);

        public static void ShowDialog(Action<bool> onConfirmation, string message = "Are you sure?")
        {
            _onConfirmationClick = onConfirmation;
            _message = message;

            ConfirmationDialog window = GetWindow<ConfirmationDialog>("Confirmation Dialog");
            window.minSize = WindowSize;
            window.maxSize = WindowSize;
            window.ShowModalUtility();
        }

        private void CreateGUI()
        {
            _mainContainer = rootVisualElement;

            LoadUxml();
            LoadUSS();
            SetupElements();
        }

        private void LoadUxml()
        {
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_UXML_PATH);
            TemplateContainer tree = uxml.Instantiate();

            for (var i = 0; i < tree.childCount; ++i)
                _mainContainer.Add(tree.ElementAt(i));
        }

        private void LoadUSS()
        {
            StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(_USS_PATH);
            _mainContainer.styleSheets.Add(uss);
        }

        private void SetupElements()
        {
            SetupMessageLabel();
            SetupButtons();
        }

        private void SetupMessageLabel()
        {
            _mainContainer.Q<Label>("confirmation-message").text = _message;
        }

        private void SetupButtons()
        {
            _mainContainer.Q<Button>("yes-button").clicked += () => OnConfirmationClicked(true);
            _mainContainer.Q<Button>("no-button").clicked += () => OnConfirmationClicked(false);
        }

        private void OnConfirmationClicked(bool value)
        {
            _onConfirmationClick?.Invoke(value);

            Close();
        }
    }
}