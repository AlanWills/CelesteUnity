using Celeste.Objects;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Narrative.Backgrounds
{
    [CreateAssetMenu(fileName = nameof(Background), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Backgrounds/Background", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class Background : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set
            {
                guid = value;
                EditorOnly.SetDirty(this);
            }
        }

        public Sprite Sprite => sprite;
        
        public float DefaultOffset => defaultOffset;

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
        [SerializeField] private float defaultOffset;

        #endregion
    }
}
