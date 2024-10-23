using Celeste.Events;
using Celeste.Input.Settings;
using Celeste.Parameters;
using Celeste.Tools;
using Celeste.Viewport;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;

namespace CelesteEditor.Viewport
{
    public static class CelesteViewportMenuItems
    {
        [MenuItem(CelesteViewportConstants.MAKE_INTO_DRAGGABLE_CAMERA_MENU_PATH, true)]
        public static bool ValidateMakeIntoADraggableCameraContextMenuItem()
        {
            return Selection.activeGameObject != null &&
                   Selection.activeGameObject.GetComponent<Camera>();
        }

        [MenuItem(CelesteViewportConstants.MAKE_INTO_DRAGGABLE_CAMERA_MENU_PATH)]
        public static void MakeIntoADraggableCameraContextMenuItem()
        {
            GameObject gameObject = Selection.activeGameObject;
            InputEditorSettings inputEditorSettings = InputEditorSettings.GetOrCreateSettings();

            // Drag Camera script
            DragCamera dragCamera = gameObject.AddComponent<DragCamera>();
            dragCamera.OnValidate();

            FloatValue dragSpeedValue = EditorOnly.MustFindAsset<FloatValue>("CameraPanSpeedValue");
            
            if (dragSpeedValue != null)
            {
                dragCamera.DragSpeed.IsConstant = false;
                dragCamera.DragSpeed.ReferenceValue = dragSpeedValue;
            }
            else
            {
                dragCamera.DragSpeed.IsConstant = true;
                dragCamera.DragSpeed.Value = 1;
            }

            // Middle Mouse First Down script
            {
                Vector2EventListener middleMouseFirstDownListener = gameObject.AddComponent<Vector2EventListener>();
                middleMouseFirstDownListener.gameEvent = inputEditorSettings.MiddleMouseButtonFirstDown;
                UnityEventTools.AddPersistentListener(middleMouseFirstDownListener.response, dragCamera.StartDrag);
            }

            // Middle Mouse Down script
            {
                Vector2EventListener middleMouseDownListener = gameObject.AddComponent<Vector2EventListener>();
                middleMouseDownListener.gameEvent = inputEditorSettings.MiddleMouseButtonDown;
                UnityEventTools.AddPersistentListener(middleMouseDownListener.response, dragCamera.DragUsingMouse);
            }

            // Middle Mouse First Up script
            {
                Vector2EventListener middleMouseFirstUpListener = gameObject.AddComponent<Vector2EventListener>();
                middleMouseFirstUpListener.gameEvent = inputEditorSettings.MiddleMouseButtonFirstUp;
                UnityEventTools.AddVoidPersistentListener(middleMouseFirstUpListener.response, dragCamera.EndDrag);
            }

            // Single Touch script
            {
                TouchEventListener touchListener = gameObject.AddComponent<TouchEventListener>();
                touchListener.gameEvent = inputEditorSettings.SingleTouch;
                UnityEventTools.AddPersistentListener(touchListener.response, dragCamera.DragUsingTouch);
            }

            // Set Input Camera script
            {
                SetCameraValue setCameraValue = gameObject.AddComponent<SetCameraValue>();
                setCameraValue.cameraValue = inputEditorSettings.InputCamera;
            }

            EditorUtility.SetDirty(gameObject);
        }
    }
}
