using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events.Tools
{
    public class CreateEventClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [Tooltip("The path relative to the Assets/ folder of the project.")]
        [SerializeField] private string directoryPath;
        [SerializeField] private string typeName;
        [SerializeField] private string namespaceName;
        [SerializeField] private bool generateEventClasses = true;
        [SerializeField] private bool generateValueChangedEventClasses = true;
        [SerializeField, ShowIf(nameof(generateEventClasses))] private string eventCreateAssetMenuPath;
        [SerializeField, ShowIf(nameof(generateValueChangedEventClasses))] private string valueChangedEventCreateAssetMenuPath;

        private const string EVENT_CLASS_FORMAT = 
"using System;\n" +
"using UnityEngine;\n" +
"using UnityEngine.Events;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\t[Serializable]\n" +
    "\tpublic class {1}UnityEvent : UnityEvent<{2}> {{ }}\n" +
    "\t\n" +
    "\t[Serializable]\n" +
    "\t[CreateAssetMenu(fileName = nameof({1}Event), menuName = \"{3}\")]\n" +
    "\tpublic class {1}Event : ParameterisedEvent<{2}>\n" +
    "\t{{\n" +
    "\t}}\n" +
"}}\n";

        private const string EVENT_LISTENER_CLASS_FORMAT = 
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}EventListener : ParameterisedEventListener<{2}, {1}Event, {1}UnityEvent>\n" +
    "\t{{\n" +
    "\t}}\n" +
"}}\n";

        private const string EVENT_RAISER_CLASS_FORMAT =
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}EventRaiser : ParameterisedEventRaiser<{2}, {1}Event>\n" +
    "\t{{\n" +
    "\t}}\n" +
"}}\n";

        private const string VALUE_CHANGED_EVENT_CLASS_FORMAT =
"using System;\n" + 
"using UnityEngine;\n" +
"using Celeste.Events;\n" + 
"\n" +
"namespace {0} \n" +
"{{\n" +
    "\t[Serializable]\n" +
    "\tpublic class {1}ValueChangedUnityEvent : ValueChangedUnityEvent<{2}> {{ }}\n" +
    "\n" +
    "\t[Serializable]\n" +
    "\t[CreateAssetMenu(fileName = nameof({1}ValueChangedEvent), menuName = \"{3}\")]\n" +
    "\tpublic class {1}ValueChangedEvent : ParameterisedValueChangedEvent<{2}>\n" +
    "\t{{\n" +
    "\t}}\n" +
"}}\n";

        private const string VALUE_CHANGED_EVENT_LISTENER_CLASS_FORMAT =
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}ValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<{2}>, {1}ValueChangedEvent, {1}ValueChangedUnityEvent>\n" +
    "\t{{\n" +
    "\t}}\n" +
"}}\n";

        private const string VALUE_CHANGED_EVENT_RAISER_CLASS_FORMAT = 
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}ValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<{2}>, {1}ValueChangedEvent>\n" +
    "\t{{\n" +
    "\t}}\n" +
"}}\n";

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Create Event Classes")]
        public static void ShowCreateEventClassesWizard()
        {
            DisplayWizard<CreateEventClassesWizard>("Create Event Classes", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                var selectionInProject = Selection.GetFiltered<Object>(SelectionMode.Assets);
                if (selectionInProject != null && selectionInProject.Length == 1)
                {
                    directoryPath = AssetUtility.GetAssetFolderPath(selectionInProject[0]);

                    const string assetsPath = "Assets/";
                    if (directoryPath.StartsWith(assetsPath))
                    {
                        directoryPath = directoryPath.Remove(0, assetsPath.Length);
                    }
                }
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            string assetDatabasePath = Application.dataPath;
            string parentDirectoryPath = !string.IsNullOrEmpty(directoryPath) ? Path.Combine(assetDatabasePath, directoryPath) : assetDatabasePath;
            Directory.CreateDirectory(parentDirectoryPath);

            string capitalisedTypeName = $"{char.ToUpper(typeName[0])}{typeName.Substring(1)}";

            if (generateEventClasses)
            {
                // Event
                {
                    string eventFileContents = string.Format(EVENT_CLASS_FORMAT, namespaceName, typeName, capitalisedTypeName, eventCreateAssetMenuPath);
                    string eventFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}Event.cs");
                    File.WriteAllText(eventFilePath, eventFileContents);
                }

                // Event Listener
                {
                    string eventListenerFileContents = string.Format(EVENT_LISTENER_CLASS_FORMAT, namespaceName, typeName, capitalisedTypeName);
                    string eventListenerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}EventListener.cs");
                    File.WriteAllText(eventListenerFilePath, eventListenerFileContents);
                }

                // Event Raiser
                {
                    string eventRaiserFileContents = string.Format(EVENT_RAISER_CLASS_FORMAT, namespaceName, typeName, capitalisedTypeName);
                    string eventRaiserFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}EventRaiser.cs");
                    File.WriteAllText(eventRaiserFilePath, eventRaiserFileContents);
                }
            }

            if (generateValueChangedEventClasses)
            {
                // Value Changed Event
                {
                    string valueChangedEventFileContents = string.Format(VALUE_CHANGED_EVENT_CLASS_FORMAT, namespaceName, capitalisedTypeName, typeName, valueChangedEventCreateAssetMenuPath);
                    string valueChangedEventFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEvent.cs");
                    File.WriteAllText(valueChangedEventFilePath, valueChangedEventFileContents);
                }

                // Value Changed Event Listener
                {
                    string valueChangedEventListenerFileContents = string.Format(VALUE_CHANGED_EVENT_LISTENER_CLASS_FORMAT, namespaceName, capitalisedTypeName, typeName);
                    string valueChangedEventListenerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEventListener.cs");
                    File.WriteAllText(valueChangedEventListenerFilePath, valueChangedEventListenerFileContents);
                }

                // Value Changed Event Raiser
                {
                    string valueChangedEventRaiserFileContents = string.Format(VALUE_CHANGED_EVENT_RAISER_CLASS_FORMAT, namespaceName, capitalisedTypeName, typeName);
                    string valueChangedEventRaiserFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}ValueChangedEventRaiser.cs");
                    File.WriteAllText(valueChangedEventRaiserFilePath, valueChangedEventRaiserFileContents);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
