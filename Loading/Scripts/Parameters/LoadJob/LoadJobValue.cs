using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Loading.Parameters
{
    [CreateAssetMenu(fileName = nameof(LoadJobValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Loading/Load Job Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LoadJobValue : ParameterValue<LoadJob, LoadJobValueChangedEvent>
    {
    }
}
