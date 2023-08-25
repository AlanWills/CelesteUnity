using Celeste.Events;
using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.Sound
{
    public interface IMusicListener
    {
        public void OnMusicEnabledChanged(ValueChangedArgs<bool> args);

        public void Play(AudioClip audioClip);
        public void Play(AudioClipSettings audioClipSettings);

        public void PlayOneShot(AudioClip audioClip);
        public void PlayOneShot(AudioClipSettings audioClipSettings);
    }
}
