using System;
using UnityEditor;
using UnityEngine;

namespace CodeLibrary24.EditorUtilities
{
    public static class EditorPopup 
    {
        public static void ShowConfirmationPopup(Action onConfirm, string message, string confirmButtonText,
            string cancelButtonText, string heading, Action onCancel = null, bool showAtScreenCenter = false)
        {
            Rect windowPos = EditorWindow.focusedWindow.position;
            ConfirmationPopupView popupViewContent = new ConfirmationPopupView(confirmButtonText, cancelButtonText);

            popupViewContent.SetHeading(heading);
            popupViewContent.SetOnConfirmAction(onConfirm);
            popupViewContent.SetOnCancelAction(onCancel);
            popupViewContent.SetMessage(message);

            DataTaggingToolPopup.ShowPopup(windowPos, popupViewContent, showAtScreenCenter);
        }

        public static void ShowNotificationPopup(string heading, string message, Action onConfirm = null,
            bool showAtScreenCenter = false)
        {
            Rect windowPos = new Rect();
            if (EditorWindow.focusedWindow != null)
            {
                windowPos = EditorWindow.focusedWindow.position;
            }

            NotificationPopup notificationPopup = new NotificationPopup();
            notificationPopup.SetMessage(message);
            notificationPopup.SetHeading(heading);

            notificationPopup.SetOnConfirmAction(onConfirm);

            DataTaggingToolPopup.ShowPopup(windowPos, notificationPopup, showAtScreenCenter);
        }

        public static void ShowNewItemCreationPopup(Action<string> onConfirm, bool showAtScreenCenter = false)
        {
            Rect windowPos = EditorWindow.focusedWindow.position;
            NewItemCreationPopup createItemPopup = new NewItemCreationPopup(onConfirm);
            DataTaggingToolPopup.ShowPopup(windowPos, createItemPopup, showAtScreenCenter);
        }
    }
}