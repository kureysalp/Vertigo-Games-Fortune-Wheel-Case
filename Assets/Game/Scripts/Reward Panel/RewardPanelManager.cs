using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Reward_Panel
{
    public class RewardPanelManager : MonoBehaviour
    {
        private Dictionary<ItemRewardData, RewardItem> itemRewardDictionary = new();

        [SerializeField] private Transform rewardItemContainer;
        [SerializeField] private RewardItem rewardItemPrefab;

        [SerializeField] private GameObject rewardFeedbackPanel;

        [SerializeField]  private Button exitButton;

        private void Awake()
        {
            EventManager.Subscribe<BaseItem>("OnPopupClosed", AddReward);
            EventManager.Subscribe("OnWheelBusy", SetPanelBusy);
            EventManager.Subscribe("OnWheelAvailable", SetPanelAvailable);
            EventManager.Subscribe("ResetTheGame", ClearRewards);

            exitButton.onClick.AddListener(GetRewards);
        }

        private void OnValidate()
        {
            if (exitButton == null)
                exitButton = GetComponentInChildren<Button>();
        }

        private void AddReward(BaseItem item)
        {
            if (itemRewardDictionary.TryGetValue(item.ItemRewardData, out var rewardItem))
            {
                rewardItem.IncreaseRewardAmount(item.AmountOfItems);
                EventManager.TriggerEvent<RewardItem>("OnRewardAddedToPanel", rewardItem);
            }
            else
            {
                var _newRewardItem = Instantiate(rewardItemPrefab, rewardItemContainer);
                _newRewardItem.SetItem(item);
                itemRewardDictionary.Add(item.ItemRewardData, _newRewardItem);
                EventManager.TriggerEvent<RewardItem>("OnRewardAddedToPanel", _newRewardItem);
            }
        }

        private void SetPanelBusy()
        {
            exitButton.interactable = false;
        }

        private void SetPanelAvailable()
        {
            exitButton.interactable = true;
        }

        private void GetRewards()
        {
            rewardFeedbackPanel.SetActive(true);
            ClearRewards();
            EventManager.TriggerEvent("ResetTheGame");

            Invoke(nameof(HideRewardFeedbackPanel), 3.0f);
        }

        private void HideRewardFeedbackPanel()
        {
            rewardFeedbackPanel.SetActive(false);
        }

        private void ClearRewards()
        {
            foreach (var _item in itemRewardDictionary)
                Destroy(_item.Value.gameObject);

            itemRewardDictionary.Clear();
        }
    }
}