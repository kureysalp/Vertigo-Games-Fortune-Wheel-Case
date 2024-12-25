using DG.Tweening;
using UnityEngine;

namespace VertigoGamesCase.Game.Scripts.Fortune_Wheel
{
    public class WheelSlotItem : BaseItem
    {
        public void SetItem(ItemRewardData itemRewardData, float rewardMultiplier)
        {
            ItemRewardData = itemRewardData;
            AmountOfItems = itemRewardData.BaseItemAmount + (int)(itemRewardData.BaseItemAmount * rewardMultiplier);

            itemRewardImage.sprite = itemRewardData.ItemSprite;

            if (itemRewardData.IsBomb)
                itemAmountText.gameObject.SetActive(false);
            else
                itemAmountText.SetText(AmountOfItems.ToString());

            ItemAppearAnimation();
        }

        public void SetItemOnSlot(int itemIndex, float slotCount)
        {
            var _newAngle = 360 / slotCount * itemIndex;

            var _rectTransform = GetComponent<RectTransform>();
            _rectTransform.localRotation = Quaternion.Euler(0, 0, -_newAngle);
        }

        private void ItemAppearAnimation()
        {
            transform.DOComplete();

            var _punchValue = Vector3.one * animationPower;
            transform.DOPunchScale(_punchValue, animationTime, 1, 1).SetEase(Ease.OutCubic);
        }
    }
}