namespace CelesteEditor.UI.Tools
{
    public static class CreateRecyclableScrollRectClassesConstants
    {
        public const string CONTROLLER_CLASS_FORMAT = "using System.Collections.Generic;\n" +
                                                      "using {0}.Objects;\n" +
                                                      "using Celeste.Tools;\n" +
                                                      "using PolyAndCode.UI;\n" +
                                                      "using UnityEngine;\n" +
                                                      "using {0}.Record;\n" +
                                                      "\n" +
                                                      "namespace {0}.UI\n" +
                                                      "{{\n" +
        "\tpublic class {1}sUIController : MonoBehaviour, IRecyclableScrollRectDataSource\n" +
        "\t{{\n" +
            "\t\t#region Properties and Fields\n" +
            "\n" +
            "\t\t[SerializeField] private RecyclableScrollRect scrollRect;\n" +
            "\t\t[SerializeField] private {1}Record {2}Record;\n" +
"\n" +
            "\t\t[NonSerialized] private List<{1}UIData> {2}CellData = new List<{1}UIData>();\n" +
"\n" +
            "\t\t#endregion\n" +
"\n" +
            "\t\t#region Unity Methods\n" +
"\n" +
            "\t\tprivate void OnValidate()\n" +
            "\t\t{{\n" +
                "\t\t\tthis.TryGetInChildren(ref scrollRect);\n" +
            "\t\t}}\n" +
"\n" +
            "\t\tprivate void Start()\n" +
            "\t\t{{\n" +
                "\t\t\tSetUpUI();\n" +
            "\t\t}}\n" +
"\n" +
            "\t\t#endregion\n" +
"\n" +
            "\t\tprivate void SetUpUI()\n" +
            "\t\t{{\n" +
                "\t\t\t{2}CellData.Clear();\n" +
                "\t\t\t\n" +
                "\t\t\tfor (int i = 0, n = {2}Record.Num{1}s; i < n; ++i)\n" +
                "\t\t\t{{\n" +
                    "\t\t\t\t{1} {2} = {2}Record.Get{1}(i);\n" +
                    "\t\t\t\t{2}CellData.Add(new {1}UIData({2}));\n" +
                "\t\t\t}}\n" +
    "\n" +
                "\t\t\tscrollRect.Initialize(this);\n" +
            "\t\t}}\n" +
"\n" +
            "\t\t#region IRecyclableScrollRectDataSource\n" +
                                                      "\n" +
        "\t\tpublic int GetItemCount()\n" +
            "\t\t{{\n" +
                "\t\t\treturn {2}CellData.Count;\n" +
            "\t\t}}\n" +
"\n" +
            "\t\tpublic void SetCell(ICell cell, int index)\n" +
            "\t\t{{\n" +
                "\t\t\t(cell as {1}UI).Hookup({2}CellData[index]);\n" +
            "\t\t}}\n" +
"\n" +
            "\t\t#endregion\n" +
        "\t}}\n" +
    "}}\n";

        public const string UI_DATA_CLASS_FORMAT = "using {0}.Objects;\n" +
                                                   "\n" +
                                                   "namespace {0}.UI\n" +
                                                   "{{\n" +
        "\tpublic class {1}UIData\n" +
        "\t{{\n" +
            "\t\t#region Properties and Fields\n" +
        "\n" +
            "\t\tpublic {1} {1} {{ get; }}\n" +
        "\n" +
            "\t\t#endregion\n" +
"\n" +
            "\t\tpublic {1}UIData({1} {2})\n" +
            "\t\t{{\n" +
                "\t\t\t{1} = {2};\n" +
            "\t\t}}\n" +
        "\t}}\n" +
                                                   "}}\n";
        
        public const string UI_CLASS_FORMAT = "using UnityEngine;\n" +
                                              "using PolyAndCode.UI;\n" +
                                              "using {0}.Objects;\n" +
                                              "\n" +
                                              "namespace {0}.UI\n" +
                                              "{{\n" +
        "\tpublic class {1}UI : MonoBehaviour, ICell\n" +
        "\t{{\n" +
            "\t\t#region Properties and Fields\n" +
"\n" +
            "\t\tprivate {1} {2};\n" +
"\n" +
            "\t\t#endregion\n" +
"\n" +
            "\t\tpublic void Hookup({1}UIData {2}UIData)\n" +
            "\t\t{{\n" +
                "\t\t\t{2} = {2}UIData.{1};\n" +
            "\t\t}}\n" +
        "\n" +
            "\t\t#region Unity Methods\n" +
"\n" +
            "\t\tprivate void OnDisable()\n" +
            "\t\t{{\n" +
                "\t\t\t{2} = null;\n" +
            "\t\t}}\n" +
"\n" +
            "\t\t#endregion\n" +
        "\t}}\n" +
    "}}\n";
    }
}
