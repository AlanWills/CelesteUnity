using PolyAndCode.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/Follow Link UI Manager")]
    public class FollowLinkUIManager : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        #region Properties and Fields

        [Header("UI")]
        [SerializeField] private RecyclableScrollRect scrollRect;

        private List<TwineNodeLink> twineNodeLinks = new List<TwineNodeLink>();

        #endregion
        public void Hookup(IReadOnlyCollection<TwineNodeLink> twineNodeLinks)
        {
            this.twineNodeLinks.Clear();
            this.twineNodeLinks.AddRange(twineNodeLinks);

            HookupCommon();
        }

        public void Hookup(List<TwineNodeLink> twineNodeLinks)
        {
            this.twineNodeLinks.Clear();
            this.twineNodeLinks.AddRange(twineNodeLinks);

            HookupCommon();
        }

        private void HookupCommon()
        {
            scrollRect.DataSource = this;
            scrollRect.ReloadData();
        }

        #region Unity Methods

        private void OnDisable()
        {
            twineNodeLinks.Clear();
        }

        #endregion

        #region Data Source Methods

        public int GetItemCount()
        {
            return twineNodeLinks.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            FollowLinkUI followLinkUI = cell as FollowLinkUI;
            followLinkUI.ConfigureCell(twineNodeLinks[index], index);
        }

        #endregion
    }
}
