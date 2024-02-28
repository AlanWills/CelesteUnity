using Celeste.Debug.Commands;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Celeste.Application
{
    [CreateAssetMenu(
        menuName = CelesteMenuItemConstants.APPLICATION_MENU_ITEM + "Debug/Console Application",
        order = CelesteMenuItemConstants.APPLICATION_MENU_ITEM_PRIORITY)]
    public class ConsoleApplication : DebugCommand
    {
        public override bool Execute(List<string> parameters, StringBuilder output)
        {
            output.AppendLine("Is Mobile: " + ApplicationInfo.IsMobile);
            output.AppendLine("PDP: " + UnityEngine.Application.persistentDataPath);
            return true;
        }
    }
}
