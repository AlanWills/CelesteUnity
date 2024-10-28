using Celeste.Tools.Attributes.GUI;
using System;
using System.IO;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;

namespace CelesteEditor.Events.Tools
{
    [Serializable]
    public struct CreateEventClassesArgs
    {
        [Tooltip("The path of the directory where the script files will be created relative to the root of the project")] public string directoryPath;
        [Tooltip("The name of the event type that will be created.  Appropriate suffixes like 'Event' will be automatically added")] public string typeName;
        [Tooltip("The full qualified namespace name that the classes will be created in")] public string namespaceName;
        [Tooltip("The name of the type that corresponds to the typed argument of the event")] public string arguments;
        [Tooltip("If true, a normal event class, as well as listener and raiser MonoBehaviours will be generated")] public bool generateEventClasses;
        [Tooltip("If true, a value changed event class, as well as listener and raiser MonoBehaviours will be generated")] public bool generateValueChangedEventClasses;
        [Tooltip("The full 'Create' menu item path for the normal event class")] [ShowIf(nameof(generateEventClasses))] public string eventCreateAssetMenuPath;
        [Tooltip("The full 'Create' menu item path for the value changed event class")] [ShowIf(nameof(generateValueChangedEventClasses))] public string valueChangedEventCreateAssetMenuPath;

        public void SetDefaultValues()
        {
            directoryPath = "Assets/";
            generateEventClasses = true;
            generateValueChangedEventClasses = true;
        }
    }

    public static class CreateEventClasses
    {
        #region Wizard Methods

        public static void Generate(
            CreateEventClassesArgs args,
            string eventClassTemplate,
            string eventListenerClassTemplate,
            string eventRaiserClassTemplate,
            string valueChangedEventClassTemplate,
            string valueChangedEventListenerClassTemplate,
            string valueChangedEventRaiserClassTemplate)
        {
            string namespaceName = args.namespaceName;
            string typeName = args.typeName;
            string arguments = args.arguments;
            string projectRootPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length);
            string parentDirectoryPath = !string.IsNullOrEmpty(args.directoryPath) ? Path.Combine(projectRootPath, args.directoryPath) : projectRootPath;
            Directory.CreateDirectory(parentDirectoryPath);

            string capitalisedTypeName = $"{char.ToUpper(typeName[0])}{typeName.Substring(1)}";

            if (args.generateEventClasses)
            {
                // Event
                {
                    eventClassTemplate = Format(eventClassTemplate, namespaceName, capitalisedTypeName, arguments, args.eventCreateAssetMenuPath);
                    string eventFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}Event.cs");
                    File.WriteAllText(eventFilePath, eventClassTemplate);
                }

                // Event Listener
                {
                    eventListenerClassTemplate = Format(eventListenerClassTemplate, namespaceName, capitalisedTypeName, arguments);
                    string eventListenerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}EventListener.cs");
                    File.WriteAllText(eventListenerFilePath, eventListenerClassTemplate);
                }

                // Event Raiser
                {
                    eventRaiserClassTemplate = Format(eventRaiserClassTemplate, namespaceName, capitalisedTypeName, arguments);
                    string eventRaiserFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}EventRaiser.cs");
                    File.WriteAllText(eventRaiserFilePath, eventRaiserClassTemplate);
                }
            }

            if (args.generateValueChangedEventClasses)
            {
                // Value Changed Event
                {
                    valueChangedEventClassTemplate = Format(valueChangedEventClassTemplate, namespaceName, capitalisedTypeName, arguments, args.valueChangedEventCreateAssetMenuPath);
                    string valueChangedEventFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEvent.cs");
                    File.WriteAllText(valueChangedEventFilePath, valueChangedEventClassTemplate);
                }

                // Value Changed Event Listener
                {
                    valueChangedEventListenerClassTemplate = Format(valueChangedEventListenerClassTemplate, namespaceName, capitalisedTypeName, arguments);
                    string valueChangedEventListenerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEventListener.cs");
                    File.WriteAllText(valueChangedEventListenerFilePath, valueChangedEventListenerClassTemplate);
                }

                // Value Changed Event Raiser
                {
                    valueChangedEventRaiserClassTemplate = Format(valueChangedEventRaiserClassTemplate, namespaceName, capitalisedTypeName, arguments);
                    string valueChangedEventRaiserFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEventRaiser.cs");
                    File.WriteAllText(valueChangedEventRaiserFilePath, valueChangedEventRaiserClassTemplate);
                }
            }
        }

        private static string Format(string inputString, string _namespace, string type, string arguments)
        {
            return inputString
                    .Replace("{NAMESPACE}", _namespace, StringComparison.Ordinal)
                    .Replace("{TYPE}", type, StringComparison.Ordinal)
                    .Replace("{ARGUMENTS}", arguments, StringComparison.Ordinal);
        }

        private static string Format(string inputString, string _namespace, string type, string arguments, string menuPath)
        {
            return Format(inputString, _namespace, type, arguments)
                .Replace("{MENU_PATH}", menuPath, StringComparison.Ordinal);
        }

        #endregion
    }
}
