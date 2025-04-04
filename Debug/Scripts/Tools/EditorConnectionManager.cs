using UnityEngine;

namespace Celeste.Debug.Tools
{
    public class EditorConnectionManager : MonoBehaviour
    {
        #region Properties and Fields
        
        [SerializeField] private EditorConnectionRecord editorConnectionRecord;
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            editorConnectionRecord.SetupEditorConnection();
        }

        private void OnDisable()
        {
            editorConnectionRecord.TeardownEditorConnection();
        }
        
        #endregion
    }
}