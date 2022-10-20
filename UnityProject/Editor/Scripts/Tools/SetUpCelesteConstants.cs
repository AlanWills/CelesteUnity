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
        public const string LOAD_ENGINE_SYSTEMS_SCENE_SET_LOAD_JOB_NAME = "LoadEngineSystemSceneSet";
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
        public const string DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME = "DisableFallbackLoadAssets";
        public const string LOADING_SCENE_NAME = "Loading";
        public const string CELESTE_GIT_IGNORE_FILE_PATH = "Assets/Celeste/UnityProject/Editor/Data/.gitignore.sample";
        public const string CELESTE_GIT_LFS_FILE_PATH = "Assets/Celeste/UnityProject/Editor/Data/.gitattributes.sample";
    }
}
