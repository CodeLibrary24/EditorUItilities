using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace CodeLibrary24.EditorUtilities.Popups
{
    public abstract class Popup : PopupWindowContent
    {
        protected VisualElement HeaderContainer;
        protected Label HeadingLabel;
        protected VisualElement MiddleContainer;
        protected VisualElement FooterContainer;

        private const string UxmlPath = EditorPaths.BasePath + "/Popups/UI/Popup.uxml";


        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, 200);
        }

        public override void OnGUI(Rect rect)
        {
            OnPopupGUI(rect);
        }

        protected virtual void OnPopupGUI(Rect rect)
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

            CachePopupReferences(rootVisualElement);
        }

        protected abstract void CachePopupReferences(VisualElement rootVisualElement);

        protected abstract void OnPopupOpen();

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
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleCenter
                    
                }
            };

            MiddleContainer.Add(label);
        }


        public void AddButton(string text, Color color, Action onClicked)
        {
            Button button = new Button
            {
                text = text, style =
                {
                    backgroundColor = color,
                    flexGrow = 1
                    
                }
            };
            button.clicked += () =>
            {
                onClicked?.Invoke();
                editorWindow.Close();
            };
            FooterContainer.Add(button);
        }

        protected abstract void OnPopupClose();
    }
}