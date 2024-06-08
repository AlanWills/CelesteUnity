using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject.Wizards
{
    public class CreateFeatureClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateFeatureClassesParameters parameters;
        [SerializeField] private TextAsset objectClassTemplate;
        [SerializeField] private TextAsset catalogueClassTemplate;
        [SerializeField] private TextAsset catalogueEditorClassTemplate;
        [SerializeField] private TextAsset recordClassTemplate;
        [SerializeField] private TextAsset nonPersistentManagerClassTemplate;
        [SerializeField] private TextAsset persistentManagerClassTemplate;
        [SerializeField] private TextAsset dtoClassTemplate;
        [SerializeField] private TextAsset persistentMenuItemsClassTemplate;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Code Generation/Create Feature Classes")]
        public static void ShowCreateFeatureClassesWizard()
        {
            DisplayWizard<CreateFeatureClassesWizard>("Create Feature Classes", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            parameters.SetDefault();

            if (objectClassTemplate == null)
            {
                objectClassTemplate = EditorOnly.FindAsset<TextAsset>("FeatureObjectClassTemplate");
            }

            if (catalogueClassTemplate == null)
            {
                catalogueClassTemplate = EditorOnly.FindAsset<TextAsset>("FeatureCatalogueClassTemplate");
            }

            if (catalogueEditorClassTemplate == null)
            {
                catalogueEditorClassTemplate = EditorOnly.FindAsset<TextAsset>("FeatureCatalogueEditorClassTemplate");
            }

            if (recordClassTemplate == null)
            {
                recordClassTemplate = EditorOnly.FindAsset<TextAsset>("FeatureRecordClassTemplate");
            }

            if (nonPersistentManagerClassTemplate == null)
            {
                nonPersistentManagerClassTemplate = EditorOnly.FindAsset<TextAsset>("FeatureNonPersistentManagerClassTemplate");
            }

            if (persistentManagerClassTemplate == null)
            {
                persistentManagerClassTemplate = EditorOnly.FindAsset<TextAsset>("FeaturePersistentManagerClassTemplate");
            }

            if (dtoClassTemplate == null)
            {
                dtoClassTemplate = EditorOnly.FindAsset<TextAsset>("FeatureDTOClassTemplate");
            }

            if (persistentMenuItemsClassTemplate == null)
            {
                persistentMenuItemsClassTemplate = EditorOnly.FindAsset<TextAsset>("FeaturePersistentMenuItemsClassTemplate");
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateFeatureClasses.Create(
                parameters,
                objectClassTemplate.text,
                catalogueClassTemplate.text,
                catalogueEditorClassTemplate.text,
                recordClassTemplate.text,
                nonPersistentManagerClassTemplate.text,
                persistentManagerClassTemplate.text,
                dtoClassTemplate.text,
                persistentMenuItemsClassTemplate.text);
        }

        #endregion
    }
}