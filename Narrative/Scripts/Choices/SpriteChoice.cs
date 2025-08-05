using System.ComponentModel;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Narrative.Choices
{
    [DisplayName("Sprite Choice")]
    public class SpriteChoice : Choice
    {
        #region Properties and Fields

        public Sprite Sprite
        {
            get => sprite;
            set
            {
                sprite = value;
                EditorOnly.SetDirty(this);
            }
        }

        [SerializeField] private Sprite sprite;

        #endregion
    }
}