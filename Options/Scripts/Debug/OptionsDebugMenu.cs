using Celeste.Debug.Menus;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Options.Debug
{
    [CreateAssetMenu(fileName = nameof(OptionsDebugMenu), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Debug/Options Debug Menu", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class OptionsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private OptionsRecord options;

        private int boolOptionsCurrentPage;
        private int intOptionsCurrentPage;
        private int floatOptionsCurrentPage;
        private int stringOptionsCurrentPage;

        private const int ENTRIES_PER_PAGE = 8;

        #endregion

        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(true)))
            {
                options.ResetAll();
            }

            // Bool Options
            {
                GUILayout.Space(5);
                using (Section("Bool Options"))
                {
                    boolOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                        boolOptionsCurrentPage,
                        ENTRIES_PER_PAGE,
                        options.NumBoolOptions,
                        (i) =>
                        {
                            BoolOption boolOption = options.GetBoolOption(i);
                            boolOption.Value = GUILayout.Toggle(boolOption.Value, boolOption.DisplayName);
                        });
                }
            }

            // Int Options
            {
                GUILayout.Space(5);
                using (Section("Int Options"))
                {
                    intOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                        intOptionsCurrentPage,
                        ENTRIES_PER_PAGE,
                        options.NumIntOptions,
                        (i) =>
                        {
                            IntOption intOption = options.GetIntOption(i);
                            intOption.Value = GUIExtensions.IntField(intOption.DisplayName, intOption.Value);
                        });
                }
            }

            // Float Options
            {
                GUILayout.Space(5);
                using (Section("Float Options"))
                {
                    floatOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                        floatOptionsCurrentPage,
                        ENTRIES_PER_PAGE,
                        options.NumFloatOptions,
                        (i) =>
                        {
                            FloatOption floatOption = options.GetFloatOption(i);
                            floatOption.Value = GUIExtensions.FloatField(floatOption.DisplayName, floatOption.Value);
                        });
                }
            }

            // String Options
            {
                GUILayout.Space(5);
                using (Section("String Options"))
                {
                    stringOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                        stringOptionsCurrentPage,
                        ENTRIES_PER_PAGE,
                        options.NumStringOptions,
                        (i) =>
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                StringOption stringOption = options.GetStringOption(i);
                                GUILayout.Label(stringOption.DisplayName);
                                stringOption.Value = GUILayout.TextField(stringOption.Value);
                            }
                        });
                }
            }
        }
    }
}
