using System;
using UnityEditor;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities.Popups
{
    public static class PopupManager
    {
        public static void ShowGenericConfirmationPopup(Rect rect, string message, Action onYesClicked, Action onNoClicked)
        {
            Popup popup = new Popup();
            PopupWindow.Show(rect, popup);
            popup.AddHeading("Are you sure?");
            popup.AddMessage(message);
            AddYesNoButtons(popup, onYesClicked, onNoClicked);
            Debug.LogError("sdssd");
        }

        public static void ShowGenericNotificationPopup(Rect rect, string heading, string message, Action onOkayClicked)
        {
            Popup popup = new Popup();
            PopupWindow.Show(rect, popup);
            popup.AddHeading(heading);
            popup.AddMessage(message);
            AddOkayButton(popup, onOkayClicked);
        }

        private static void AddOkayButton(Popup popup, Action onClicked)
        {
            popup.AddButton("Okay", EditorConstants.Orange, onClicked);
        }

        private static void AddYesNoButtons(Popup popup, Action onYesClicked, Action onNoClicked)
        {
            popup.AddButton("Yes", EditorConstants.Green, onYesClicked);
            popup.AddButton("No", EditorConstants.Red, onNoClicked);
        }
    }
}