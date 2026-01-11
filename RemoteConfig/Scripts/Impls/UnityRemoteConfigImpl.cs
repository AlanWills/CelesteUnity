#if UNITY_REMOTE_CONFIG
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Celeste.RemoteConfig;
using System;
using Celeste;
using Celeste.RemoteConfig.Objects;

public class UnityRemoteConfigImpl : IRemoteConfigImpl
{
    public struct userAttributes { }
    public struct appAttributes { }

    #region Properties and Fields

    private bool isDataFetched;
    private Action<string> onDataFetched; 

    #endregion

    public async Task FetchData(RemoteConfigEnvironmentIds environmentIds, bool isDebugBuild)
    {
        string environmentId = environmentIds.GetEnvironmentId(DataSource.Unity, isDebugBuild);
        RemoteConfigService.Instance.SetEnvironmentID(environmentId);
        await FetchRemoteConfig();
    }

    public void AddOnDataFetchedCallback(Action<string> dataCallback)
    {
        onDataFetched += dataCallback;
    }

    public void RemoveOnDataFetchedCallback(Action<string> dataCallback)
    {
        onDataFetched -= dataCallback;
    }

    private async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private async Task FetchRemoteConfig()
    {
        isDataFetched = false;

        // UPDATE: This had bugs and Unity told me I didn't need it anyway
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        //if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }
        
        RuntimeConfig runtimeConfig = await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        string configResponseString = runtimeConfig.config.ToString();
        Debug.Log($"RemoteConfigService.Instance.appConfig fetched: {configResponseString}", CelesteLog.RemoteConfig);

        isDataFetched = true;
        onDataFetched?.Invoke(configResponseString);
    }
}
#endif