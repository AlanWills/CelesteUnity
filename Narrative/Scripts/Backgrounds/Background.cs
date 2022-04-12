using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative.Backgrounds
{
    [CreateAssetMenu(fileName = nameof(Background), menuName = "Celeste/Narrative/Backgrounds/Background")]
    public class Background : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
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
