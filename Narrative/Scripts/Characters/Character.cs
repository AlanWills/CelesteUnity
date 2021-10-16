using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Parameters.Rendering;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    public class Character : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
        }

        public string CharacterName
        {
            get { return characterName.Value; }
            set { characterName.Value = value; }
        }

        public Sprite CharacterAvatarIcon
        {
            get { return characterAvatarIcon.Value; }
            set { characterAvatarIcon.Value = value; }
        }

        [SerializeField] private int guid;
        [SerializeField] private StringReference characterName;
        [SerializeField] private SpriteReference characterAvatarIcon;

        #endregion

        public static Character Create()
        {
            Character character = CreateInstance<Character>();
            character.Repair();

            return character;
        }

        public void Repair()
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

            if (characterAvatarIcon == null)
            {
                characterAvatarIcon = CreateInstance<SpriteReference>();
                characterAvatarIcon.hideFlags = HideFlags.HideInHierarchy;
                characterAvatarIcon.name = "CharacterAvatarIcon";
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(characterAvatarIcon, this);
#endif
            }
        }
    }
}
