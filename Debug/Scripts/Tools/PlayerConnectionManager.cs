using UnityEngine;

namespace Celeste.Debug.Tools
{
    public class PlayerConnectionManager : MonoBehaviour
    {
        #region Properties and Fields
        
        [SerializeField] private PlayerConnectionRecord playerConnectionRecord;
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            // The editor doesn't seem able to connect to itself, so that's good
            playerConnectionRecord.SetupConnectionToEditor();
        }

        private void OnDisable()
        {
            playerConnectionRecord.TearDownConnectionToEditor();
        }
        
        #endregion
    }
}