using CodeLibrary24.EditorUtilities.Popups;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupTestWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset _visualTreeAsset;

    private static PopupTestWindow _window;


    [MenuItem("CodeLibrary24/PopupTestWindow")]
    public static void ShowExample()
    {
        _window = GetWindow<PopupTestWindow>();
        _window.titleContent = new GUIContent("PopupEditorWindow");
    }

    public void CreateGUI()
    {
        DrawConfirmationPopupTest();
    }

    private void DrawConfirmationPopupTest()
    {
        Button button = new Button();
        button.text = "Show Confirmation Popup";
        rootVisualElement.Add(button);
        button.clicked += () =>
        {
            PopupManager.ShowGenericConfirmationPopup(rootVisualElement.localBound,
                "This is a dummy confirmation message.",
                () => { Debug.LogError("Yes"); },
                () => { Debug.LogError("No"); });
        };
        
    }
}