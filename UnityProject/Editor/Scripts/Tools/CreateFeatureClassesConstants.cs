using Celeste.Persistence;
using CelesteEditor.Persistence;
using UnityEditor;

namespace CelesteEditor.UnityProject
{
    public static class CreateFeatureClassesConstants
    {
        public const string OBJECT_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "\n" +
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
           "\n" +
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
           "using {3}.Objects;\n" +
           "using {3}.Catalogue;\n" +
           "\n" +
           "namespace {0}.Catalogue\n" +
           "{{\n" +
           "    [CustomEditor(typeof({1}))]\n" +
           "    public class {1}Editor : IIndexableItemsEditor<{2}>\n" +
           "    {{\n" +
           "    }}\n" +
           "}}";

        public const string RECORD_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "\n" +
            "namespace {0}.Record\n" +
            "{{\n" +
            "    [CreateAssetMenu(fileName = nameof({1}), menuName = \"{2}\")]\n" +
            "    public class {1} : ScriptableObject\n" +
            "    {{\n" +
            "    }}\n" +
            "}}";

        public const string NON_PERSISTENT_MANAGER_SCRIPT_CONTENTS =
            "using UnityEngine;\n" +
            "\n" +
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
            "\n" +
            "namespace {0}.Managers\n" +
            "{{\n" +
            "    [AddComponentMenu(\"{2}\")]\n" +
            "    public class {1} : PersistentSceneManager<{1}, {3}>\n" +
            "    {{\n" +
            "        public const string FILE_NAME = \"{1}.dat\";\n" +
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
            "\n" +
            "namespace {0}.Persistence\n" +
            "{{\n" +
            "    [Serializable]\n" +
            "    public class {1}\n" +
            "    {{\n" +
            "    }}\n" +
            "}}";

        public const string PERSISTENCE_MENU_ITEMS_SCRIPT_CONTENTS =
            "using UnityEditor;\n" +
            "using Celeste.Persistence;\n" +
            "using CelesteEditor.Persistence;\n" +
            "\n" +
            "namespace {0}.Persistence\n" +
            "{{\n" +
            "    public static class {1}PersistenceMenuItems\n" +
            "    {{\n" +
            "        [MenuItem(\"{2}\", priority = 0)]\n" +
            "        public static void Open{1}SaveMenuItem()\n" +
            "        {{\n" +
            "            PersistenceMenuItemUtility.OpenExplorerAtPersistentData();\n" +
            "        }}\n" +
            "\n" +
            "        [MenuItem(\"{3}\", priority = 100)]\n" +
            "        public static void Delete{1}SaveMenuItem()\n" +
            "        {{\n" +
            "            PersistenceUtility.DeletePersistentDataFile({4}.FILE_NAME);\n" +
            "        }}\n" +
            "    }}\n" +
            "}}\n";
    }
}