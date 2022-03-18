using Celeste.Events;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.Twine.Loading
{
    [AddComponentMenu("Celeste/Twine/Loading/On Twine Context Loaded")]
    public class OnTwineContextLoaded : MonoBehaviour
    {
        [SerializeField] private TwineRecord twineRecord;

        public void OnContextLoaded(OnContextLoadedArgs onContextLoadedArgs)
        {
            TwineContext twineContext = onContextLoadedArgs.context as TwineContext;
            TwineStory twineStory = twineContext.TwineStory;

            twineRecord.ImportTwineStory(twineStory);
            twineRecord.LoadTwineStory(twineStory.name);
        }
    }
}
