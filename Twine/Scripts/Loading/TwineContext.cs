using Celeste.Scene.Events;

namespace Celeste.Twine.Loading
{
    public class TwineContext : Context
    {
        public TwineStory TwineStory { get; }

        public TwineContext(TwineStory twineStory)
        {
            TwineStory = twineStory;
        }
    }
}
