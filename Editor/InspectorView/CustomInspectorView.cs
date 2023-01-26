using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomInspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<CustomInspectorView, UxmlTraits>
    {
    }

    private Editor _editor;

    public void InspectTargetObject<TInspectorType>(UnityEngine.Object targetObject)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(_editor);

        VisualElement container = null;

        try
        {
            _editor = Editor.CreateEditor(targetObject, typeof(TInspectorType));
            if (targetObject != null)
            {
                container  = new IMGUIContainer(() =>
                {
                    if (targetObject != null)
                    {
                        _editor.OnInspectorGUI();
                    }
                });
              VisualElement  window = _editor.CreateInspectorGUI();
              container.Add(window);
            }
        }
        catch // If no custom inspector found, draw default inspector
        {
            _editor = Editor.CreateEditor(targetObject);
            if (targetObject != null)
            {
                container = new IMGUIContainer(() =>
                {
                    if (targetObject != null)
                    {
                        _editor.OnInspectorGUI();
                    }
                });
            }
        }

        if (container != null)
        {
            Add(container);
        }
        else
        {
            Debug.LogError("Container is null - something went wrong");
        }
    }
}