using Celeste.Components;
using Celeste.DeckBuilding.Components;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Info")]
    public class InfoComponent : CardComponent
    {
        #region Properties and Fields

        [SerializeField] private string displayName;
        [SerializeField] private Sprite sprite;

        #endregion

        public string GetDisplayName(Instance instance)
        {
            return displayName;
        }

        public Sprite GetSprite(Instance instance)
        {
            return sprite;
        }
    }
}