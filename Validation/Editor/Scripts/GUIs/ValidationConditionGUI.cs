using System.Text;
using UnityEditor;
using UnityEngine;
using Celeste;

namespace CelesteEditor.Validation.GUIs
{
    public class ValidationConditionGUI
    {
        private enum ConditionStatus
        {
            NotRun,
            Passed,
            Failed
        }

        #region Properties and Fields

        private IValidationCondition validationCondition;
        private ConditionStatus conditionStatus = ConditionStatus.NotRun;
        private StringBuilder output = new StringBuilder(512);

        #endregion

        public ValidationConditionGUI(IValidationCondition validationCondition)
        {
            this.validationCondition = validationCondition;
        }

        #region Validation Methods

        public void Reset()
        {
            conditionStatus = ConditionStatus.NotRun;
        }

        public void Check()
        {
            output.Clear();
            conditionStatus = validationCondition.Validate(output) ? ConditionStatus.Passed : ConditionStatus.Failed;

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

        public bool CanFix()
        {
            return conditionStatus == ConditionStatus.Failed &&
                   validationCondition is IFixableCondition &&
                   (validationCondition as IFixableCondition).CanFix();
        }

        public void Fix()
        {
            if (CanFix())
            {
                output.Clear();
                (validationCondition as IFixableCondition).Fix(output);
                Check();

                if (output.Length > 0)
                {
                    Debug.Log(output.ToString());
                }
            }
        }

        #endregion

        #region GUI

        public void GUI()
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle guiStyle = UnityEngine.GUI.skin.label;
            if (conditionStatus == ConditionStatus.Passed)
            {
                guiStyle = CelesteGUIStyles.SuccessLabel;
            }
            else if (conditionStatus == ConditionStatus.Failed)
            {
                guiStyle = CelesteGUIStyles.ErrorLabel;
            }

            EditorGUILayout.LabelField(validationCondition.DisplayName, guiStyle);

            if (GUILayout.Button("Check", GUILayout.ExpandWidth(false)))
            {
                Check();
            }

            if (CanFix() && GUILayout.Button("Fix", GUILayout.ExpandWidth(false)))
            {
                Fix();
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }

    public class ValidationConditionGUI<T> where T : Object
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
                guiStyle = CelesteGUIStyles.SuccessLabel;
            }
            else if (conditionStatus == ConditionStatus.Failed)
            {
                guiStyle = CelesteGUIStyles.ErrorLabel;
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
