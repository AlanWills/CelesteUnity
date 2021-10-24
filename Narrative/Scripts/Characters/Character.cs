using Celeste.Narrative.UI;
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

        public UIPosition DefaultUIPosition
        {
            get { return defaultUIPosition; }
        }

        [SerializeField] private int guid;
        [SerializeField] private StringReference characterName;
        [SerializeField] private SpriteReference characterAvatarIcon;
        [SerializeField] private UIPosition defaultUIPosition = UIPosition.Centre;

        #endregion

        public static Character Create(string characterName, string directory)
        {
            Character character = ScriptableObject.CreateInstance<Character>();
            character.name = characterName;

#if UNITY_EDITOR
            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"{directory}/{characterName}.asset");

            UnityEditor.AssetDatabase.CreateAsset(character, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
            character.Repair();
            character.CharacterName = characterName;

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
