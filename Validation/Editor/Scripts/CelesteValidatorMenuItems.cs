using System.Diagnostics;
using UnityEditor;

namespace CelesteEditor.Validation
{
    public static class CelesteValidatorMenuItems
    {
        private const string EXECUTE_VALIDATE_ASSETS_MENU_PATH = "Celeste/Assets/Validate";

        [MenuItem(EXECUTE_VALIDATE_ASSETS_MENU_PATH)]
        public static void ExecuteValidateAssets()
        {
            Validator.FindValidationConditions();
            
            if (Validator.Validate())
            {
                UnityEngine.Debug.Log("All Validation Conditions Passed!");
            }
        }
    }
}
