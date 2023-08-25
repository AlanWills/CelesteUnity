using Celeste.Tools.Attributes.GUI;
using System;
using System.IO;
using UnityEngine;

namespace CelesteEditor.Events.Tools
{
    [Serializable]
    public struct CreateEventClassesArgs
    {
        [Tooltip("The path of the directory where thescript files will be created")] public string directoryPath;
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
