using TMPro;
using UnityEngine;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts
{
    public class ZoneGuide : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI safeZoneText;
        [SerializeField] private TextMeshProUGUI superZoneText;

        [SerializeField] private FortuneWheelType safeWheelType;
        [SerializeField] private FortuneWheelType superWheelType;

        private void Awake()
        {
            EventManager.Subscribe<int>("OnZoneChange", ZoneChanged);
        }


        private void ZoneChanged(int zoneNumber)
        {
            var _nextSafeZone = safeWheelType.OccurInterval + zoneNumber / safeWheelType.OccurInterval * safeWheelType.OccurInterval;
            if (_nextSafeZone % superWheelType.OccurInterval == 0)
                _nextSafeZone += safeWheelType.OccurInterval;
            safeZoneText.SetText(_nextSafeZone.ToString());

            var _nextSuperZone = superWheelType.OccurInterval + zoneNumber / superWheelType.OccurInterval * superWheelType.OccurInterval;
            superZoneText.SetText(_nextSuperZone.ToString());
        }
    }
}