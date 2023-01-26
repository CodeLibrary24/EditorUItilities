using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace CircusCharlie.DataTaggingTool
{
    public abstract class DataTaggingToolPopup : PopupWindowContent
    {
        protected VisualElement popupContent;
        protected Button confirmationButton;
        protected Button cancelButton;
        protected Label displayMessage;
        protected Label heading;

        protected abstract string uxmlPath { get; }

        protected DataTaggingToolPopup()
        {
            LoadUxml();
        }

        private void LoadUxml()
        {
            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            if (visualTreeAsset == null)
            {
                Debug.LogError("Uxml file not found at path: " + uxmlPath);
            }
            popupContent = visualTreeAsset.Instantiate();
        }

        public override void OnOpen()
        {
            editorWindow.rootVisualElement.Add(popupContent);
            editorWindow.rootVisualElement.AddToClassList("popupLayout");
            EditorUtils.FindRootVisualElement(editorWindow.rootVisualElement).AddToClassList("popupLayout");
        }

        public void SetHeading(string popupHeading)
        {
            heading.text = popupHeading;
        }

        public void SetMessage(string message)
        {
            displayMessage.text = message;
        }

        public void ClosePopup()
        {
            editorWindow.Close();
        }

        //Set the window size
        public override Vector2 GetWindowSize()
        {
            //It should be equal to the size of the VisualAsset for popup. 
            return new Vector2(300, 160);
        }

        public override void OnGUI(Rect rect)
        {
            // Intentionally left empty
        }

        public override void OnClose()
        {
        }

        public static void ShowPopup(Rect windowPos, PopupWindowContent popupContent, bool showAtScreenCenter)
        {
            Rect positionRect;

            if (EditorWindow.focusedWindow == null || showAtScreenCenter)
            {
                positionRect = GetCenterOfMainWindow(popupContent.GetWindowSize());

            }
            else
            {
                Vector2 localCenter = windowPos.center - windowPos.position; // TODO: Don't calculate pos if showing at screen center, all this code to be shifted to popup class only, not editor utils
                Vector2 popupPos = new Vector2(localCenter.x - popupContent.GetWindowSize().x / 2,
                    localCenter.y - popupContent.GetWindowSize().y / 2);
                positionRect = new Rect(popupPos, Vector2.zero);
            }

            PopupWindow.Show(positionRect, popupContent);
        }

        public static Rect GetCenterOfMainWindow(Vector2 windowSize)
        {
            Rect main = EditorGUIUtility.GetMainWindowPosition();
            Rect pos = new Rect
            {
                x = (main.width - windowSize.x) * 0.5f,
                y = (main.height - windowSize.y) * 0.5f
            };
            return pos;
        }
    }
}