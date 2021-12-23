using Celeste.Testing;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Testing
{
    [CustomEditor(typeof(IntegrationTest))]
    public class IntegrationTestEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty mTestTypeProperty = default;
        private SerializedProperty mTestTypeArgsProperty = default;
        private int mCurrentLogPage = 0;
        private IntegrationTest mIntegrationTest = default;

        private const int mLogPageSize = 25;

        #endregion

        private void OnEnable()
        {
            mIntegrationTest = target as IntegrationTest;

            mTestTypeProperty = serializedObject.FindProperty("mTestType");
            mTestTypeArgsProperty = serializedObject.FindProperty("mTestTypeArgs");
        }

        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Status", mIntegrationTest.TestStatus.ToString());

            DrawPropertiesExcluding(serializedObject, "m_Script");

            // We have a test type set but no args, so we create them automatically here
            if (mTestTypeProperty.objectReferenceValue != null &&
                mTestTypeArgsProperty.objectReferenceValue == null)
            {
                mIntegrationTest.CreateArgs();
            }

            EditorGUILayout.Space();

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Args", CelesteEditorStyles.BoldLabel);

                if (GUILayout.Button("Create", GUILayout.ExpandWidth(false)))
                {
                    mIntegrationTest.CreateArgs();
                }

                if (GUILayout.Button("Delete Old", GUILayout.ExpandWidth(false)))
                {
                    foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(target)))
                    {
                        bool isTest = obj == target;
                        bool isInUseArgs = mTestTypeArgsProperty.objectReferenceValue != null && obj == mTestTypeArgsProperty.objectReferenceValue;

                        if (!isTest && !isInUseArgs)
                        {
                            DestroyImmediate(obj, true);
                            
                            EditorUtility.SetDirty(target);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }

            if (mTestTypeArgsProperty.objectReferenceValue != null)
            {
                using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
                {
                    Editor argsEditor = Editor.CreateEditor(mTestTypeArgsProperty.objectReferenceValue);
                    argsEditor.OnInspectorGUI();
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Save Last Results As Expected"))
            {
                mIntegrationTest.SaveLastResultsAsExpected();
            }

            if (GUILayout.Button("Run"))
            {
                IntegrationTestInvoker.instance.RunTest(target as IntegrationTest);
            }

            EditorGUILayout.Space();

            int logPages = Mathf.CeilToInt(mIntegrationTest.NumTestLogEntries / mLogPageSize);

            using (var horizontalLayout = new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                GUI.enabled = mCurrentLogPage > 0;
                if (GUILayout.Button("<<", GUILayout.ExpandWidth(false)))
                {
                    mCurrentLogPage = 0;
                }

                if (GUILayout.Button("<", GUILayout.ExpandWidth(false)))
                {
                    --mCurrentLogPage;
                }
                GUI.enabled = true;

                GUIContent logPageString = new GUIContent(mCurrentLogPage.ToString());
                Vector2 logPageStringSize = GUI.skin.label.CalcSize(logPageString);
                EditorGUILayout.LabelField(logPageString, GUILayout.ExpandWidth(false), GUILayout.Width(logPageStringSize.x));

                GUI.enabled = mCurrentLogPage < logPages - 1;
                if (GUILayout.Button(">", GUILayout.ExpandWidth(false)))
                {
                    ++mCurrentLogPage;
                }

                if (GUILayout.Button(">>", GUILayout.ExpandWidth(false)))
                {
                    mCurrentLogPage = logPages - 1;
                }
                GUI.enabled = true;
                
                GUILayout.FlexibleSpace();
            }

            for (int i = mCurrentLogPage * mLogPageSize, n = Mathf.Min((mCurrentLogPage + 1) * mLogPageSize, mIntegrationTest.NumTestLogEntries); i < n; ++i)
            {
                GUIContent logString = new GUIContent(mIntegrationTest.GetTestLogEntry(i));
                Vector2 logStringSize = GUI.skin.label.CalcSize(logString);
                EditorGUILayout.LabelField(logString, GUILayout.ExpandWidth(true), GUILayout.Height(logStringSize.y));
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
