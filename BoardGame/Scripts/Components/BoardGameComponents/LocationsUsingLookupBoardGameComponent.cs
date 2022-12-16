using Celeste.BoardGame.Interfaces;
using Celeste.Scene.Hierarchy;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Locations Using Lookup")]
    [CreateAssetMenu(fileName = nameof(LocationsUsingLookupBoardGameComponent), menuName = "Celeste/Board Game/Board Game Components/Locations Using Lookup")]
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
