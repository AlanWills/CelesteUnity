using Celeste.Debug.Menus;
using Celeste.Logic.Catalogue;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.Logic.Debug
{
    [CreateAssetMenu(fileName = nameof(ConditionsDebugMenu), menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Debug/Conditions Debug Menu", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    public class ConditionsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private ConditionCatalogue conditionCatalogue;
        [SerializeField] private int conditionsPerPage = 20;

        [NonSerialized] private int currentPage = 0;

        #endregion

        protected override void OnDrawMenu()
        {
            GUIUtils.ReadOnlyPaginatedList(
                currentPage,
                conditionsPerPage,
                conditionCatalogue.NumItems,
                (index) =>
                {
                    Condition condition = conditionCatalogue.GetItem(index);
                    if (condition != null)
                    {
                        condition.IsMet = GUILayout.Toggle(condition.IsMet, condition.name);
                    }
                });
        }
    }
}
