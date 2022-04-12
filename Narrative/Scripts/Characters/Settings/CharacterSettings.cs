using Celeste.Assets;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.Characters.Settings
{
    public abstract class CharacterSettings : ScriptableObject, IHasAssets
    {
        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public abstract T FindCustomisationByGuid<T>(int guid) where T : CharacterCustomisation;
    }
}
