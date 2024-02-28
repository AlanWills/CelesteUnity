using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative.Backgrounds
{
    [CreateAssetMenu(fileName = nameof(Background), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Backgrounds/Background", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class Background : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public Sprite Sprite
        {
            get { return sprite; }
        }

        public float AspectRatio
        {
            get
            {
                if (sprite != null)
                {
                    float width = sprite.rect.width;
                    float height = sprite.rect.height;

                    return width / height;
                }

                return 1;
            }
        }

        [SerializeField] private int guid;
        [SerializeField] private Sprite sprite;

#endregion
    }
}
