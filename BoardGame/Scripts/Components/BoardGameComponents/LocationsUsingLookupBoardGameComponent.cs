using Celeste.BoardGame.Interfaces;
using Celeste.Scene.Hierarchy;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Locations Using Lookup")]
    [CreateAssetMenu(
        fileName = nameof(LocationsUsingLookupBoardGameComponent), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Board Game Components/Locations Using Lookup",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class LocationsUsingLookupBoardGameComponent : BoardGameComponent, IBoardGameLocations
    {
        #region Properties and Fields

        [SerializeField] private StringGameObjectDictionary locations;

        #endregion

        public Transform FindLocation(string name)
        {
#if NULL_CHECKS
            if (string.IsNullOrEmpty(name))
            {
                UnityEngine.Debug.LogAssertion($"Null or empty name passed into {nameof(FindLocation)}.");
                return null;
            }
#endif

            GameObject location = locations.GetItem(name);
            return location != null ? location.transform : null;
        }
    }
}
