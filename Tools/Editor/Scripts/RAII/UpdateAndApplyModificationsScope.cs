using System;
using UnityEditor;

namespace CelesteEditor.Tools
{
    public class UpdateAndApplyModificationsScope : IDisposable
    {
        #region Properties and Fields

        private SerializedObject serializedObject;

        #endregion

        public UpdateAndApplyModificationsScope(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
            
            serializedObject.Update();
        }

        public void Dispose()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}