using Celeste.Objects;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Application
{
    [AddComponentMenu("Celeste/Application/Info")]
    public class ApplicationInfo : SceneSingleton<ApplicationInfo>
    {
        #region Properties and Fields

        public static bool IsEditor => Instance.isEditor.Value;
        public static bool IsMobile => Instance.isMobile.Value;
        public static bool IsDebugBuild => Instance.isDebugBuild.Value;

        [SerializeField] private BoolValue isEditor;
        [SerializeField] private BoolValue isMobile;
        [SerializeField] private BoolValue isDebugBuild;
        
        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            isEditor.GetIsEditor();
            isMobile.GetIsMobile();
            isDebugBuild.GetIsDebugBuild();
        }

        #endregion
    }
}
