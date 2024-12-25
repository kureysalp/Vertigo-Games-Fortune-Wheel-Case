using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Zone_Progress
{
    public class ZoneManager : MonoBehaviour
    {
        [FormerlySerializedAs("currentZoneNumber")] [SerializeField]
        private int currentZoneIndex;

        private List<Zone> activeZoneList = new();

        [SerializeField] private Zone zonePrefab;
        [SerializeField] private int initialZoneCount;
        [SerializeField] private int maxZoneCountInBar;
        [SerializeField] private float distanceBetweenZones;
        [SerializeField] private float zoneLength;
        [SerializeField] private float zoneMoveTime;

        [SerializeField] private Transform zoneContainer;


        [SerializeField] private FortuneWheelType bronzeWheel;
        [SerializeField] private FortuneWheelType safeWheel;
        [SerializeField] private FortuneWheelType superWheel;

        [SerializeField] private Sprite bronzeZoneSprite;
        [SerializeField] private Sprite safeZoneSprite;
        [SerializeField] private Sprite superZoneSprite;

        private void Awake()
        {
            EventManager.Subscribe("OnPopupClosed", ZoneCompleted);
            EventManager.Subscribe("ResetTheGame", ResetAll);
        }

        private void Start()
        {
            InitializeZones();
        }


        private void InitializeZones()
        {
            for (var i = 0; i < initialZoneCount; i++)
                CreateNewZone(i, i + 1);

            SetWheel();
        }

        private void CreateNewZone(int positionIndex, int zoneNumber)
        {
            var _zone = Instantiate(zonePrefab, zoneContainer); // Can be object pooling.
            _zone.RectTransform.anchoredPosition = new Vector2(distanceBetweenZones * positionIndex, 0);
            SetupZone(_zone, zoneNumber);

            activeZoneList.Add(_zone);
        }

        private void SetupZone(Zone zone, int zoneNumber)
        {
            if (zoneNumber % superWheel.OccurInterval == 0)
            {
                zone.SetZone(zoneNumber, superZoneSprite, superWheel);
                return;
            }

            if (zoneNumber % safeWheel.OccurInterval == 0)
            {
                zone.SetZone(zoneNumber, safeZoneSprite, safeWheel);
                return;
            }

            zone.SetZone(zoneNumber, bronzeZoneSprite, bronzeWheel);
            EventManager.TriggerEvent<int>("OnZoneChange", currentZoneIndex + 1);
        }

        private void SetWheel()
        {
            var _centerIndexOfZoneBar = Mathf.FloorToInt((float)maxZoneCountInBar / 2);
            var _indexOfCurrentZone = activeZoneList.Count < maxZoneCountInBar ? currentZoneIndex : _centerIndexOfZoneBar;
            var _currentZone = activeZoneList[_indexOfCurrentZone];

            var _zoneInfo = new ZoneInfo(_currentZone.RewardMultiplier, _currentZone.WheelType);
            EventManager.TriggerEvent<ZoneInfo>("SetWheelOnZoneChange", _zoneInfo);
        }

        private void ZoneCompleted()
        {
            currentZoneIndex++;
            MoveZones();
            SetWheel();
        }

        private void MoveZones()
        {
            var _endOfZoneBarIndex = Mathf.FloorToInt((float)maxZoneCountInBar / 2) + 1;
            CreateNewZone(_endOfZoneBarIndex, currentZoneIndex + _endOfZoneBarIndex);

            var _doTweenSequence = DOTween.Sequence();
            foreach (var _zone in activeZoneList)
            {
                var _zoneCurrentPosition = _zone.RectTransform.anchoredPosition;
                var _zoneNewPosition = _zoneCurrentPosition + distanceBetweenZones * Vector2.left;
                _doTweenSequence.Join(_zone.RectTransform.DOAnchorPos(_zoneNewPosition, zoneMoveTime).SetEase(Ease.OutCubic));
            }

            if (activeZoneList.Count > maxZoneCountInBar)
                activeZoneList.RemoveAt(0);

            _doTweenSequence.OnComplete(NextZoneSetup);
        }

        private void NextZoneSetup()
        {
            if (activeZoneList.Count <= maxZoneCountInBar) return;

            Destroy(activeZoneList[0].gameObject);
        }

        private void ResetAll()
        {
            foreach (var _zone in activeZoneList)
                Destroy(_zone.gameObject);

            activeZoneList.Clear();
            currentZoneIndex = 0;

            InitializeZones();
        }
    }
}