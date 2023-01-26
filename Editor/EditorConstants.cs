using UnityEngine;

namespace CircusCharlie.DataTaggingTool
{
    public class EditorConstants
    {
        public const string PREVIEW_CAMERA_PREFAB_NAME = "PreviewCamera(ForEditorOnly)";
        public const string DUMMY_PREFAB_NAME = "_PrefabDummy(EditorOnly)";

        public const string PROPERTIES_SELCECT_LIST_HEADING = "Properties";
        public const string PROPERTY_SELCECT_BUTTON_STYLE_CLASS = "button";
        public const string ASSESSMENT_LABEL_STYLE_CLASS = "assessmentView";
        public const string PROPERTY_HEADING_STYLE_CLASS = "propertyHeading";
        public const string ELEMENT_HIGHLIGHTED_STYLE_CLASS = "onHighlight";
        //Do not remove, uses based on AudioIDPicker's availability.
        public const string FEATURE_NOT_AVAILABLE_HIGHLIGHT_CLASS = "featureNotAvailable";
        public const string FEATURE_AVAILABLE_HIGHLIGHT_CLASS = "featureAvailable";

        public const string CONFIRMATION_TEXT = "Are you sure you want to delete " + ITEM_TYPE_PLACEHOLDER + " " + ITEM_NAME_PLACEHOLDER + "?";
        public const string ITEM_TYPE_PLACEHOLDER = "_ITEM_TYPE_PLACEHOLDER_";
        public const string ITEM_NAME_PLACEHOLDER = "_ITEM_NAME_PLACEHOLDER_";

        //User Prompts
        public const string TARGET_NOT_MAPPED_HEADING = "Unmapping!";
        public const string TARGET_NOT_MAPPED_MESSAGE = "Are you sure you want to unmap, this might break your transform property and Camera Preview data?";
        //Do not remove, uses based on AudioIDPicker's availability. 
        public const string INSTALL_AUDIO_ID_PICKER_MESSAGE = "Install AudioIDPicker tool for this feature!";
        public const string INSTALL_OBJECT_PLACEMENT_TOOL_MESSAGE = "Install ObjectPlacementTool tool for this feature!";
        public const string AUDIO_ID_PICKER_NOT_INSTALLED = "AudioIDPicker Not Installed!";
        public const string OBJECT_PLACEMENT_TOOL_NOT_INSTALLED = "ObjectPlacementTool Not Installed!";
        public const string SINGLE_QUOTE = "\"";
        public const string INCORRECT_VIDEO_PATH = "Incorrect Video Path!";
        public const string SELECT_CORRECT_VIDEO_FILE = "Please select a Video File from ";
        public const string SELECT_VIDEO_PATH_PLACE_HOLDER = "Select a Video File path...";

        //Asset folder names
        public const string RESOURCES_FOLDER = "Resources";
        public const string STREAMING_ASSETS_FOLDER = "StreamingAssets";

        public const string AUDIO_ID_PICKER_MANIFEST_PATH = "com.CircusCharlie.audioidpicker";
        public const string AUDIO_MANAGER_MANIFEST_PATH = "com.CircusCharlie.audiomanager";
        public const string OBJECT_PLACEMENT_TOOL_MANIFEST_PATH = "com.CircusCharlie.objectplacementtool";
        public const string VIDEO_FORMATS_FOR_VIDEO_PROPERTY = "mp4,mkv,mov,avi,flv";

        //Colors
        public static readonly Color GREEN = new Color(0.07f, 0.28f, 0.03f);
        public static readonly Color BRIGHT_GREEN = new Color(.03f, 0.56f, 0.30f);
        public static readonly Color ORANGE = new Color(0.51f, 0.26f, 0.02f);
        public static readonly Color BRIGHT_ORANGE = new Color(0.58f, 0.35f, 0.14f);
        public static readonly Color LIGHT_ORANGE = new Color(0.97f, 0.65f, 0.18f);
        public static readonly Color RED = new Color(0.51f, 0.03f, 0f);
        public static readonly Color LIGHT_RED = new Color(0.97f, 0.03f, 0f);

        public static string YES_STRING = "Yes";
        public static string NO_STRING = "No";
        public static string POPUP_HEADER_DELETE = "Delete!";
        public static float DEFAULT_CAMERA_PREVIEW_DISTRANCE = 1.5f;

        public static readonly Vector2 NEW_ELEMENT_SCROLL_OFFSET = new Vector2(0, 80);
        public static string CUSTOM_HOTSPOT_OBJECT_CREATED = "Custom Hotspot Object Created!";
        public static string MAP_TO_CUSTOM_HOTSPOT_OBJECT = "Map target to ";
    }
}