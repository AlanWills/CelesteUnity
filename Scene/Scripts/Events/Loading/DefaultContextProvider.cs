using Celeste.Scene.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.Scene
{
    [CreateAssetMenu(fileName = nameof(DefaultContextProvider), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Default Context Provider")]
    public class DefaultContextProvider : ContextProvider
    {
        public override Context Create()
        {
            return new Context();
        }
    }
}