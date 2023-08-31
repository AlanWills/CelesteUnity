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
            Hookup(rewardItem, 1, 1);
        }

        public void Hookup(RewardItem rewardItem, int multiplier)
        {
            Hookup(rewardItem, multiplier, multiplier);
        }

        public void Hookup(RewardItem rewardItem, int baseMultiplier, int bonusMultiplier)
        {
            Debug.Assert(rewardItem != null, $"Null reward inputted into {nameof(RewardUI)}.");
            if (rewardItem != null)
            {
                rewardIcon.sprite = rewardItem.Icon;

                if (rewardItem.CanBeMultiplied)
                {
                    rewardQuantity.text = bonusMultiplier != 1 ?
                        $"x <s>{rewardItem.Quantity * baseMultiplier}</s> {rewardItem.Quantity * baseMultiplier * bonusMultiplier}" :
                        $"x {rewardItem.Quantity * bonusMultiplier}";
                }
                else
                {
                    rewardQuantity.text = $"x {rewardItem.Quantity}";
                }

                rewardQuantity.gameObject.SetActive(showQuantity);
            }
        }
    }
}
