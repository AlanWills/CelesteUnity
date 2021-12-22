using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(DownloadAddressablesLoadJob), menuName = "Celeste/Loading/Load Jobs/Download Addressables")]
    public class DownloadAddressablesLoadJob : LoadJob
    {
        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield break;
        }
    }
}