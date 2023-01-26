using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CircusCharlie.DataTaggingTool
{
    public class ConfirmationPopupView : DataTaggingToolPopup
    {
        protected override string uxmlPath => EditorPaths.CONFIRMATION_POPUP_PATH;
        private Action _onConfirm;
        private Action _onCancel;

        public ConfirmationPopupView(string confirmButtonText, string cancelButtonText)
        {
            confirmationButton = popupContent.Q<Button>("ConfirmButton");
            cancelButton = popupContent.Q<Button>("CancelButton");
            displayMessage = popupContent.Q<Label>("ConfirmationText");
            heading = popupContent.Q<Label>("Heading");

            confirmationButton.text = confirmButtonText;
            cancelButton.text = cancelButtonText;
            confirmationButton.clicked += () =>
            {
                _onConfirm?.Invoke();
                ClosePopup();
            };

            cancelButton.clicked += () =>
            {
                _onCancel?.Invoke();
                ClosePopup();
            };
        }

        public void SetOnConfirmAction(Action onConfirm)
        {
            _onConfirm = onConfirm;
        }

        public void SetOnCancelAction(Action onCancel)
        {
            _onCancel = onCancel;
        }
    }
}
