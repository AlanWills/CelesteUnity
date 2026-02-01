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
        public const string DOWNLOAD_BOOTSTRAP_ADDRESSABLES_LOAD_JOB_NAME = "DownloadBootstrapAddressables";
        public const string LOAD_BOOTSTRAP_SCENE_SET_LOAD_JOB_NAME = "LoadBootstrapSceneSet";
        public const string LOAD_JOB_NAME = "StartupLoadJob";
        public const string SCENE_NAME = "Startup";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
        public const string SCENE_SET_NAME = "StartupSceneSet";
        public const string NAMESPACE_NAME = "Startup";
    }

    public static class BootstrapConstants
    {
        public const string ADDRESSABLES_GROUP_NAME = "BootstrapAddressables";
        public const string FOLDER_NAME = "Bootstrap/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string LOAD_JOBS_FOLDER_PATH = FOLDER_PATH + "Jobs/";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string DOWNLOAD_COMMON_ADDRESSABLES_LOAD_JOB_NAME = "DownloadCommonAddressables";
        public const string LOAD_ENGINE_SYSTEMS_SCENE_SET_LOAD_JOB_NAME = "LoadEngineSystemsSceneSet";
        public const string LOAD_GAME_SYSTEMS_SCENE_SET_LOAD_JOB_NAME = "LoadGameSystemsSceneSet";
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
        public const string GAME_SYSTEMS_PREFAB_NAME = "GameSystems";
        public const string GAME_SYSTEMS_DEBUG_PREFAB_NAME = "GameSystemsDebug";
    }

    public static class MainMenuConstants
    {
        public const string FOLDER_NAME = "MainMenu/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string LOAD_JOBS_FOLDER_PATH = FOLDER_PATH + "Jobs/";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string SCENE_NAME = "MainMenu";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
        public const string DEBUG_SCENE_NAME = "MainMenuDebug";
        public const string DEBUG_SCENE_PATH = SCENES_FOLDER_PATH + DEBUG_SCENE_NAME + ".unity";
        public const string SCENE_SET_NAME = "MainMenuSceneSet";
        public const string LOAD_MAIN_MENU_SCENE_SET_LOAD_JOB_NAME = "LoadMainMenuSceneSet";
    }

    public class LoadingConstants
    {
        public const string FOLDER_NAME = "Loading/";
        public const string FOLDER_PATH = "Assets/" + FOLDER_NAME;
        public const string SCENE_NAME = "Loading";
        public const string SCENES_FOLDER_PATH = FOLDER_PATH + "Scenes/";
        public const string SCENE_PATH = SCENES_FOLDER_PATH + SCENE_NAME + ".unity";
    }

    public struct CelesteConstants
    {
        public const string COMMON_ADDRESSABLES_GROUP_NAME = "CommonAddressables";
        public const string DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME = "DisableFallbackLoadAssets";
        public const string CUSTOM_PROGUARD_FILE_CONTENTS = "-keep class com.yasirkula.unity.* { *; }";
        public const string MAIN_INPUT_CAMERA_PREFAB_NAME = "MainInputCamera";
        
        public readonly string LOADING_SCENE_PATH;
        public readonly string CELESTE_GIT_IGNORE_FILE_PATH;
        public readonly string CELESTE_GIT_LFS_FILE_PATH;

        public CelesteConstants(string celesteRootFolder)
        {
            UnityEngine.Debug.Assert(celesteRootFolder.EndsWith('/'), $"Celeste Root Folder does not end in the correct path delimiter.  This will cause serious issues with Setup!");
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
        public const string BUILD_PREPARATION_STEPS_FOLDER = EDITOR_FOLDER + "BuildPreparationSteps/";
        public const string BUILD_POST_PROCESS_STEPS_FOLDER = EDITOR_FOLDER + "BuildPostProcessSteps/";
        public const string ASSET_PREPARATION_STEPS_FOLDER = EDITOR_FOLDER + "AssetPreparationSteps/";
        public const string ASSET_POST_PROCESS_STEPS_FOLDER = EDITOR_FOLDER + "AssetPostProcessSteps/";
        public const string SCRIPTING_DEFINE_SYMBOLS_FOLDER = EDITOR_FOLDER + "ScriptingDefineSymbols/";
        public const string COMMON_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "Common/";
        public const string WINDOWS_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "Windows/";
        public const string ANDROID_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "Android/";
        public const string IOS_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "iOS/";
        public const string WEBGL_JENKINS_BUILD_FILES_FOLDER = EDITOR_FOLDER + "WebGL/";
        public const string FASTLANE_BUILD_FILES_FOLDER = "fastlane";

        public readonly string CELESTE_BUILD_SYSTEM_FOLDER;
        public readonly string CELESTE_BUILD_SYSTEM_EDITOR_FOLDER => CELESTE_BUILD_SYSTEM_FOLDER + "Editor/";
        public readonly string CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_FOLDER + "Data/";
        public readonly string CELESTE_COMMON_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Common/";
        public readonly string CELESTE_WINDOWS_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Windows/";
        public readonly string CELESTE_ANDROID_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "Android/";
        public readonly string CELESTE_IOS_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "iOS/";
        public readonly string CELESTE_WEBGL_JENKINS_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "WebGL/";
        public readonly string CELESTE_FASTLANE_BUILD_FILES_FOLDER => CELESTE_BUILD_SYSTEM_EDITOR_DATA_FOLDER + "fastlane/";

        public BuildSystemConstants(string celesteRootFolder)
        {
            UnityEngine.Debug.Assert(celesteRootFolder.EndsWith('/'), $"Celeste Root Folder does not end in the correct path delimiter.  This will cause serious issues with Setup!");
            CELESTE_BUILD_SYSTEM_FOLDER = $"{celesteRootFolder}BuildSystem/";
        }
    }

    public struct ThirdPartyPackageConstants
    {
        public const string NATIVE_FILE_PICKER_PACKAGE = "git@github.com:AlanWills/UnityNativeFilePicker.git";
        public const string NATIVE_SHARE_PACKAGE = "git@github.com:AlanWills/UnityNativeShare.git";
        public const string RUNTIME_INSPECTOR_PACkKAGE = "git@github.com:AlanWills/UnityRuntimeInspector.git";
        public const string LUA_PACKAGE = "git@github.com:AlanWills/Lua-CSharp-Unity.git";
    }
}
