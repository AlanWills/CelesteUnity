namespace CelesteEditor.Events.Tools
{
    public static class CreateEventClassesConstants
    {
        public const string EVENT_CLASS_FORMAT = 
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
    "\tpublic class {1}Event : ParameterisedEvent<{2}> {{ }}\n" +
    "\t\n" +
    "\t[Serializable]\n" +
    "\tpublic class Guaranteed{1}Event : GuaranteedParameterisedEvent<{1}Event, {2}> {{ }}\n" +
"}}\n";

        public const string EVENT_LISTENER_CLASS_FORMAT = 
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}EventListener : ParameterisedEventListener<{2}, {1}Event, {1}UnityEvent> {{ }}\n" +
"}}\n";

        public const string EVENT_RAISER_CLASS_FORMAT =
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}EventRaiser : ParameterisedEventRaiser<{2}, {1}Event> {{ }}\n" +
"}}\n";

        public const string VALUE_CHANGED_EVENT_CLASS_FORMAT =
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
    "\tpublic class {1}ValueChangedEvent : ParameterisedValueChangedEvent<{2}> {{ }}\n" +
    "\t\n" +
    "\t[Serializable]\n" +
    "\tpublic class Guaranteed{1}ValueChangedEvent : GuaranteedParameterisedValueChangedEvent<{1}ValueChangedEvent, {2}> {{ }}\n" +
"}}\n";

        public const string VALUE_CHANGED_EVENT_LISTENER_CLASS_FORMAT =
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}ValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<{2}>, {1}ValueChangedEvent, {1}ValueChangedUnityEvent> {{ }}\n" +
"}}\n";

        public const string VALUE_CHANGED_EVENT_RAISER_CLASS_FORMAT = 
"using UnityEngine;\n" +
"using Celeste.Events;\n" +
"\n" +
"namespace {0}\n" +
"{{\n" +
    "\tpublic class {1}ValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<{2}>, {1}ValueChangedEvent> {{ }}\n" +
"}}\n";
    }
}
