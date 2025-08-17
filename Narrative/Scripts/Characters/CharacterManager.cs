using Celeste.Narrative.Characters.Settings;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [AddComponentMenu("Celeste/Narrative/Characters/Character Manager")]
    public class CharacterManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private CharacterSettings characterSettings;

        #endregion

        #region Callbacks

        public void OnNarrativeBegin(NarrativeRuntime narrativeRuntime)
        {
            // Go through each character and resolve any customisations
            /*ChapterRecord chapterRecord = narrativeRuntime.ChapterRecord;

            for (int i = 0, n = chapterRecord.NumCharacterRecords; i < n; ++i)
            {
                CharacterRecord characterRecord = chapterRecord.GetCharacterRecord(i);
                
                if (characterRecord.AvatarCustomisationGuid != 0)
                {
                    var customisation = characterSettings.FindCustomisationByGuid<SpriteCharacterCustomisation>(characterRecord.AvatarCustomisationGuid);
                    UnityEngine.Debug.Assert(customisation != null, $"Could not find customisation with guid {characterRecord.AvatarCustomisationGuid}.");
                    characterRecord.CharacterAvatarIcon = customisation.Sprite;
                }
            }*/
        }

        #endregion
    }
}