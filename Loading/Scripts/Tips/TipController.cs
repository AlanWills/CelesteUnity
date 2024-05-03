using Celeste.Tools;
using TMPro;
using UnityEngine;

namespace Celeste.Loading
{
    [AddComponentMenu("Celeste/Loading/Tips/Tip Controller")]
    public class TipController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TipsRecord tipsRecord;
        [SerializeField] private TextMeshProUGUI tipText;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref tipText);
        }

        private void OnEnable()
        {
            string randomTip = tipsRecord.RandomTip;
            if (!string.IsNullOrEmpty(randomTip))
            {
                tipText.text = tipsRecord.RandomTip;
            }
        }

        #endregion
    }
}
