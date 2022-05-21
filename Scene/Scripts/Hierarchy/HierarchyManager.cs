using UnityEngine;

namespace Celeste.Scene.Hierarchy
{
    [AddComponentMenu("Celeste/Scene/Hierarchy Manager")]
    public class HierarchyManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private StringGameObjectDictionary gameObjectCache;

        #endregion
    }
}
