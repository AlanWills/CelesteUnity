using Celeste.Narrative.Characters;
using Celeste.Narrative.Parameters;
using Celeste.Narrative.Persistence;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Narrative.Choices
{
    [DisplayName("Character Avatar Icon Choice")]
    public class CharacterAvatarIconChoice : Choice, ISpriteChoice
    {
        #region Properties and Fields

        public Sprite Sprite
        {
            get { return spriteCharacterCustomisation.Sprite; }
        }

        [SerializeField] private SpriteCharacterCustomisation spriteCharacterCustomisation;
        [SerializeField] private Character character;
        [SerializeField] private ChapterRecordValue currentChapterRecord;

        #endregion

        public override void CopyFrom(Choice original)
        {
            base.CopyFrom(original);

            spriteCharacterCustomisation = (original as CharacterAvatarIconChoice).spriteCharacterCustomisation;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            character.CharacterAvatarIcon = Sprite;

            CharacterRecord characterRecord = currentChapterRecord.Value.FindCharacterRecord(character);
            characterRecord.AvatarCustomisationGuid = spriteCharacterCustomisation.Guid;
        }
    }
}