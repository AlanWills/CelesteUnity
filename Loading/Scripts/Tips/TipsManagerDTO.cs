using System.Collections.Generic;

namespace Celeste.Loading
{
    [System.Serializable]
    public class TipsManagerDTO
    {
        public List<uint> unseenIndexes;
        public List<uint> seenIndexes;

        public TipsManagerDTO(TipsRecord tipsRecord)
        {
            unseenIndexes = new List<uint>(tipsRecord.UnseenIndexes);
            seenIndexes = new List<uint>(tipsRecord.SeenIndexes);
            UnityEngine.Debug.Assert(tipsRecord.AllTips.Count == (unseenIndexes.Count + seenIndexes.Count),
                $"Tip index mismatch.  All: {tipsRecord.AllTips.Count}.  Unseen: {unseenIndexes.Count}.  Seen: {seenIndexes.Count}");
        }
    }
}