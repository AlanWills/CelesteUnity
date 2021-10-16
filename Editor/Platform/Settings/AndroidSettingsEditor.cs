using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CustomEditor(typeof(AndroidSettings))]
    [CanEditMultipleObjects]
    public class AndroidSettingsEditor : PlatformSettingsEditor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("scriptingBackend"));
            
            AndroidSettings androidSettings = target as AndroidSettings;
            androidSettings.Architecture = (AndroidArchitecture)EditorGUILayout.EnumPopup("Architecture", androidSettings.Architecture);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
