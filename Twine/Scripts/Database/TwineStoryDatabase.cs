using Celeste.Objects;
using UnityEngine;

namespace Celeste.Twine
{
    [CreateAssetMenu(fileName = nameof(TwineStoryDatabase), menuName = "Celeste/Twine/Twine Story Database")]
    public class TwineStoryDatabase : ListScriptableObject<TwineStory>
    {
    }
}