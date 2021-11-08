using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(BackgroundCatalogue), menuName = "Celeste/Narrative/Backgrounds/Background Catalogue")]
    public class BackgroundCatalogue : ArrayScriptableObject<Background>
    {
        public Background FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}