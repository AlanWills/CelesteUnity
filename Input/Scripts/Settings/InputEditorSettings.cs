using Celeste.Events;
using Celeste.Tools.Settings;
using UnityEngine;
using Celeste.Parameters;
using Celeste.Tools;

namespace Celeste.Input.Settings
{
    [CreateAssetMenu(fileName = nameof(InputEditorSettings), menuName = CelesteMenuItemConstants.INPUT_MENU_ITEM + "Input Editor Settings", order = CelesteMenuItemConstants.INPUT_MENU_ITEM_PRIORITY)]
    public class InputEditorSettings : EditorSettings<InputEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "InputEditorSettings.asset";

        public CameraValue InputCamera => inputCameraValue;
        public CameraEvent SetInputCamera => setInputCameraEvent;

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
        [SerializeField] private CameraEvent setInputCameraEvent;

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

        #region Unity Methods

        private void OnValidate()
        {
            if (inputCameraValue == null)
            {
                inputCameraValue = EditorOnly.MustFindAsset<CameraValue>("InputCamera");
            }

            if (setInputCameraEvent == null)
            {
                setInputCameraEvent = EditorOnly.MustFindAsset<CameraEvent>("SetInputCamera");
            }

            if (leftMouseButtonFirstDownEvent == null)
            {
                leftMouseButtonFirstDownEvent = EditorOnly.MustFindAsset<Vector2Event>("LeftMouseButtonFirstDown");
            }

            if (leftMouseButtonDownEvent == null)
            {
                leftMouseButtonDownEvent = EditorOnly.MustFindAsset<Vector2Event>("LeftMouseButtonDown");
            }

            if (leftMouseButtonFirstUpEvent == null)
            {
                leftMouseButtonFirstUpEvent = EditorOnly.MustFindAsset<Vector2Event>("LeftMouseButtonFirstUp");
            }

            if (middleMouseButtonFirstDownEvent == null)
            {
                middleMouseButtonFirstDownEvent = EditorOnly.MustFindAsset<Vector2Event>("MiddleMouseButtonFirstDown");
            }

            if (middleMouseButtonDownEvent == null)
            {
                middleMouseButtonDownEvent = EditorOnly.MustFindAsset<Vector2Event>("MiddleMouseButtonDown");
            }

            if (middleMouseButtonFirstUpEvent == null)
            {
                middleMouseButtonFirstUpEvent = EditorOnly.MustFindAsset<Vector2Event>("MiddleMouseButtonFirstUp");
            }

            if (rightMouseButtonFirstDownEvent == null)
            {
                rightMouseButtonFirstDownEvent = EditorOnly.MustFindAsset<Vector2Event>("RightMouseButtonFirstDown");
            }

            if (rightMouseButtonDownEvent == null)
            {
                rightMouseButtonDownEvent = EditorOnly.MustFindAsset<Vector2Event>("RightMouseButtonDown");
            }

            if (rightMouseButtonFirstUpEvent == null)
            {
                rightMouseButtonFirstUpEvent = EditorOnly.MustFindAsset<Vector2Event>("RightMouseButtonFirstUp");
            }

            if (singleTouchEvent == null)
            {
                singleTouchEvent = EditorOnly.MustFindAsset<TouchEvent>("SingleTouch");
            }

            if (doubleTouchEvent == null)
            {
                doubleTouchEvent = EditorOnly.MustFindAsset<MultiTouchEvent>("DoubleTouch");
            }

            if (tripleTouchEvent == null)
            {
                tripleTouchEvent = EditorOnly.MustFindAsset<MultiTouchEvent>("TripleTouch");
            }
        }

        #endregion
        
#if UNITY_EDITOR
        public static InputEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            OnValidate();
        }
#endif
    }
}
