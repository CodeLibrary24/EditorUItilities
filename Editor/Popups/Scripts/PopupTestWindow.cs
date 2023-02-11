using System;
using CodeLibrary24.EditorUtilities.Popups;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PopupTestWindow : EditorWindow
{
    [FormerlySerializedAs("_visualTreeAsset")]
    [SerializeField]
    private VisualTreeAsset visualTreeAsset;

    private static PopupTestWindow _window;


    [MenuItem("CodeLibrary24/EditorUtilities/Tests/PopupTest")]
    public static void ShowExample()
    {
        _window = GetWindow<PopupTestWindow>();
        _window.titleContent = new GUIContent("PopupEditorWindow");
    }

    public void CreateGUI()
    {
        DrawConfirmationPopupTest();
        DrawNotificationPopupTest();
    }

    private void DrawConfirmationPopupTest()
    {
        AddButton("Show Confirmation Popup", () =>
        {
            PopupManager.ShowGenericConfirmationPopup(rootVisualElement.localBound,
                "This is a dummy confirmation message.",
                () => { Debug.Log("Yes"); },
                () => { Debug.Log("No"); });
        });
    }

    private void DrawNotificationPopupTest()
    {
        AddButton("Show Notification Popup", () =>
        {
            PopupManager.ShowGenericNotificationPopup(rootVisualElement.localBound, "Alert!",
                "This is a dummy notification message.",
                () => { Debug.Log("Okay"); });
        });
    }

    private void AddButton(string buttonText, Action onClicked)
    {
        Button button = new Button
        {
            text = buttonText
        };
        rootVisualElement.Add(button);
        button.clicked += onClicked;
    }
}