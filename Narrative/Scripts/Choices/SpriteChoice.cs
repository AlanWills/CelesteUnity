using System.ComponentModel;
using UnityEngine;

namespace Celeste.Narrative.Choices
{
    [DisplayName("Sprite Choice")]
    public class SpriteChoice : Choice
    {
        #region Properties and Fields

        public Sprite Sprite
        {
            get { return sprite; }
            set
            {
                sprite = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private Sprite sprite;

        #endregion

        public override void CopyFrom(Choice original)
        {
            base.CopyFrom(original);

            sprite = (original as SpriteChoice).sprite;
        }
    }
}