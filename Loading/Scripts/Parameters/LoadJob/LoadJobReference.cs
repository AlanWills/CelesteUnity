using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Loading.Parameters
{
    [CreateAssetMenu(fileName = nameof(LoadJobReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Loading/Load Job Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LoadJobReference : ParameterReference<LoadJob, LoadJobValue, LoadJobReference>
    {
    }
}
