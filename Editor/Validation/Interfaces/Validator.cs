using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CelesteEditor.Validation
{
    public static class Validator<T> where T : UnityEngine.Object
    {
        #region Properties and Fields

        public static int NumValidationConditions
        {
            get { return validationConditions.Count; }
        }

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
                        Debug.LogFormat("Found validation condition type: {0}", t.Name);
                        validationConditions.Add(Activator.CreateInstance(t) as IValidationCondition<T>);
                    }
                }
            }

            stopWatch.Stop();
            Debug.LogFormat("Loaded {0} Validation Conditions in {1} seconds", validationConditions.Count, stopWatch.ElapsedMilliseconds / 1000.0f);
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
