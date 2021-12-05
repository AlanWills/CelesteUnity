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

        private TwineNode twineNode;

        #endregion

        public void Hookup(TwineNode twineNode)
        {
            this.twineNode = twineNode;

            scrollRect.DataSource = this;
            scrollRect.ReloadData();
        }

        #region Data Source Methods

        public int GetItemCount()
        {
            return twineNode.links.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            FollowLinkUI followLinkUI = cell as FollowLinkUI;
            followLinkUI.ConfigureCell(twineNode.links[index], index);
        }

        #endregion
    }
}
