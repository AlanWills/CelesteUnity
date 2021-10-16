using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Validation.GUIs
{
    public class ValidationConditionGUI<T> where T : UnityEngine.Object
    {
        private enum ConditionStatus
        { 
            NotRun,
            Passed,
            Failed
        }

        #region Properties and Fields

        private IValidationCondition<T> validationCondition;
        private ConditionStatus conditionStatus = ConditionStatus.NotRun;
        private StringBuilder output = new StringBuilder(512);

        #endregion

        public ValidationConditionGUI(IValidationCondition<T> validationCondition)
        {
            this.validationCondition = validationCondition;
        }

        #region Validation Methods

        public void Reset()
        {
            conditionStatus = ConditionStatus.NotRun;
        }

        public void Check(T asset)
        {
            output.Clear();
            conditionStatus = validationCondition.Validate(asset, output) ? ConditionStatus.Passed : ConditionStatus.Failed;

            if (output.Length > 0)
            {
                if (conditionStatus == ConditionStatus.Failed)
                {
                    Debug.LogAssertion(output.ToString());
                }
                else
                {
                    Debug.Log(conditionStatus.ToString());
                }
            }
        }

        public bool CanFix(T asset)
        {
            return conditionStatus == ConditionStatus.Failed && 
                   validationCondition is IFixableCondition<T> && 
                   (validationCondition as IFixableCondition<T>).CanFix(asset);
        }

        public void Fix(T asset)
        {
            if (CanFix(asset))
            {
                output.Clear();
                (validationCondition as IFixableCondition<T>).Fix(asset, output);
                Check(asset);

                if (output.Length > 0)
                {
                    Debug.Log(output.ToString());
                }
            }
        }

        #endregion

        #region GUI

        public void GUI(T asset)
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle guiStyle = UnityEngine.GUI.skin.label;
            if (conditionStatus == ConditionStatus.Passed)
            {
                guiStyle = CelesteEditorStyles.SuccessLabel;
            }
            else if (conditionStatus == ConditionStatus.Failed)
            {
                guiStyle = CelesteEditorStyles.ErrorLabel;
            }

            EditorGUILayout.LabelField(validationCondition.DisplayName, guiStyle);

            if (GUILayout.Button("Check", GUILayout.ExpandWidth(false)))
            {
                Check(asset);
            }

            if (CanFix(asset) && GUILayout.Button("Fix", GUILayout.ExpandWidth(false)))
            {
                Fix(asset);
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
