using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CelesteEditor.Validation
{
    public static class Validator
    {
        #region Properties and Fields

        public static int NumValidationConditions => validationConditions.Count;

        private static List<IValidationCondition> validationConditions = new List<IValidationCondition>();

        #endregion

        static Validator()
        {
            FindValidationConditions();
        }

        #region Validation Methods

        public static void FindValidationConditions()
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            validationConditions.Clear();
            Type validationCondition = typeof(IValidationCondition);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (validationCondition.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        Debug.Log($"Found validation condition type: {t.Name}");
                        validationConditions.Add(Activator.CreateInstance(t) as IValidationCondition);
                    }
                }
            }

            stopWatch.Stop();
            Debug.Log($"Loaded {validationConditions.Count} Validation Conditions in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds.");
        }

        public static IValidationCondition GetValidationCondition(int validationCondition)
        {
            return 0 <= validationCondition && validationCondition < NumValidationConditions ? validationConditions[validationCondition] : null;
        }

        public static bool Validate()
        {
            bool passesValidation = true;
            StringBuilder error = new StringBuilder(1024);

            foreach (IValidationCondition validationCondition in validationConditions)
            {
                error.Clear();
                passesValidation &= validationCondition.Validate(error);

                if (error.Length > 0)
                {
                    Debug.LogAssertion(error.ToString());
                }
            }

            return passesValidation;
        }

        #endregion
    }

    public static class Validator<T> where T : UnityEngine.Object
    {
        #region Properties and Fields

        public static int NumValidationConditions => validationConditions.Count;

        private static List<IValidationCondition<T>> validationConditions = new List<IValidationCondition<T>>();

        #endregion

        static Validator()
        {
            FindValidationConditions();
        }

        #region Validation Methods

        public static void FindValidationConditions()
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            validationConditions.Clear();
            Type validationCondition = typeof(IValidationCondition<T>);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (validationCondition.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        Debug.Log($"Found validation condition type: {t.Name}");
                        validationConditions.Add(Activator.CreateInstance(t) as IValidationCondition<T>);
                    }
                }
            }

            stopWatch.Stop();
            Debug.Log($"Loaded {validationConditions.Count} Validation Conditions in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds");
        }

        public static IValidationCondition<T> GetValidationCondition(int validationCondition)
        {
            return 0 <= validationCondition && validationCondition < NumValidationConditions ? validationConditions[validationCondition] : null;
        }

        public static bool Validate(T asset)
        {
            bool passesValidation = true;
            StringBuilder error = new StringBuilder(1024);

            foreach (IValidationCondition<T> validationCondition in validationConditions)
            {
                error.Clear();
                passesValidation &= validationCondition.Validate(asset, error);

                if (error.Length > 0)
                {
                    Debug.LogAssertion(error.ToString());
                }
            }

            return passesValidation;
        }

        #endregion
    }
}
