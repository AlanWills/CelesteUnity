namespace CelesteEditor.UnityProject.Constants
{
    public static class StartupConstants
    {
        public const string FOLDER_NAME = "Startup/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string LOAD_JOBS_FOLDER_PATH = FOLDER_PATH + "Jobs/";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string LOAD_CONTENT_CATALOGUE_LOAD_JOB_NAME = "LoadContentCatalogue";
        public const string ENABLE_BUNDLED_ADDRESSABLES_LOAD_JOB_NAME = "EnableBundledAddressables";
        public const string LOAD_BOOTSTRAP_SCENE_SET_LOAD_JOB_NAME = "LoadBootstrapSceneSet";
        public const string LOAD_JOB_NAME = "StartupLoadJob";
        public const string SCENE_NAME = "Startup";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
        public const string SCENE_SET_NAME = "StartupSceneSet";
        public const string NAMESPACE_NAME = "Startup";
    }

    public static class BootstrapConstants
    {
        public const string FOLDER_NAME = "Bootstrap/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string LOAD_JOBS_FOLDER_PATH = FOLDER_PATH + "Jobs/";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string LOAD_ENGINE_SYSTEMS_SCENE_SET_LOAD_JOB_NAME = "LoadEngineSystemsSceneSet";
        public const string LOAD_JOB_NAME = "BootstrapLoadJob";
        public const string SCENE_NAME = "Bootstrap";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
        public const string SCENE_SET_NAME = "BootstrapSceneSet";
        public const string NAMESPACE_NAME = "Bootstrap";
        public const string BOOTSTRAP_MANAGER_PREFAB_NAME = "BootstrapManager";
    }

    public static class EngineSystemsConstants
    {
        public const string FOLDER_NAME = "EngineSystems/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string DATA_FOLDER_PATH = FOLDER_PATH + "Data/";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string SCENE_NAME = "EngineSystems";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
        public const string DEBUG_SCENE_NAME = "EngineSystemsDebug";
        public const string DEBUG_SCENE_PATH = SCENES_FOLDER_PATH + DEBUG_SCENE_NAME + ".unity";
        public const string SCENE_SET_NAME = "EngineSystemsSceneSet";
        public const string ENGINE_SYSTEMS_PREFAB_NAME = "EngineSystems";
        public const string ENGINE_SYSTEMS_DEBUG_PREFAB_NAME = "EngineSystemsDebug";
    }

    public static class GameSystemsConstants
    {
        public const string FOLDER_NAME = "GameSystems/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string DATA_FOLDER_PATH = FOLDER_PATH + "Data/";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string SCENE_NAME = "GameSystems";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
        public const string DEBUG_SCENE_NAME = "GameSystemsDebug";
        public const string DEBUG_SCENE_PATH = SCENES_FOLDER_PATH + DEBUG_SCENE_NAME + ".unity";
        public const string SCENE_SET_NAME = "GameSystemsSceneSet";
    }

    public struct CelesteConstants
    {
        public const string DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME = "DisableFallbackLoadAssets";
        public const string LOADING_SCENE_NAME = "Loading";
        
        public readonly string LOADING_SCENE_PATH;
        public readonly string CELESTE_GIT_IGNORE_FILE_PATH;
        public readonly string CELESTE_GIT_LFS_FILE_PATH;

        public CelesteConstants(string celesteRootFolder)
        {
            LOADING_SCENE_PATH = $"{celesteRootFolder}Loading/Scenes/Loading.unity";
            CELESTE_GIT_IGNORE_FILE_PATH = $"{celesteRootFolder}UnityProject/Editor/Data/.gitignore.sample";
            CELESTE_GIT_LFS_FILE_PATH = $"{celesteRootFolder}UnityProject/Editor/Data/.gitattributes.sample";
        }
    }

    public struct BuildSystemConstants
    {
        public const string FOLDER_NAME = "BuildSystem/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string EDITOR_FOLDER = FOLDER_PATH + "Editor/";
        public const string SCRIPTING_DEFINE_SYMBOLS_FOLDER = EDITOR_FOLDER + "ScriptingDefineSymbols/";
        public const string COMMON_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "Common/";
        public const string WINDOWS_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "Windows/";
        public const string ANDROID_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "Android/";
        public const string IOS_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "iOS/";
        public const string WEBGL_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "WebGL/";
        
        public readonly string CELESTE_BUILD_SYSTEM_FOLDER;
        public readonly string CELESTE_BUILD_SYSTEM_EDITOR_FOLDER => CELESTE_BUILD_SYSTEM_FOLDER + "Editor/";
        public readonly string CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_FOLDER + "Data/";
        public readonly string CELESTE_COMMON_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Common/";
        public readonly string CELESTE_WINDOWS_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Windows/";
        public readonly string CELESTE_ANDROID_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Android/";
        public readonly string CELESTE_IOS_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "iOS/";
        public readonly string CELESTE_WEBGL_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "WebGL/";

        public BuildSystemConstants(string celesteRootFolder)
        {
            CELESTE_BUILD_SYSTEM_FOLDER = $"{celesteRootFolder}BuildSystem/";
        }
    }
}
