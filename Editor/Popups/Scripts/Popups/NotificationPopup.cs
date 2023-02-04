using System;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities.Popups
{
    public class NotificationPopup : Popup
    {
        private readonly string _heading;
        private readonly string _message;
        private readonly Action _onOkayClicked;

        public NotificationPopup(string heading, string message, Action onOkayClicked)
        {
            _heading = heading;
            _message = message;
            _onOkayClicked = onOkayClicked;
        }

        protected override void CachePopupReferences(VisualElement rootVisualElement)
        {
            AddHeading(_heading);
            AddMessage(_message);
            AddOkayButton();
        }

        private void AddOkayButton()
        {
            AddButton("Okay", EditorConstants.BrightOrange, _onOkayClicked);
        }

        protected override void OnPopupOpen()
        {
        }

        protected override void OnPopupClose()
        {
        }
    }
}