using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCase.Game.Scripts.Zone_Progress;
using Random = UnityEngine.Random;

namespace VertigoGamesCase.Game.Scripts.Fortune_Wheel
{
    public class FortuneWheel : MonoBehaviour
    {
        private FortuneWheelType fortuneWheelType;
        private List<WheelSlotItem> wheelSlotItems = new();
        [SerializeField] private ItemRewardData bombItem;
        [SerializeField] private Transform wheelTransform;

        private float currentMultiplier;

        [Header("Wheel Properties")] [SerializeField]
        private Image fortuneWheelImage;

        [SerializeField] private Image fortuneWheelPointerImage;

        [SerializeField] private int sliceCount;
        [SerializeField] private float itemCreateIntervalTime;

        [SerializeField] private int spinCount;
        [SerializeField] private float spinDuration;

        public FortuneWheelType TestFortuneWheel;

        [SerializeField] private Button spinButton;

        private bool isWheelBusy;

        private void Awake()
        {
            EventManager.Subscribe<ZoneInfo>("SetWheelOnZoneChange", SetFortuneWheel);
            EventManager.Subscribe("OnPopupClosed", ClearContent);
            EventManager.Subscribe("ResetTheGame", ResetAll);
        }

        private void Start()
        {
            spinButton.onClick.AddListener(SpinTheWheel);
        }

        private void OnValidate()
        {
            if (spinButton == null)
                spinButton = GetComponentInChildren<Button>();
        }


        public WheelSlotItem WheelSlotItem;

        private IEnumerator SetRewardSlices()
        {
            SetWheelBusy();
            yield return new WaitForSeconds(.2f); // Delay for making sure wheel reset is done.

            var _randomIndexForBomb = Random.Range(0, sliceCount);

            for (var i = 0; i < sliceCount; i++)
            {
                if (fortuneWheelType.HasBomb)
                    if (i == _randomIndexForBomb)
                    {
                        SetWheelSlice(_randomIndexForBomb, bombItem);
                        continue;
                    }

                var _randomItem = fortuneWheelType.ItemRewards[Random.Range(0, fortuneWheelType.ItemRewards.Count)];
                SetWheelSlice(i, _randomItem);
                yield return new WaitForSeconds(itemCreateIntervalTime);
            }

            SetWheelAvailable();
        }

        private void SetWheelSlice(int index, ItemRewardData itemReward)
        {
            var _bombItem = Instantiate(WheelSlotItem, wheelTransform); // Can be object pooling.
            _bombItem.SetItem(itemReward, currentMultiplier);
            _bombItem.SetItemOnSlot(index, sliceCount);
            wheelSlotItems.Add(_bombItem);
        }

        private void SetWheelBusy()
        {
            isWheelBusy = true;
            spinButton.interactable = !isWheelBusy;
            EventManager.TriggerEvent("OnWheelBusy");
        }

        private void SetWheelAvailable()
        {
            isWheelBusy = false;
            spinButton.interactable = !isWheelBusy;
            EventManager.TriggerEvent("OnWheelAvailable");
        }

        private void SpinTheWheel()
        {
            if (isWheelBusy) return;
            SetWheelBusy();

            var _weightedRandomIndex = GetWeightedRandomIndex();
            var _targetAngle = spinCount * 360 + 360 / sliceCount * _weightedRandomIndex;

            var _newRotation = new Vector3(0, 0, _targetAngle);
            wheelTransform.DORotate(_newRotation, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic).OnComplete(() => WheelSpinDone(wheelSlotItems[_weightedRandomIndex]));
        }

        private void WheelSpinDone(BaseItem itemReward)
        {
            isWheelBusy = false;
            EventManager.TriggerEvent<BaseItem>("OnWheelStopped", itemReward);
        }

        private void ClearContent()
        {
            foreach (var _wheelSlotItem in wheelSlotItems)
                Destroy(_wheelSlotItem.gameObject);

            wheelSlotItems.Clear();
            wheelTransform.rotation = Quaternion.identity;
        }

        private void SetFortuneWheel(ZoneInfo zoneInfo)
        {
            fortuneWheelType = zoneInfo.WheelType;
            fortuneWheelImage.sprite = fortuneWheelType.FortuneWheelSprite;
            fortuneWheelPointerImage.sprite = fortuneWheelType.FortuneWheelPointerSprite;

            currentMultiplier = zoneInfo.RewardMultiplier;

            StartCoroutine(SetRewardSlices());
        }

        private int GetWeightedRandomIndex()
        {
            var _cumulativeArray = new int[wheelSlotItems.Count];
            var _maxWeight = 0;
            for (var i = 0; i < wheelSlotItems.Count; i++)
            {
                var _weight = 0;
                _weight += wheelSlotItems[i].ItemRewardData.ItemWeight;
                _cumulativeArray[i] = _weight;
                _maxWeight = _weight;
            }

            var _randomWeight = Random.Range(0, _maxWeight);

            var _index = Array.BinarySearch(_cumulativeArray, _randomWeight);
            if (_index < 0)
                _index = ~_index;

            return _index;
        }

        private void ResetAll()
        {
            ClearContent();
            currentMultiplier = 0f;
        }
    }
}