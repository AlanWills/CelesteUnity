using Celeste;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Validation.GUIs
{
    public class ValidatorGUI
    {
        #region Properties and Fields

        private List<ValidationConditionGUI> validationConditionGUIs = new List<ValidationConditionGUI>();

        #endregion

        public ValidatorGUI()
        {
            for (int i = 0; i < Validator.NumValidationConditions; ++i)
            {
                validationConditionGUIs.Add(new ValidationConditionGUI(Validator.GetValidationCondition(i)));
            }
        }

        #region GUI

        public void GUI()
        {
            EditorGUILayout.LabelField("Validation", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Check All", GUILayout.ExpandWidth(false)))
                {
                    LogExtensions.Clear();

                    foreach (ValidationConditionGUI gui in validationConditionGUIs)
                    {
                        gui.Check();
                    }
                }

                if (GUILayout.Button("Fix All", GUILayout.ExpandWidth(false)))
                {
                    LogExtensions.Clear();

                    foreach (ValidationConditionGUI gui in validationConditionGUIs)
                    {
                        gui.Fix();
                    }
                }

                if (GUILayout.Button("Reset All", GUILayout.ExpandWidth(false)))
                {
                    foreach (ValidationConditionGUI gui in validationConditionGUIs)
                    {
                        gui.Reset();
                    }
                }

                if (GUILayout.Button("Clear Log", GUILayout.ExpandWidth(false)))
                {
                    LogExtensions.Clear();
                }
            }

            EditorGUILayout.Separator();

            foreach (ValidationConditionGUI gui in validationConditionGUIs)
            {
                gui.GUI();
            }

            CelesteEditorGUILayout.HorizontalLine();
        }

        #endregion
    }

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
            EditorGUILayout.LabelField("Validation", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Check All", GUILayout.ExpandWidth(false)))
                {
                    LogExtensions.Clear();

                    foreach (ValidationConditionGUI<T> gui in validationConditionGUIs)
                    {
                        gui.Check(asset);
                    }
                }

                if (GUILayout.Button("Fix All", GUILayout.ExpandWidth(false)))
                {
                    LogExtensions.Clear();

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
                    LogExtensions.Clear();
                }
            }

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
