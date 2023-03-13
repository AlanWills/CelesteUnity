namespace CelesteEditor.UnityProject
{
    public static class CreateFeatureClassesConstants
    {
        public const string OBJECT_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "\n\n" +
            "namespace {0}.Objects\n" +
            "{{\n" +
            "    [CreateAssetMenu(fileName = nameof({1}), menuName = \"{2}\")]\n" +
            "    public class {1} : ScriptableObject\n" +
            "    {{\n" +
            "    }}\n" +
            "}}";

        public const string CATALOGUE_SCRIPT_CONTENTS =
           "using UnityEngine;\n" +
           "using Celeste.Objects;\n" +
           "using {0}.Objects;\n" +
           "\n\n" +
           "namespace {0}.Catalogue\n" +
           "{{\n" +
           "    [CreateAssetMenu(fileName = nameof({1}), menuName = \"{2}\")]\n" +
           "    public class {1} : ListScriptableObject<{3}>\n" +
           "    {{\n" +
           "    }}\n" +
           "}}";

        public const string CATALOGUE_EDITOR_SCRIPT_CONTENTS =
           "using UnityEngine;\n" +
           "using UnityEditor;\n" +
           "using CelesteEditor.DataStructures;\n" +
           "using {0}.Objects;" +
           "\n\n" +
           "namespace {0}.Catalogue\n" +
           "{{\n" +
           "    [CustomEditor(typeof({2}))]\n" +
           "    public class {1}Editor : IIndexableItemsEditor<{2}>\n" +
           "    {{\n" +
           "    }}\n" +
           "}}";

        public const string RECORD_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "\n\n" +
            "namespace {0}.Record\n" +
            "{{\n" +
            "    [CreateAssetMenu(fileName = nameof({1}), menuName = \"{2}\")]\n" +
            "    public class {1} : ScriptableObject\n" +
            "    {{\n" +
            "    }}\n" +
            "}}";

        public const string NON_PERSISTENT_MANAGER_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "\n\n" +
            "namespace {0}.Managers\n" +
            "{{\n" +
            "    [AddComponentMenu(\"{2}\")]\n" +
            "    public class {1} : MonoBehaviour\n" +
            "    {{\n" +
            "    }}\n" +
            "}}";

        public const string PERSISTENT_MANAGER_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "using Celeste.Persistence;\n" +
            "using {0}.Persistence;\n" +
            "\n\n" +
            "namespace {0}.Managers\n" +
            "{{\n" +
            "    [AddComponentMenu(\"{2}\")]\n" +
            "    public class {1} : PersistentSceneManager<{1}, {3}>\n" +
            "    {{\n" +
            "\n" +
            "        public const string FILE_NAME = \"{1}.dat\";\n"+
            "        protected override string FileName => FILE_NAME;\n" +
            "\n" +
            "        protected override {3} Serialize()\n" +
            "        {{\n" +
            "            return new {3}();\n" +
            "        }}\n" +
            "\n" +
            "        protected override void Deserialize({3} dto)\n" +
            "        {{\n" +
            "        }}\n" +
            "\n" +
            "        protected override void SetDefaultValues()\n" +
            "        {{\n" +
            "        }}\n" +
            "    }}\n" +
            "}}";

        public const string DTO_SCRIPT_CONTENTS =
            "using System;\n" +
            "\n\n" +
            "namespace {0}.Persistence\n" +
            "{{\n" +
            "    [Serializable]\n" +
            "    public class {1}\n" +
            "    {{\n" +
            "    }}\n" +
            "}}";
    }
}