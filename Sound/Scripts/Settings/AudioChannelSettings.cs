using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(AudioChannelSettings), menuName = "Celeste/Sound/Audio Channel Settings")]
    public class AudioChannelSettings : ScriptableObject
    {
        #region Properties and Fields

        public float Volume => volume.Value;

        [SerializeField] private FloatValue volume;

        #endregion
    }
}
