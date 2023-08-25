using Celeste.Tools.Attributes.GUI;
using CelesteEditor.DeckBuilding.Utils;
using UnityEditor;
using UnityEngine;
using static CelesteEditor.DeckBuilding.Utils.CreateCardsFromSprites;

namespace CelesteEditor.DeckBuilding.Wizards
{
    public class CreateCardsFromSpritesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateCardsFromSpritesParameters parameters;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Asset Generation/Create Cards From Sprites")]
        public static void ShowCreateCardsFromSpritesWizard()
        {
            DisplayWizard<CreateCardsFromSpritesWizard>("Create Cards", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateCardsFromSprites.CreateCards(parameters);
        }

        #endregion
    }
}
