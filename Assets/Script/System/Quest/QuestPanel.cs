using UnityEngine;
using UnityEngine.UI;
using ComponentPool;

namespace QuestSystem
{
    public class QuestPanel : MonoBehaviour
    {
        [SerializeField]
        private Text title;
        [SerializeField]
        private Text description;
        [SerializeField]
        private Text reward;
        [SerializeField]
        private Button accept;
        [SerializeField]
        private Button cancel;

        private Quest quest;

        private GameManager gameManager => GameManager.Instance;
        private QuestManager questManager => QuestManager.Instance;

        private void Awake()
        { 
            accept.onClick.AddListener(AcceptClicked);
            cancel.onClick.AddListener(CancelClicked);

            gameObject.SetActive(false);
        }

        public void OpenQuestWindow(Quest quest) 
        {
            UIState(true);

            this.quest = quest;

            title.text = this.quest.Title;
            description.text = this.quest.Description;
            reward.text = $"{this.quest.GoldReward}G";
        }

        #region Event Function

        private void AcceptClicked() 
        {
            questManager.QuestAcceptCallBack.Invoke();

            questManager.AcceptQuest(quest);

            ResetQuest();

            UIState(false);
        }

        private void CancelClicked()
        {
            ResetQuest();

            UIState(false);
        }

        private void ResetQuest()
        {
            questManager.QuestAcceptCallBack = null;
            quest = null;
        }

        private void UIState(bool state) 
        {
            gameObject.SetActive(state);
            gameManager.questMode = state;
        }

        #endregion
    }
}