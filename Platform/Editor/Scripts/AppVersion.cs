using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = nameof(AppVersion), menuName = "Celeste/Platform/App Version")]
    public class AppVersion : ScriptableObject
    {
        #region Properties and Fields

        public int Major => major;
        public int Minor => minor;
        public int Build => build;

        [SerializeField] private int major;
        [SerializeField] private int minor;
        [SerializeField] private int build;

        #endregion

        public override string ToString()
        {
            return $"{major}.{minor}.{build}";
        }

        public void IncrementMajor()
        {
            ++major;
            minor = 0;
            build = 0;
            
            EditorUtility.SetDirty(this);
        }

        public void IncrementMinor()
        {
            ++minor;
            build = 0;
            
            EditorUtility.SetDirty(this);
        }

        public void IncrementBuild()
        {
            ++build;
            
            EditorUtility.SetDirty(this);
        }
    }
}