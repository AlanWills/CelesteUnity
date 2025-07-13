using Celeste.Narrative.Loading;
using Celeste.Narrative.Parameters;
using Celeste.Scene;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(NarrativeContextProvider), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Loading/Narrative Context Provider", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeContextProvider : ContextProvider
    {
        [SerializeField] private ChapterRecordValue chapterRecord;

        public override Context Create()
        {
            return new NarrativeContext(chapterRecord.Value);
        }
    }
}