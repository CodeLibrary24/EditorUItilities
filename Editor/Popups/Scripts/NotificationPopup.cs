using System;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities
{
    public class NotificationPopup : DataTaggingToolPopup
    {
        protected override string UxmlPath => EditorPaths.NotificationPopupPath;
        private Action _onConfirm;

        public NotificationPopup()
        {
            ConfirmationButton = PopupContent.Q<Button>("OkayButton");
            DisplayMessage = PopupContent.Q<Label>("NotificationMessage");
            Heading = PopupContent.Q<Label>("NotificationHeading");

            ConfirmationButton.clicked += () =>
            {
                _onConfirm?.Invoke();
                ClosePopup();
            };
        }

        public void SetOnConfirmAction(Action onConfirm)
        {
            _onConfirm = onConfirm;
        }
    }
}