using Celeste.Narrative.Persistence;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateAssetMenu(fileName = nameof(ChapterRecordValue), menuName = "Celeste/Parameters/Narrative/Chapter Record Value")]
    public class ChapterRecordValue : ParameterValue<ChapterRecord>
    {
    }
}
