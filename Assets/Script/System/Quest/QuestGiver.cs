using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ComponentPool;

namespace QuestSystem
{
    public class QuestGiver : MonoBehaviour
    {
        [Header("¥æ¥I¸ê°T")]
        [SerializeField]
        private string giver; 
        [SerializeField]
        private Quest quest;
        [SerializeField]
        private List<Quest> standByQuests;
        [Header("NPC")]
        [SerializeField]
        private NPCTrigger trigger; 

        private QuestManager questManager => QuestManager.Instance;

        private void Awake()
        {
            trigger = this.GetComponent<NPCTrigger>();
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize() 
        {
            standByQuests = questManager.GetQuests(giver);

            SetQuest();
        }

        private void SetQuest() 
        {
            if (standByQuests.Count <= 0) { return; }

            var progressQuests = standByQuests.Where(quest => quest.QuestState == EQuestState.Progress).ToArray();

            quest = progressQuests.Length >= 1 ? progressQuests.First() : standByQuests.First();

            quest.QuestEndCallBack = delegate() 
            {
                if(quest != null) 
                {
                    var quest = this.quest;

                    RemoveQuest(quest);
                    trigger.TriggerCallBackReset();
                    questManager.FininshQuest(quest);
                }
            };

            if (trigger != null) { trigger.OnTriggeredCallBack = new NPCTrigger.TriggeredHandler(quest.Invoke); }
        }

        private void RemoveQuest(Quest quest) 
        {
            this.quest = null;

            standByQuests.Remove(quest);
            
            SetQuest();
        }
    }

}