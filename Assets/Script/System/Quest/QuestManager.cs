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

        [SerializeField]
        private QuestListAsset list;
        [SerializeField]
        private QuestPanel questPanel;
        [SerializeField]
        private List<Quest> acceptQuest = new List<Quest>();

        private QuestListAsset defaultList;
        [SerializeField]
        private List<QuestAsset> quests;

        private string sceneName;

        public Action QuestAcceptCallBack { get; set; }

        public Action<string> EnemyKillCallBack { get; set; }
        public Action<string> ItemPickUpCallBack { get; set; }

        private void Initialize() 
        {
            sceneName = SceneManager.GetActiveScene().name;

            list = Resources.Load<QuestListAsset>(Path.Combine("Quest", "Quest_List"));
            defaultList = Resources.Load<QuestListAsset>(Path.Combine("Quest", "Quest_List_Default"));

            if (list.Count <= 0) { list.Initialize(defaultList.Copy); }

            quests = new List<QuestAsset>(list[sceneName]);
        }

        public List<Quest> GetQuests(string giver) 
        {
            var targetQuests = new List<Quest>();

            foreach (var asset in quests) 
            {
                if (asset.Giver == giver)
                {
                    targetQuests.Add(asset.Quest);
                }
            }

            return targetQuests;
        }

        public void OpenQuestPanel(Quest quest) 
        {
            questPanel.gameObject.SetActive(true);
            questPanel.OpenQuestWindow(quest); 
        }

        public void AcceptQuest(Quest quest) { acceptQuest.Add(quest); }

        public void FininshQuest(Quest quest) 
        { 
            acceptQuest.Remove(quest);

            var asset = quests.Find(match => match.Quest.Equals(quest));

            if (asset.NewQuestAsset.Length != 0) { asset.NewQuestAsset.ToList().ForEach(a => quests.Add(a)); }

            quests.Remove(asset);
        }
    }
}