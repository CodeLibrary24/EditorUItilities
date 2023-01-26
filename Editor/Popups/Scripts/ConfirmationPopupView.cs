using System;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public class ConfirmationPopupView : DataTaggingToolPopup
    {
        protected override string UxmlPath => EditorPaths.ConfirmationPopupPath;
        private Action _onConfirm;
        private Action _onCancel;

        public ConfirmationPopupView(string confirmButtonText, string cancelButtonText)
        {
            ConfirmationButton = PopupContent.Q<Button>("ConfirmButton");
            CancelButton = PopupContent.Q<Button>("CancelButton");
            DisplayMessage = PopupContent.Q<Label>("ConfirmationText");
            Heading = PopupContent.Q<Label>("Heading");

            ConfirmationButton.text = confirmButtonText;
            CancelButton.text = cancelButtonText;
            ConfirmationButton.clicked += () =>
            {
                _onConfirm?.Invoke();
                ClosePopup();
            };

            CancelButton.clicked += () =>
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
