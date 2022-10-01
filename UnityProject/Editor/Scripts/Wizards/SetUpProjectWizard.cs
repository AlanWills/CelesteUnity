using Celeste.Scene;
using CelesteEditor.BuildSystem;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CelesteEditor.UnityProject.Wizards
{
    public class SetUpProjectWizard : ScriptableWizard
    {
        #region Properties and Fields

        [Header("Build System")]
        [SerializeField] private bool runsOnWindows = true;
        [SerializeField] private bool runsOnAndroid = true;
        [SerializeField] private bool runsOniOS = true;
        [SerializeField] private bool runsOnWebGL = true;

        [Header("Code")]
        [SerializeField] private bool rootNamespaceName;

        [Header("Scenes")]
        [SerializeField] private bool needsStartupScene = true;
        [SerializeField] private bool needsBootstrapScene = true;

        [Header("Assets")]
        [SerializeField] private bool usesAddressables = true;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Set Up Project")]
        public static void ShowSetUpProjectWizard()
        {
            DisplayWizard<SetUpProjectWizard>("Set Up Project", "Set Up");
        }

        #endregion

        #region Wizard Methods

        private void OnWizardCreate()
        {
            CreateBuildSystemData();
            CreateSceneData();
            CreateAssetData();
        }

        #endregion

        #region Utility

        private void CreateBuildSystemData()
        {
            if (runsOnWindows)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateWindowsSettings();
            }

            if (runsOnAndroid)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateAndroidSettings();
            }

            if (runsOniOS)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateiOSSettings();
            }

            if (runsOnWebGL)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateWebGLSettings();
            }
        }

        private void CreateSceneData()
        {
            if (needsStartupScene)
            {
                CreateStartupScene();
            }

            if (needsBootstrapScene)
            {
                CreateBootstrapScene();
            }
        }

        private void CreateStartupScene()
        {
            AssetUtility.CreateFolder("Assets/Startup/Scenes");

            UnityEngine.SceneManagement.Scene startupScene = SceneManager.CreateScene("Startup");
            EditorSceneManager.SaveScene(startupScene, "Assets/Startup/Scenes/Startup.unity");

            SceneSet sceneSet = CreateInstance<SceneSet>();
            sceneSet.name = "StartupSceneSet";
            sceneSet.AddScene("Startup", SceneType.Baked);
            AssetUtility.CreateAssetInFolder(sceneSet, "Assets/Startup/Scenes");

            EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/Startup/Scenes/Startup.scene", true)
            };

            // Create assembly definition and menu items
            CreateAssembliesParameters startupAssembly = new CreateAssembliesParameters();
            startupAssembly.hasEditorAssembly = false;
            startupAssembly.assemblyName = $"{rootNamespaceName}.Startup";
            startupAssembly.directoryName = "Startup";
            startupAssembly.hasSceneMenuItem = true;

            CreateAssemblyDefinition.CreateAssemblies(startupAssembly);
        }

        private void CreateBootstrapScene()
        {
            AssetUtility.CreateFolder("Assets/Bootstrap/Scenes");

            UnityEngine.SceneManagement.Scene bootstrapScene = SceneManager.CreateScene("Bootstrap");
            EditorSceneManager.SaveScene(bootstrapScene, "Assets/Bootstrap/Scenes/Bootstrap.unity");

            SceneSet bootstrapSceneSet = CreateInstance<SceneSet>();
            bootstrapSceneSet.name = "BootstrapSceneSet";
            bootstrapSceneSet.AddScene("Bootstrap", SceneType.Addressable);
            AssetUtility.CreateAssetInFolder(bootstrapSceneSet, "Assets/Bootstrap/Scenes");

            SceneSet engineSystemsSceneSet = CreateInstance<SceneSet>();
            engineSystemsSceneSet.name = "EngineSystemsSceneSet";
            engineSystemsSceneSet.AddScene("EngineSystems", SceneType.Addressable);
            AssetUtility.CreateAssetInFolder(engineSystemsSceneSet, "Assets/Bootstrap/Scenes");

            // Create assembly definition and menu items
            CreateAssembliesParameters bootstrapAssembly = new CreateAssembliesParameters();
            bootstrapAssembly.hasEditorAssembly = false;
            bootstrapAssembly.assemblyName = $"{rootNamespaceName}.Bootstrap";
            bootstrapAssembly.directoryName = "Bootstrap";
            bootstrapAssembly.hasSceneMenuItem = true;

            CreateAssemblyDefinition.CreateAssemblies(bootstrapAssembly);
        }

        private void CreateAssetData()
        {

        }

        #endregion
    }
}
