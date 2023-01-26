using System;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace CircusCharlie.DataTaggingTool
{
    public class NewItemCreationPopup : DataTaggingToolPopup
    {
        protected override string uxmlPath => EditorPaths.NEW_ITEM_POPUP_PATH;

        private TextField itemNameText;

        public NewItemCreationPopup(Action<string> onConfirm)
        {
            cancelButton = popupContent.Q<Button>("CancelButton");
            confirmationButton = popupContent.Q<Button>("CreateButton");
            itemNameText = popupContent.Q<TextField>("ItemName");

            confirmationButton.clicked += () => { Confirm(onConfirm); };

            itemNameText.focusable = true;
            itemNameText.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Return)
                    Confirm(onConfirm);
            });

            cancelButton.clicked += ClosePopup;
        }

        private void Confirm(Action<string> onConfirm)
        {
            if (string.IsNullOrEmpty(itemNameText.value) == false)
            {
                onConfirm?.Invoke(itemNameText.value);
                ClosePopup();
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();
            popupContent.Focus();
            itemNameText.ElementAt(1).Focus();
        }
    }
}