using System;
using UnityEngine.UIElements;

namespace CodeLibrary24.EditorUtilities.Popups
{
    public class ConfirmationPopup : Popup
    {
        private readonly string _message;
        private readonly Action _onYesClicked;
        private readonly Action _onNoClicked;

        public ConfirmationPopup(string message, Action onYesClicked, Action onNoClicked)
        {
            _message = message;
            _onYesClicked = onYesClicked;
            _onNoClicked = onNoClicked;
        }

        protected override void CachePopupReferences(VisualElement rootVisualElement)
        {
        }

        protected override void OnPopupOpen()
        {
            AddHeading("Are you sure?");
            AddMessage(_message);
            AddYesNoButtons();
        }

        private void AddYesNoButtons()
        {
            AddButton("Yes", EditorConstants.BrightGreen, _onYesClicked);
            AddButton("No", EditorConstants.Red, _onNoClicked);
        }

        protected override void OnPopupClose()
        {
        }
    }
}