using System;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace CodeLibrary24.EditorUtilities
{
    public class NewItemCreationPopup : DataTaggingToolPopup
    {
        protected override string UxmlPath => EditorPaths.NewItemPopupPath;

        private readonly TextField _itemNameText;

        public NewItemCreationPopup(Action<string> onConfirm)
        {
            CancelButton = PopupContent.Q<Button>("CancelButton");
            ConfirmationButton = PopupContent.Q<Button>("CreateButton");
            _itemNameText = PopupContent.Q<TextField>("ItemName");

            ConfirmationButton.clicked += () => { Confirm(onConfirm); };

            _itemNameText.focusable = true;
            _itemNameText.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Return)
                    Confirm(onConfirm);
            });

            CancelButton.clicked += ClosePopup;
        }

        private void Confirm(Action<string> onConfirm)
        {
            if (string.IsNullOrEmpty(_itemNameText.value) == false)
            {
                onConfirm?.Invoke(_itemNameText.value);
                ClosePopup();
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();
            PopupContent.Focus();
            _itemNameText.ElementAt(1).Focus();
        }
    }
}