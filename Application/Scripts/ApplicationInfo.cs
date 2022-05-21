using Celeste.Objects;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Application
{
    [AddComponentMenu("Celeste/Application/Info")]
    public class ApplicationInfo : SceneSingleton<ApplicationInfo>
    {
        #region Properties and Fields

        [SerializeField]
        private BoolValue isEditor;
        public static bool IsEditor
        {
            get { return Instance.isEditor.Value; }
        }

        [SerializeField]
        private BoolValue isMobile;
        public static bool IsMobile
        {
            get { return Instance.isMobile.Value; }
        }

        [SerializeField]
        private BoolValue isDebugBuild;
        public static bool IsDebugBuild
        {
            get { return Instance.isDebugBuild.Value; }
        }

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
