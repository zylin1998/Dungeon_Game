using System;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using InventorySystem;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "Quest Asset", menuName = "Quest/Quest Asset", order = 1)]
    public class QuestAsset : PackableObject
    {
        #region Quest Packed

        [System.Serializable]
        public class Pack : PackableObjectPack
        {
            public string quest;
            public string questState;
            public QuestGoalAsset.Pack goal;

            public Pack() { }

            public Pack(QuestAsset asset)
            {
                this.quest = asset.name;
                this.questState = asset.detail.questState;
                this.goal = asset.goal.Packed as QuestGoalAsset.Pack;
            }
        }

        #endregion

        #region Giver

        [Serializable]
        public class QuestGiver
        {
            public string scene;
            public string giver;
        }

        #endregion

        #region Detail

        [Serializable]
        public class QuestDetail
        {
            public string title;
            public string description;
            public int goldReward;
            public string questState;
        }

        #endregion

        #region Dialogue

        [Serializable]
        public class QuestDialogue
        {
            public DialogueAsset start;
            public DialogueAsset progress;
            public DialogueAsset finish;

            public DialogueAsset GetDialogue(string questState)
            {
                DialogueAsset dialogue = null;

                if (questState == "Start") { dialogue = start; }
                if (questState == "Progress") { dialogue = progress; }
                if (questState == "Finish") { dialogue = finish; }

                return dialogue;
            }
        }

        #endregion

        [Header("任務生成對象")]
        [SerializeField]
        private QuestGiver giver;
        [Header("任務資訊")]
        [SerializeField]
        private QuestDetail detail;
        [SerializeField]
        private QuestGoalAsset goal;
        [Header("任務資訊")]
        [SerializeField]
        private QuestDialogue dialogue;
        [Header("新開放任務")]
        [SerializeField]
        private List<QuestAsset> newQuestAsset = new List<QuestAsset>();

        public QuestGiver Giver => this.giver;
        public QuestDetail Detail => this.detail;
        public string Targets => goal.ToString();

        public List<QuestAsset> NewQuestAsset => newQuestAsset;
        
        public override IPackableHandler.BasicPack Packed => new Pack(this);

        public Action QuestEndCallBack { get; set; }
        
        public override void Initialized()
        {
            this.detail.questState = "Start";
            this.goal.Initialized();
        }

        public override void Initialized(IPackableHandler.BasicPack basicPack)
        {
            if (basicPack is Pack pack) 
            { 
                pack.UnPacked(this, packable => 
                {
                    this.detail.questState = pack.questState;
                    this.goal.Initialized(pack.goal);
                }); 
            }
        }

        public void Invoke()
        {
            var trigger = DialogueTrigger.Instance;
            var category = QuestManager.Instance.Category;
            var dialogue = this.dialogue.GetDialogue(this.detail.questState);

            if (category.Compare(this.detail.questState, "Start")) 
            { 
                trigger.TriggerDialogue(dialogue, () => { QuestManager.Instance.OpenQuestPanel(this); }); 
            }

            if (category.Compare(this.detail.questState, "Progress")) 
            {
                trigger.TriggerDialogue(dialogue, () => { }); 
            }

            if (category.Compare(this.detail.questState, "Finish")) 
            {
                trigger.TriggerDialogue(dialogue, Reward); 
            }
        }

        private void Reward()
        {
            if (this.QuestEndCallBack != null) { this.QuestEndCallBack.Invoke(); }

            Inventory.Instance.IncreaseGold(detail.goldReward);
        }

        public void OnQuestGather(string targetName)
        {
            this.goal.AmountGathered(targetName);

            if (this.goal.IsReached())
            {
                QuestManager.Instance.FinishQuest(this);
            }
        }
    }
}