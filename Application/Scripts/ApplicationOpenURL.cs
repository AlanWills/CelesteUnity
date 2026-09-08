using UnityEngine;

namespace Celeste.Application
{
    public class ApplicationOpenURL : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            UnityEngine.Application.OpenURL(url);
        }
    }
}