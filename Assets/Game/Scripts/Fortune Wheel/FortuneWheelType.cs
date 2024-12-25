using System.Collections.Generic;
using UnityEngine;

namespace VertigoGamesCase.Game.Scripts.Fortune_Wheel
{
    [CreateAssetMenu(fileName = "Fortune Wheel")]
    public class FortuneWheelType : ScriptableObject
    {
        [SerializeField] private Sprite fortuneWheelSprite;
        [SerializeField] private Sprite fortuneWheelPointerSprite;

        [SerializeField] private List<ItemRewardData> itemRewards = new();

        [SerializeField] private int occurInterval;
        [SerializeField] private bool hasBomb;

        public Sprite FortuneWheelSprite => fortuneWheelSprite;
        public Sprite FortuneWheelPointerSprite => fortuneWheelPointerSprite;

        public List<ItemRewardData> ItemRewards => itemRewards;

        public int OccurInterval => occurInterval;

        public bool HasBomb => hasBomb;
    }
}