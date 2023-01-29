using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events.Tools
{
    public struct CreateEventClassesArgs
    {
        [Tooltip("The path relative to the Assets/ folder of the project.")]
        public string directoryPath;
        public string typeName;
        public string namespaceName;
        public string arguments;
        public bool generateEventClasses;
        public bool generateValueChangedEventClasses;
        public string eventCreateAssetMenuPath;
        public string valueChangedEventCreateAssetMenuPath;
    }

    public static class CreateEventClasses
    {
        #region Wizard Methods

        public static void Generate(CreateEventClassesArgs args)
        {
            string namespaceName = args.namespaceName;
            string typeName = args.typeName;
            string arguments = args.arguments;
            string assetDatabasePath = Application.dataPath;
            string parentDirectoryPath = !string.IsNullOrEmpty(args.directoryPath) ? Path.Combine(assetDatabasePath, args.directoryPath) : assetDatabasePath;
            Directory.CreateDirectory(parentDirectoryPath);

            string capitalisedTypeName = $"{char.ToUpper(typeName[0])}{typeName.Substring(1)}";

            if (args.generateEventClasses)
            {
                // Event
                {
                    string eventFileContents = string.Format(CreateEventClassesConstants.EVENT_CLASS_FORMAT, namespaceName, typeName, arguments, args.eventCreateAssetMenuPath);
                    string eventFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}Event.cs");
                    File.WriteAllText(eventFilePath, eventFileContents);
                }

                // Event Listener
                {
                    string eventListenerFileContents = string.Format(CreateEventClassesConstants.EVENT_LISTENER_CLASS_FORMAT, namespaceName, typeName, arguments);
                    string eventListenerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}EventListener.cs");
                    File.WriteAllText(eventListenerFilePath, eventListenerFileContents);
                }

                // Event Raiser
                {
                    string eventRaiserFileContents = string.Format(CreateEventClassesConstants.EVENT_RAISER_CLASS_FORMAT, namespaceName, typeName, arguments);
                    string eventRaiserFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}EventRaiser.cs");
                    File.WriteAllText(eventRaiserFilePath, eventRaiserFileContents);
                }
            }

            if (args.generateValueChangedEventClasses)
            {
                // Value Changed Event
                {
                    string valueChangedEventFileContents = string.Format(CreateEventClassesConstants.VALUE_CHANGED_EVENT_CLASS_FORMAT, namespaceName, capitalisedTypeName, arguments, args.valueChangedEventCreateAssetMenuPath);
                    string valueChangedEventFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEvent.cs");
                    File.WriteAllText(valueChangedEventFilePath, valueChangedEventFileContents);
                }

                // Value Changed Event Listener
                {
                    string valueChangedEventListenerFileContents = string.Format(CreateEventClassesConstants.VALUE_CHANGED_EVENT_LISTENER_CLASS_FORMAT, namespaceName, capitalisedTypeName, arguments);
                    string valueChangedEventListenerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEventListener.cs");
                    File.WriteAllText(valueChangedEventListenerFilePath, valueChangedEventListenerFileContents);
                }

                // Value Changed Event Raiser
                {
                    string valueChangedEventRaiserFileContents = string.Format(CreateEventClassesConstants.VALUE_CHANGED_EVENT_RAISER_CLASS_FORMAT, namespaceName, capitalisedTypeName, arguments);
                    string valueChangedEventRaiserFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEventRaiser.cs");
                    File.WriteAllText(valueChangedEventRaiserFilePath, valueChangedEventRaiserFileContents);
                }
            }
        }

        #endregion
    }
}
