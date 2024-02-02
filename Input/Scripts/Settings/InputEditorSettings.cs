using Celeste.Events;
using Celeste.Tools.Settings;
using UnityEngine;
using Celeste.Parameters;
#if UNITY_EDITOR
using CelesteEditor.Tools;
#endif

namespace Celeste.Input.Settings
{
    [CreateAssetMenu(fileName = nameof(InputEditorSettings), menuName = "Celeste/Input/Input Editor Settings")]
    public class InputEditorSettings : EditorSettings<InputEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Input/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "InputEditorSettings.asset";

        public CameraValue InputCamera => inputCameraValue;

        public Vector2Event LeftMouseButtonFirstDown => leftMouseButtonFirstDownEvent;
        public Vector2Event LeftMouseButtonDown => leftMouseButtonDownEvent;
        public Vector2Event LeftMouseButtonFirstUp => leftMouseButtonFirstUpEvent;

        public Vector2Event MiddleMouseButtonFirstDown => middleMouseButtonFirstDownEvent;
        public Vector2Event MiddleMouseButtonDown => middleMouseButtonDownEvent;
        public Vector2Event MiddleMouseButtonFirstUp => middleMouseButtonFirstUpEvent;

        public Vector2Event RightMouseButtonFirstDown => rightMouseButtonFirstDownEvent;
        public Vector2Event RightMouseButtonDown => rightMouseButtonDownEvent;
        public Vector2Event RightMouseButtonFirstUp => rightMouseButtonFirstUpEvent;

        public TouchEvent SingleTouch => singleTouchEvent;
        public MultiTouchEvent DoubleTouch => doubleTouchEvent;
        public MultiTouchEvent TripleTouch => tripleTouchEvent;

        [SerializeField] private CameraValue inputCameraValue;

        [Header("Left Mouse Button Events")]
        [SerializeField] private Vector2Event leftMouseButtonFirstDownEvent;
        [SerializeField] private Vector2Event leftMouseButtonDownEvent;
        [SerializeField] private Vector2Event leftMouseButtonFirstUpEvent;

        [Header("Middle Mouse Button Events")]
        [SerializeField] private Vector2Event middleMouseButtonFirstDownEvent;
        [SerializeField] private Vector2Event middleMouseButtonDownEvent;
        [SerializeField] private Vector2Event middleMouseButtonFirstUpEvent;

        [Header("Right Mouse Button Events")]
        [SerializeField] private Vector2Event rightMouseButtonFirstDownEvent;
        [SerializeField] private Vector2Event rightMouseButtonDownEvent;
        [SerializeField] private Vector2Event rightMouseButtonFirstUpEvent;

        [Header("Touch Events")]
        [SerializeField] private TouchEvent singleTouchEvent;
        [SerializeField] private MultiTouchEvent doubleTouchEvent;
        [SerializeField] private MultiTouchEvent tripleTouchEvent;

        #endregion

#if UNITY_EDITOR
        public static InputEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            inputCameraValue = AssetUtility.FindAsset<CameraValue>("InputCamera");

            leftMouseButtonFirstDownEvent = AssetUtility.FindAsset<Vector2Event>("LeftMouseButtonFirstDown");
            leftMouseButtonDownEvent = AssetUtility.FindAsset<Vector2Event>("LeftMouseButtonDown");
            leftMouseButtonFirstUpEvent = AssetUtility.FindAsset<Vector2Event>("LeftMouseButtonFirstUp");

            middleMouseButtonFirstDownEvent = AssetUtility.FindAsset<Vector2Event>("MiddleMouseButtonFirstDown");
            middleMouseButtonDownEvent = AssetUtility.FindAsset<Vector2Event>("MiddleMouseButtonDown");
            middleMouseButtonFirstUpEvent = AssetUtility.FindAsset<Vector2Event>("MiddleMouseButtonFirstUp");

            rightMouseButtonFirstDownEvent = AssetUtility.FindAsset<Vector2Event>("RightMouseButtonFirstDown");
            rightMouseButtonDownEvent = AssetUtility.FindAsset<Vector2Event>("RightMouseButtonDown");
            rightMouseButtonFirstUpEvent = AssetUtility.FindAsset<Vector2Event>("RightMouseButtonFirstUp");

            singleTouchEvent = AssetUtility.FindAsset<TouchEvent>("SingleTouch");
            doubleTouchEvent = AssetUtility.FindAsset<MultiTouchEvent>("DoubleTouch");
            tripleTouchEvent = AssetUtility.FindAsset<MultiTouchEvent>("TripleTouch");
        }
#endif
    }
}
