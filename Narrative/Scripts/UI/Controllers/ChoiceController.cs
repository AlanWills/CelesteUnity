using Celeste.Narrative.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Choice Controller")]
    public class ChoiceController : MonoBehaviour
    {
        #region Properties and Fields

        private IChoice choice;
        private Action<IChoice> onChosen;

        #endregion

        protected void Hookup(IChoice choice, Action<IChoice> onChosen)
        {
            this.choice = choice;
            this.onChosen = onChosen;

            // Hook up InvalidBehaviour here
        }

        #region Unity Methods

        private void OnDisable()
        {
            choice = null;
            onChosen = null;
        }

        #endregion

        #region Callbacks

        public void OnChoiceSelected()
        {
            if (onChosen != null)
            {
                onChosen(choice);
            }
        }

        #endregion
    }
}
