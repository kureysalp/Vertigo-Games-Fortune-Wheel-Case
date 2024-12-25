using DG.Tweening;
using TMPro;
using UnityEngine;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Reward_Popup
{
    public class RewardPopupItem : BaseItem
    {
        [SerializeField] private TextMeshProUGUI itemName;

        public void SetItem(BaseItem itemReward)
        {
            ItemRewardData = itemReward.ItemRewardData;
            AmountOfItems = itemReward.AmountOfItems;

            itemRewardImage.sprite = ItemRewardData.ItemSprite;
            if (ItemRewardData.IsBomb)
                itemAmountText.gameObject.SetActive(false);
            else
            {
                itemAmountText.gameObject.SetActive(true);
                itemAmountText.SetText(AmountOfItems.ToString());
            }

            itemName.SetText(ItemRewardData.ItemName);

            PopUpAnimation();
        }

        private void PopUpAnimation()
        {
            itemRewardImage.transform.localScale = Vector3.zero;
            itemAmountText.transform.localScale = Vector3.zero;
            itemName.transform.localScale = Vector3.zero;

            var _doTweenSequence = DOTween.Sequence();

            var _animationVectorScale = Vector3.one;
            _doTweenSequence.Append(itemRewardImage.transform.DOScale(_animationVectorScale, animationPower).SetEase(Ease.OutCubic));
            _doTweenSequence.Append(itemAmountText.transform.DOScale(_animationVectorScale, animationPower).SetEase(Ease.OutCubic));
            _doTweenSequence.Join(itemName.transform.DOScale(_animationVectorScale, animationPower).SetEase(Ease.OutCubic));
        }
    }
}