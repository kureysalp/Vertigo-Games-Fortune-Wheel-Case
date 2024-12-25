using System;
using DG.Tweening;
using UnityEngine;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Reward_Panel
{
    public class RewardItem : BaseItem
    {
        [SerializeField] private float amountTextIncreaseAnimationDuration;

        private RectTransform itemImageRectTransform;

        public RectTransform ItemImageRectTransform => itemImageRectTransform;


        private void Awake()
        {
            itemImageRectTransform = itemRewardImage.GetComponent<RectTransform>();
        }

        public void SetItem(BaseItem itemReward)
        {
            ItemRewardData = itemReward.ItemRewardData;
            AmountOfItems = itemReward.AmountOfItems;

            itemRewardImage.sprite = ItemRewardData.ItemSprite;
            itemAmountText.SetText(AmountOfItems.ToString());
            AppearAnimation();
        }

        public void IncreaseRewardAmount(int amount)
        {
            var _oldAmount = AmountOfItems;
            AmountOfItems += amount;

            DOTween.To(() => _oldAmount, x => _oldAmount = x, AmountOfItems, amountTextIncreaseAnimationDuration)
                .OnUpdate(() => itemAmountText.SetText(AmountOfItems.ToString()));
            var _animationPowerVector = Vector3.one * animationPower;
            itemAmountText.transform.DOPunchScale(_animationPowerVector, animationTime).SetEase(Ease.OutCubic);
        }

        private void AppearAnimation()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, animationTime).SetEase(Ease.OutCubic);
        }
    }
}