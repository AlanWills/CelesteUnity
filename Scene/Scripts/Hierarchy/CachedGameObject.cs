using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.Scene.Hierarchy
{
    [AddComponentMenu("Celeste/Scene/Cached Game Object")]
    public class CachedGameObject : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private string cacheId;
        [SerializeField] private StringGameObjectDictionary gameObjectCache;

        [Header("Caching Options")]
        [SerializeField, HideIf(nameof(cacheOnEnable))] private bool cacheOnAwake = true;
        [SerializeField, HideIf(nameof(cacheOnAwake))] private bool cacheOnEnable = false;
        [SerializeField] private bool uncacheOnDisable = false;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(cacheId))
            {
                cacheId = name;
            }

            if (cacheOnAwake)
            {
                cacheOnEnable = false;
            }
            
            if (cacheOnEnable)
            {
                cacheOnAwake = false;
            }
        }

        private void Awake()
        {
            if (cacheOnAwake)
            {
                Cache();
            }
        }

        private void OnEnable()
        {
            if (cacheOnEnable)
            {
                Cache();
            }
        }

        private void OnDisable()
        {
            if (uncacheOnDisable)
            {
                Uncache();
            }
        }

        private void OnDestroy()
        {
            if (!uncacheOnDisable)
            {
                Uncache();
            }
        }

        #endregion

        public void Cache()
        {
            gameObjectCache.AddItem(cacheId, gameObject);
        }

        public void Uncache()
        {
            gameObjectCache.RemoveItem(cacheId);
        }
    }
}
