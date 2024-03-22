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
                GUILayout.Label("Bool Options", GUI.skin.label.New().Bold().MiddleCentreAligned());
                GUILayout.Space(5);

                boolOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                    boolOptionsCurrentPage,
                    ENTRIES_PER_PAGE,
                    options.NumBoolOptions,
                    (i) =>
                    {
                        BoolOption boolOption = options.GetBoolOption(i);
                        GUILayout.Label($"{boolOption.DisplayName}: {boolOption.Value}");
                    });
            }

            // Int Options
            {
                GUILayout.Space(5);
                GUILayout.Label("Int Options", GUI.skin.label.New().Bold().MiddleCentreAligned());
                GUILayout.Space(5);

                intOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                    intOptionsCurrentPage,
                    ENTRIES_PER_PAGE,
                    options.NumIntOptions,
                    (i) =>
                    {
                        IntOption intOption = options.GetIntOption(i);
                        GUILayout.Label($"{intOption.DisplayName}: {intOption.Value}");
                    });
            }

            // Float Options
            {
                GUILayout.Space(5);
                GUILayout.Label("Float Options", GUI.skin.label.New().Bold().MiddleCentreAligned());
                GUILayout.Space(5);

                floatOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                    floatOptionsCurrentPage,
                    ENTRIES_PER_PAGE,
                    options.NumFloatOptions,
                    (i) =>
                    {
                        FloatOption floatOption = options.GetFloatOption(i);
                        GUILayout.Label($"{floatOption.DisplayName}: {floatOption.Value}");
                    });
            }

            // String Options
            {
                GUILayout.Space(5);
                GUILayout.Label("String Options", GUI.skin.label.New().Bold().MiddleCentreAligned());
                GUILayout.Space(5);

                stringOptionsCurrentPage = GUIExtensions.ReadOnlyPaginatedList(
                    stringOptionsCurrentPage,
                    ENTRIES_PER_PAGE,
                    options.NumStringOptions,
                    (i) =>
                    {
                        StringOption stringOption = options.GetStringOption(i);
                        GUILayout.Label($"{stringOption.DisplayName}: {stringOption.Value}");
                    });
            }
        }
    }
}
