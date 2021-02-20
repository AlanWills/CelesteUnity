using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Validation.GUIs
{
    public class ValidatorGUI<T> where T : UnityEngine.Object
    {
        #region Properties and Fields

        private List<ValidationConditionGUI<T>> validationConditionGUIs = new List<ValidationConditionGUI<T>>();

        #endregion

        public ValidatorGUI()
        {
            for (int i = 0; i < Validator<T>.NumValidationConditions; ++i)
            {
                validationConditionGUIs.Add(new ValidationConditionGUI<T>(Validator<T>.GetValidationCondition(i)));
            }
        }

        #region GUI

        public void GUI(T asset)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Validation", CelesteEditorStyles.BoldLabel);

            if (GUILayout.Button("Check All", GUILayout.ExpandWidth(false)))
            {
                LogUtility.Clear();

                foreach (ValidationConditionGUI<T> gui in validationConditionGUIs)
                {
                    gui.Check(asset);
                }
            }

            if (GUILayout.Button("Fix All", GUILayout.ExpandWidth(false)))
            {
                LogUtility.Clear();

                foreach (ValidationConditionGUI<T> gui in validationConditionGUIs)
                {
                    gui.Fix(asset);
                }
            }

            if (GUILayout.Button("Reset All", GUILayout.ExpandWidth(false)))
            {
                foreach (ValidationConditionGUI<T> gui in validationConditionGUIs)
                {
                    gui.Reset();
                }
            }

            if (GUILayout.Button("Clear Log", GUILayout.ExpandWidth(false)))
            {
                LogUtility.Clear();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();

            foreach (ValidationConditionGUI<T> gui in validationConditionGUIs)
            {
                gui.GUI(asset);
            }

            CelesteEditorGUILayout.HorizontalLine();
        }

        #endregion
    }
}
