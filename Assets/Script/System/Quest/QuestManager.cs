using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        #region Singleton
        
        public static QuestManager Instance { get; private set; }

        private void Awake() 
        {
            if(Instance != null) { return; }

            Instance = this;

            Initialize();
        }

        #endregion

        public QuestListAsset QuestList { get; private set; }
        
        private string SceneName { get; set; }
        
        public IConfirmUICtrlHandler<QuestAsset> AcceptUI { get; private set; }

        public event Action<string> EnemyKillCallBack 
        {
            add => QuestList.EnemyKillCallBack += value;

            remove => QuestList.EnemyKillCallBack -= value;
        }

        public event Action<string> ItemPickUpCallBack
        {
            add => QuestList.ItemPickUpCallBack += value;

            remove => QuestList.ItemPickUpCallBack -= value;
        }

        public Action<bool> QuestUpdateCallBack { get; set; }

        public ICategoryHandler.Category Category => QuestList.Category;

        private void Start()
        {
            AcceptUI = CustomContainer.GetContent<QuestAcceptUI>("QuestAccept");
        }

        private void Initialize() 
        {
            SceneName = SceneManager.GetActiveScene().name;

            QuestList = GameManager.Instance.UserData.GetPackables<QuestListAsset>();
        }

        public void OpenQuestPanel(QuestAsset quest) 
        {
            AcceptUI.SendMessage(quest);
        }

        public void AcceptQuest(QuestAsset quest) 
        {
            quest.Detail.questState = "Progress";

            this.EnemyKillCallBack += quest.OnQuestGather;
            this.UpdateUI(false);
        }

        public void FinishQuest(QuestAsset quest) 
        {
            quest.Detail.questState = "Finish";

            this.EnemyKillCallBack -= quest.OnQuestGather;
            this.UpdateUI(false);
        }

        public void RemoveQuest(QuestAsset quest) 
        {
            QuestList.RemoveQuest(quest);

            this.UpdateUI(true);
        }

        #region Get Quest

        public QuestAsset GetQuest(string giver) 
        {
            return QuestList.GetQuest(SceneName, giver);
        }

        public List<QuestAsset> GetQuestList(string giver) 
        {
            return QuestList.GetQuests(SceneName, giver);
        }

        #endregion

        #region Action Invoke

        public void EnemyKilled(string enemy) 
        {
            if (QuestList.EnemyKillCallBack != null) { QuestList.EnemyKillCallBack.Invoke(enemy); }
        }

        public void ItemPickedUp(string item) 
        {
            if (QuestList.ItemPickUpCallBack != null) { QuestList.ItemPickUpCallBack.Invoke(item); }
        }

        #endregion

        public void UpdateUI(bool refresh) 
        {
            if (QuestUpdateCallBack != null) { QuestUpdateCallBack.Invoke(refresh); }
        }
    }
}