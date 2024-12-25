using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VertigoGamesCase.Game.Scripts.Fortune_Wheel
{
    public class BaseItem : MonoBehaviour
    {
        public ItemRewardData ItemRewardData { get; set; }
        public int AmountOfItems { get; set; }

        [SerializeField] protected TextMeshProUGUI itemAmountText;
        [SerializeField] protected Image itemRewardImage;

        [SerializeField] protected float animationTime;
        [SerializeField] protected float animationPower;
    }
}