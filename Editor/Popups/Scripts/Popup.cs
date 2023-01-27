using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities.Popups
{
    public class Popup : PopupWindowContent
    {
        protected VisualElement HeaderContainer;
        protected Label HeadingLabel;
        protected VisualElement MiddleContainer;
        protected VisualElement FooterContainer;
        protected Button PopupButton;

        private const string UxmlPath = EditorPaths.BasePath + "/Popups/UI/Popup.uxml";


        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, 200);
        }

        public override void OnGUI(Rect rect)
        {
            DrawGUI(rect);
        }

        protected virtual void DrawGUI(Rect rect)
        {
        }

        public override void OnOpen()
        {
            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            visualTreeAsset.CloneTree(editorWindow.rootVisualElement);
            CacheReferences(editorWindow.rootVisualElement);
            OnPopupOpen();
        }

        private void CacheReferences(VisualElement rootVisualElement)
        {
            HeaderContainer = rootVisualElement.Q<VisualElement>("HeaderContainer");
            MiddleContainer = rootVisualElement.Q<VisualElement>("MiddleContainer");
            FooterContainer = rootVisualElement.Q<VisualElement>("FooterContainer");

            HeadingLabel = rootVisualElement.Q<Label>("Heading");
            PopupButton = rootVisualElement.Q<Button>("Button");

            CachePopupReferences(rootVisualElement);
        }

        protected virtual void CachePopupReferences(VisualElement rootVisualElement)
        {
        }

        protected virtual void OnPopupOpen()
        {
        }

        public override void OnClose()
        {
            OnPopupClose();
        }

        public void AddHeading(string heading)
        {
            HeadingLabel.text = heading;
        }

        public void AddMessage(string message)
        {
            Label label = new Label
            {
                text = message, style =
                {
                    flexGrow = 1
                }
            };

            MiddleContainer.Add(label);
        }

     
        public void AddButton(string text, Color color, Action OnClicked)
        {
            Button button = new Button
            {
                text = text, style =
                {
                    backgroundColor = color
                }
            };
            button.clicked += OnClicked;
            FooterContainer.Add(button);
        }

        protected virtual void OnPopupClose()
        {
        }
    }
}