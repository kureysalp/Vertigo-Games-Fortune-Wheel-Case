using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCase.Game.Scripts.Reward_Panel;
using Random = UnityEngine.Random;

namespace VertigoGamesCase.Game.Scripts.Reward_Popup
{
    public class ItemRewardEffect : MonoBehaviour
    {
        [SerializeField] private RectTransform[] imageRectTransforms;
        [SerializeField] private Image[] images;

        [SerializeField] private float firstBurstDuration;
        [SerializeField] private float firstBurstAmount;
        [SerializeField] private float deliverToTargetDuration;


        private void Awake()
        {
            EventManager.Subscribe<RewardItem>("OnRewardAddedToPanel", SetEffect);
        }

        private void Start()
        {
            foreach (var _imageRect in imageRectTransforms)
                _imageRect.gameObject.SetActive(false);
        }

        public IEnumerator CO_Burst(RectTransform target)
        {
            yield return null;
            var _sequence = DOTween.Sequence();

            foreach (var _image in imageRectTransforms)
            {
                _image.gameObject.SetActive(true);
                _image.anchoredPosition = Vector2.zero;
                var _burstPosition = Random.insideUnitCircle * firstBurstAmount;
                _sequence.Join(_image.DOAnchorPos(_burstPosition, firstBurstDuration).SetEase(Ease.OutQuint));
            }

            _sequence.OnComplete(() =>
            {
                foreach (var _image in imageRectTransforms)
                {
                    var _img = _image; //To not lose reference of image when OnComplete triggered.
                    _image.DOMove(target.position, deliverToTargetDuration).SetEase(Ease.InOutQuart).OnComplete(() => { _img.gameObject.SetActive(false); });
                }
            });
        }


        private void SetEffect(RewardItem rewardItem)
        {
            foreach (var _image in images) _image.sprite = rewardItem.ItemRewardData.ItemSprite;

            StartCoroutine(CO_Burst(rewardItem.ItemImageRectTransform));
        }
    }
}