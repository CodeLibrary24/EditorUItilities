using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EditableLabel : VisualElement
{
    public new class UxmlFactory : UxmlFactory<EditableLabel>
    {
    }

    private const string _ASSET_PATH = "Assets/UIToolkitUtilities/Editor/EditableLabel";
    private const string _UXML_PATH = _ASSET_PATH + "/EditableLabel.uxml";

    private Label _label;
    private TextField _textField;
    private bool _isEditing = false;

    public Action<string> OnEditFinished;

    public string Text
    {
        set => _label.text = value;
    }

    public bool IsEditing
    {
        set
        {
            _isEditing = value;

            if (value)
                _textField.value = _label.text;

            _textField.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            _label.style.display = value ? DisplayStyle.None : DisplayStyle.Flex;
        }
        get => _isEditing;
    }

    public EditableLabel()
    {
        Initialize();
        CacheReferences();
        SetupFields();
    }

    private void Initialize()
    {
        VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_UXML_PATH);
        hierarchy.Add(uxml.Instantiate());
    }

    private void CacheReferences()
    {
        _label = this.Q<Label>();
        _textField = this.Q<TextField>();
    }

    private void SetupFields()
    {
        _label.RegisterCallback<MouseDownEvent>(e =>
        {
            if (e.button == Decimal.Zero && e.clickCount > 1)
                IsEditing = true;
        });

        _textField.focusable = true;
        _textField.RegisterCallback<FocusOutEvent>(e => CloseTextField());
        _textField.RegisterCallback<KeyDownEvent>(e =>
        {
            if (e.keyCode == KeyCode.Return)
                CloseTextField();
            ;
        });
        _textField.RegisterCallback<GeometryChangedEvent>(e=>_textField.Focus());
        
        IsEditing = false;
    }

    private void CloseTextField()
    {
        if (!IsEditing) return;
        
        OnEditFinished?.Invoke(_textField.value);
        IsEditing = false;
    }
}