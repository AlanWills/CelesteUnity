using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Celeste.RemoteConfig;
using System;

public class UnityRemoteConfigImpl : IRemoteConfigImpl
{
    public struct userAttributes { }
    public struct appAttributes { }

    #region Properties and Fields

    private Action<string> onDataFetched; 

    #endregion

    public async Task FetchData(string environmentId)
    {
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
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += OnDataFetched;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    #region Callbacks

    private void OnDataFetched(ConfigResponse configResponse)
    {
        string configResponseString = RemoteConfigService.Instance.appConfig.config.ToString();
        Debug.Log($"RemoteConfigService.Instance.appConfig fetched: {configResponseString}");

        onDataFetched?.Invoke(configResponseString);
    }

    #endregion
}