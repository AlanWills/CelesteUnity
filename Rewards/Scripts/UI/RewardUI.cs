using Celeste.Rewards.Catalogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Rewards.UI
{
    [AddComponentMenu("Celeste/Rewards/UI/Reward UI")]
    public class RewardUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI rewardQuantity;
        [SerializeField] private bool showQuantity = true;

        #endregion

        public void Hookup(Reward reward)
        {
            UnityEngine.Debug.Assert(reward != null, $"Null reward inputted into {nameof(RewardUI)}.");
            if (reward != null)
            {
                rewardIcon.sprite = reward.Icon;
                rewardQuantity.text = $"x {reward.Quantity}";

                rewardQuantity.gameObject.SetActive(showQuantity);
            }
        }
    }
}
