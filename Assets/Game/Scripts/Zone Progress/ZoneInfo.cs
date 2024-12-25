using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Zone_Progress
{
    public class ZoneInfo
    {
        private float rewardMultiplier;
        private FortuneWheelType wheelType;

        public ZoneInfo(float rewardMultiplier, FortuneWheelType wheelType)
        {
            this.rewardMultiplier = rewardMultiplier;
            this.wheelType = wheelType;
        }

        public float RewardMultiplier => rewardMultiplier;

        public FortuneWheelType WheelType => wheelType;
    }
}