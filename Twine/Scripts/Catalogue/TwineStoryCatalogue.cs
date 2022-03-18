using Celeste.Objects;
using UnityEngine;

namespace Celeste.Twine
{
    [CreateAssetMenu(fileName = nameof(TwineStoryCatalogue), menuName = "Celeste/Twine/Twine Story Catalogue")]
    public class TwineStoryCatalogue : ListScriptableObject<TwineStory>
    {
    }
}