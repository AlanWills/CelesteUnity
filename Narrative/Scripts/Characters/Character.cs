using Celeste.Components;
using Celeste.Narrative.Characters.Components;
using Celeste.Narrative.UI;
using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(Character), menuName = "Celeste/Narrative/Characters/Character")]
    public class Character : ComponentContainerUsingSubAssets<CharacterComponent>, IIntGuid, IEditorInitializable
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set
            {
                if (guid != value)
                {
                    guid = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public string CharacterName
        {
            get => characterName.Value;
            set => characterName.Value = value;
        }

        public UIPosition DefaultUIPosition => defaultUIPosition;

        [SerializeField] private int guid;
        [SerializeField] private StringReference characterName;
        [SerializeField] private UIPosition defaultUIPosition = UIPosition.Centre;

        #endregion

        #region Object Overrides

        public override string ToString()
        {
            return CharacterName;
        }

        #endregion

        public void Editor_Initialize()
        {
            if (characterName == null)
            {
                characterName = CreateInstance<StringReference>();
                characterName.hideFlags = HideFlags.HideInHierarchy;
                characterName.name = "CharacterName";
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(characterName, this);
#endif
            }
        }
    }
}
