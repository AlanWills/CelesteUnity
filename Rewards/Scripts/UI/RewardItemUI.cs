using Celeste.Rewards.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Rewards.UI
{
    [AddComponentMenu("Celeste/Rewards/UI/Reward Item UI")]
    public class RewardItemUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI rewardQuantity;
        [SerializeField] private bool showQuantity = true;

        #endregion

        public void Hookup(RewardItem rewardItem)
        {
            Debug.Assert(rewardItem != null, $"Null reward inputted into {nameof(RewardUI)}.");
            if (rewardItem != null)
            {
                rewardIcon.sprite = rewardItem.Icon;
                rewardQuantity.text = $"x {rewardItem.Quantity}";

                rewardQuantity.gameObject.SetActive(showQuantity);
            }
        }
    }
}
