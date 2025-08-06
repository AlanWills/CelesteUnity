using Celeste.Narrative;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Compatibility;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Choices
{
    [InitializeOnLoad]
    public static class ChoicesConstants
    {
        #region Properties and Fields

        public static readonly List<Type> ChoiceOptions = new();
        public static readonly List<string> ChoiceDisplayNames = new();

        #endregion

        static ChoicesConstants()
        {
            Tools.Utils.TypeExtensions.LoadTypes<IChoice>(ChoiceOptions, ChoiceDisplayNames);
        }
    }
}
