using Celeste.Narrative.Characters;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative
{
    [FilePath("Assets/Narrative/Editor/NodeConstants.asset", FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(fileName = "NodeConstants", menuName = "Celeste/Narrative/Editor/Node Constants")]
    public class NodeConstants : ScriptableSingleton<NodeConstants>
    {
        #region Properties and Fields

        public Character Narrator
        {
            get { return narrator; }
        }

        [SerializeField] private Character narrator;

        #endregion
    }
}
