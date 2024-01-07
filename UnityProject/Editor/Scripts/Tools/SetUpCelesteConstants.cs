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
        public const string LOAD_JOBS_FOLDER_PATH = FOLDER_PATH + "Steps/";
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
        public const string SCENE_SET_NAME = "EngineSystemsSceneSet";
        public const string ENGINE_SYSTEMS_PREFAB_NAME = "EngineSystems";
    }

    public static class CelesteConstants
    {
        public const string CELESTE_ROOT_FOLDER = "Assets/Celeste/";
        public const string DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME = "DisableFallbackLoadAssets";
        public const string LOADING_SCENE_NAME = "Loading";
        public const string LOADING_SCENE_PATH = CELESTE_ROOT_FOLDER + "Loading/Scenes/Loading.unity";
        public const string CELESTE_GIT_IGNORE_FILE_PATH = CELESTE_ROOT_FOLDER + "UnityProject/Editor/Data/.gitignore.sample";
        public const string CELESTE_GIT_LFS_FILE_PATH = CELESTE_ROOT_FOLDER + "UnityProject/Editor/Data/.gitattributes.sample";
    }

    public static class BuildSystemConstants
    {
        public const string FOLDER_NAME = "BuildSystem/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string DATA_FOLDER_PATH = FOLDER_PATH + "Edior/Data/";
        public const string COMMON_JENKINS_BUILD_FILES_FOLDER_PATH = FOLDER_PATH + "Editor/Data/";
        public const string CELESTE_BUILD_SYSTEM_FOLDER = CelesteConstants.CELESTE_ROOT_FOLDER + "BuildSystem/";
        public const string CELESTE_BUILD_SYSTEM_EDITOR_FOLDER = CELESTE_BUILD_SYSTEM_FOLDER + "Editor/";
        public const string CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER = CELESTE_BUILD_SYSTEM_EDITOR_FOLDER + "Data/";
        public const string CELESTE_COMMON_JENKINS_BUILD_FILES_FOLDER = CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Common/";
        public const string CELESTE_WINDOWS_JENKINS_BUILD_FILES_FOLDER = CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Windows/";
        public const string CELESTE_ANDROID_JENKINS_BUILD_FILES_FOLDER = CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Android/";
        public const string CELESTE_IOS_JENKINS_BUILD_FILES_FOLDER = CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "iOS/";
        public const string CELESTE_WEBGL_JENKINS_BUILD_FILES_FOLDER = CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "WebGL/";
    }
}
