using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(TipsRecord), menuName = CelesteMenuItemConstants.LOADING_MENU_ITEM + "Tips/Tips Record", order = CelesteMenuItemConstants.LOADING_MENU_ITEM_PRIORITY)]
    public class TipsRecord : ScriptableObject
    {
        #region Properties and Fields

        public IReadOnlyList<string> AllTips => allTips;
        public IReadOnlyList<uint> UnseenIndexes => unseenIndexes;
        public IReadOnlyList<uint> SeenIndexes => seenIndexes;

        public string RandomTip
        {
            get
            {
                if (allTips.Count == 0)
                {
                    UnityEngine.Debug.LogAssertion($"No tips found in {nameof(TipsRecord)}.");
                    return string.Empty;
                }

                if (unseenIndexes.Count == 0)
                {
                    unseenIndexes.AddRange(seenIndexes);
                    seenIndexes.Clear();
                }

                int chosenUnseenIndex = UnityEngine.Random.Range(0, unseenIndexes.Count);
                uint tipIndex = unseenIndexes[chosenUnseenIndex];
                unseenIndexes.RemoveAt(chosenUnseenIndex);
                seenIndexes.Add(tipIndex);
                onChanged?.Invoke();

                return allTips[(int)tipIndex];
            }
        }

        [Header("Data")]
        [SerializeField] private List<string> commonTips = new List<string>();
        [SerializeField] private List<string> desktopTips = new List<string>();
        [SerializeField] private List<string> mobileTips = new List<string>();
        [SerializeField] private List<string> html5Tips = new List<string>();

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event onChanged;

        [NonSerialized] private List<string> allTips = new List<string>();
        [NonSerialized] private List<uint> unseenIndexes = new List<uint>();
        [NonSerialized] private List<uint> seenIndexes = new List<uint>();

        #endregion

        #region Save/Load Methods

        public void Initialize()
        {
            Initialize(new List<uint>(), new List<uint>());
        }

        public void Initialize(IReadOnlyList<uint> _unseenIndexes, IReadOnlyList<uint> _seenIndexes)
        {
            SetUpTips();

            seenIndexes.Clear();
            unseenIndexes.Clear();

            if (_unseenIndexes.Count + _seenIndexes.Count != allTips.Count)
            {
                // The number of total tips has changed (either we've added more in a new update or removed some)
                // Just wipe our indexes and start again
                unseenIndexes.Capacity = allTips.Count;

                for (uint i = 0; i < (uint)allTips.Count; ++i)
                {
                    unseenIndexes.Add(i);
                }
            }
            else
            {
                unseenIndexes.AddRange(_unseenIndexes);
                seenIndexes.AddRange(_seenIndexes);
            }

            UnityEngine.Debug.Assert((unseenIndexes.Count + seenIndexes.Count) == allTips.Count, 
                $"After initializing tips there was a mismatch between the indices {unseenIndexes.Count + seenIndexes.Count} and total tips {allTips.Count}.");
        }

        #endregion

        #region Utility Functions

        private void SetUpTips()
        {
            allTips.Clear();
            allTips.AddRange(commonTips);
#if UNITY_ANDROID || UNITY_IOS
            allTips.AddRange(mobileTips);
#elif UNITY_STANDALONE
            allTips.AddRange(desktopTips);
#elif UNITY_WEBGL
            allTips.AddRange(html5Tips);
#endif
        }

        #endregion
    }
}
