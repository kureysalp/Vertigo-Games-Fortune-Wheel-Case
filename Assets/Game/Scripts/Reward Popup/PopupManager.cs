using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCase.Game.Scripts.Fortune_Wheel;

namespace VertigoGamesCase.Game.Scripts.Reward_Popup
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private RewardPopupItem rewardPopupItem;
        [SerializeField] private GameObject rewardPopup;
        [SerializeField] private GameObject bombPanel;

        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveUpButton;

        [SerializeField] private float popupDuration;

        private void Awake()
        {
            EventManager.Subscribe<BaseItem>("OnWheelStopped", ShowRewardPopup);
        }

        private void Start()
        {
            giveUpButton.onClick.AddListener(GiveUp);
            reviveUpButton.onClick.AddListener(Revive);

            bombPanel.SetActive(false);
        }

        private void OnValidate()
        {
            if (giveUpButton == null)
                giveUpButton = transform.Find("ui_reward_popup_holder/ui_reward_popup_bomb/ui_reward_popup_bomb_giveup_button").GetComponent<Button>();

            if (reviveUpButton == null)
                reviveUpButton = transform.Find("ui_reward_popup_holder/ui_reward_popup_bomb/ui_reward_popup_bomb_revive_button").GetComponent<Button>();
        }

        private void ShowRewardPopup(BaseItem rewardItem)
        {
            rewardPopupItem.SetItem(rewardItem);
            rewardPopup.SetActive(true);

            if (!rewardItem.ItemRewardData.IsBomb)
                StartCoroutine(CO_HideRewardPopup(rewardItem));
            else
                ShowBombPanel();
        }

        private IEnumerator CO_HideRewardPopup(BaseItem rewardItem)
        {
            bombPanel.SetActive(false);
            yield return new WaitForSeconds(popupDuration);
            rewardPopup.SetActive(false);
            EventManager.TriggerEvent<BaseItem>("OnPopupClosed", rewardItem);
            EventManager.TriggerEvent("OnPopupClosed");
        }

        private void ShowBombPanel()
        {
            bombPanel.SetActive(true);
        }

        private void GiveUp()
        {
            bombPanel.SetActive(false);
            rewardPopup.SetActive(false);
            EventManager.TriggerEvent("ResetTheGame");
        }

        private void Revive()
        {
            rewardPopup.SetActive(false);
            EventManager.TriggerEvent("OnPopupClosed");
        }
    }
}