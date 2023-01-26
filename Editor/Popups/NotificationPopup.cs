using System;
using UnityEngine.UIElements;

namespace CircusCharlie.DataTaggingTool
{
    public class NotificationPopup : DataTaggingToolPopup
    {
        protected override string uxmlPath => EditorPaths.NOTIFICATION_POPUP_PATH;
        private Action _onConfirm;

        public NotificationPopup()
        {
            confirmationButton = popupContent.Q<Button>("OkayButton");
            displayMessage = popupContent.Q<Label>("NotificationMessage");
            heading = popupContent.Q<Label>("NotificationHeading");

            confirmationButton.clicked += () =>
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