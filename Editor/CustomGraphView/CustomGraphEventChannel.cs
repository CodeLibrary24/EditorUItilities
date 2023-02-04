using System;

namespace CodeLibrary24.EditorUtilities
{
    public class CustomGraphEventChannel
    {
        private static CustomGraphEventChannel _instance = null;
        public static CustomGraphEventChannel Instance => _instance ??= new CustomGraphEventChannel();

        public Action OnClearViewsRequested;
        public Action OnChangeDetected;
    }
}