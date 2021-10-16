using Celeste.Debug;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Debug.Menus
{
    [AddComponentMenu("Celeste/Deck Building/Debug/Menus/Hookup Deck Runtime Debug Menu")]
    public class HookupDeckRuntimeDebugMenu : MonoBehaviour
    {
        [SerializeField] private DeckRuntime deckRuntime;
        [SerializeField] private DeckRuntimeDebugMenu debugMenu;

        private void Start()
        {
            debugMenu.Hookup(deckRuntime);
        }
    }
}