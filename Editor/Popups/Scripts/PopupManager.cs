using System;
using UnityEditor;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities.Popups 
{
    public static class PopupManager
    {
        public static void ShowGenericConfirmationPopup(Rect rect, string message, Action onYesClicked, Action onNoClicked)
        {
            PopupWindow.Show(rect, new ConfirmationPopup(message, onYesClicked, onNoClicked));
        }

        public static void ShowGenericNotificationPopup(Rect rect, string heading, string message, Action onOkayClicked)
        {
            PopupWindow.Show(rect, new NotificationPopup(heading, message, onOkayClicked));
        }
    }
}