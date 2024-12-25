using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Zone_Progress
{
    public class Zone : MonoBehaviour
    {
        private int zoneNumber;

        private FortuneWheelType fortuneWheelType;

        private Image image;
        [SerializeField] private TextMeshProUGUI zoneText;

        [SerializeField] private float rewardMultiplierBase;

        private RectTransform rectTransform;

        public float RewardMultiplier => rewardMultiplierBase * zoneNumber;
        public FortuneWheelType WheelType => fortuneWheelType;

        public RectTransform RectTransform => rectTransform;

        private void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }


        public void SetZone(int zoneNumber, Sprite zoneSprite, FortuneWheelType fortuneWheelType)
        {
            this.zoneNumber = zoneNumber;
            zoneText.text = zoneNumber.ToString();

            image = GetComponent<Image>();
            image.sprite = zoneSprite;

            this.fortuneWheelType = fortuneWheelType;
        }
    }
}