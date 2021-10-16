using Celeste.Objects;
using System;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(StoryCatalogue), menuName = "Celeste/Narrative/Production/Story Catalogue")]
    public class StoryCatalogue : ArrayScriptableObject<Story>
    {
        public Story FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}