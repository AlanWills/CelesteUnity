namespace CelesteEditor.Parameters
{
    public static class CreateParameterClassesConstants
    {
        public const string VALUE_PARAMETER_CLASS_FORMAT =
            "using Celeste.Events;\n" +
            "using Celeste.Parameters.Constraints;\n" +
            "using System.Collections.Generic;\n" +
            "using UnityEngine;\n" +
            "\n" +
            "{0}\n" +
            "    [CreateAssetMenu(fileName = nameof({1}Value), menuName = \"{3}\")]\n" +
            "    public class {1}Value : ParameterValue<{2}, {1}ValueChangedEvent>\n" +
            "    {\n" +
            "    }\n" +
            "}\n";

        public const string REFERENCE_PARAMETER_CLASS_FORMAT =
            "using UnityEngine;\n" +
            "\n" +
            "{0}\n" +
            "    [CreateAssetMenu(fileName = nameof({1}Reference), menuName = \"{3}\")]\n" +
            "    public class {1}Reference : ParameterReference<{2}, {1}Value, {1}Reference>\n" +
            "    {\n" +
            "    }\n" +
            "}\n";
    }
}
