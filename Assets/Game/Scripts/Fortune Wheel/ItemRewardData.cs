using UnityEngine;

namespace VertigoGamesCase.Game.Scripts.Fortune_Wheel
{
    [CreateAssetMenu(fileName = "Item Reward", menuName = "Item Reward Data", order = 0)]
    public class ItemRewardData : ScriptableObject
    {
        [SerializeField] private Sprite itemSprite;

        [SerializeField] private bool isBomb;

        [SerializeField] private string itemName;

        [SerializeField] private int baseItemAmount;

        [Range(0, 30)] [SerializeField] private int itemWeight;


        public Sprite ItemSprite => itemSprite;

        public bool IsBomb => isBomb;

        public string ItemName => itemName;

        public int ItemWeight => itemWeight;

        public int BaseItemAmount => baseItemAmount;
    }
}