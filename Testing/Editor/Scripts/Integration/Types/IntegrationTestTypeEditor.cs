using Celeste.Testing;
using UnityEditor;

namespace CelesteEditor.Testing
{
    [CustomEditor(typeof(IntegrationTestType), isFallback = true)]
    public class IntegrationTestTypeEditor : Editor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
