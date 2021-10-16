using Celeste.Narrative.Persistence;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [AddComponentMenu("Celeste/Narrative/Characters/Character Manager")]
    public class CharacterManager : MonoBehaviour
    {
        // Take care of tracking character positions and records
        // Hook into Enter/Exit for Narrative Runtime like view manager?
        // Will also need access to the record

        #region Properties and Fields

        [SerializeField] private CharacterCustomisationCatalogue customisationCatalogue;

        #endregion

        #region Callbacks

        public void OnNarrativeBegin(NarrativeRuntime narrativeRuntime)
        {
            // Go through each character and resolve any customisations
            ChapterRecord chapterRecord = narrativeRuntime.Record;

            for (int i = 0, n = chapterRecord.NumCharacterRecords; i < n; ++i)
            {
                CharacterRecord characterRecord = chapterRecord.GetCharacterRecord(i);
                
                if (characterRecord.AvatarCustomisationGuid != 0)
                {
                    var customisation = customisationCatalogue.FindByGuid<SpriteCharacterCustomisation>(characterRecord.AvatarCustomisationGuid);
                    UnityEngine.Debug.Assert(customisation != null, $"Could not find customisation with guid {characterRecord.AvatarCustomisationGuid}.");
                    characterRecord.CharacterAvatarIcon = customisation.Sprite;
                }
            }
        }

        #endregion
    }
}