using UnityEngine;
using DialogueSystem;
using InventorySystem;

namespace QuestSystem
{
    #region Quest State Enum

    [System.Serializable]
    public enum EQuestState 
    {
        Start = 0,
        Progress = 1,
        Finish = 2,
    }

    #endregion

    [System.Serializable]
    public class Quest
    {
        [Header("任務資訊")]
        [SerializeField]
        private string title;
        [SerializeField]
        private string description;
        [SerializeField]
        private int goldReward;
        [Header("任務狀態")]
        [SerializeField]
        private EQuestState questState;
        [SerializeField]
        private QuestGoalAsset goal;
        [Header("任務對話")]
        [SerializeField]
        private DialogueAsset startDialogue;
        [SerializeField]
        private DialogueAsset progressDialogue;
        [SerializeField]
        private DialogueAsset finishDialogue;

        public string Title => title;
        public string Description => description;
        public int GoldReward => goldReward;

        public EQuestState QuestState => questState;
        
        public QuestGoalAsset.Packed GoalPack => goal.Pack;

        public delegate void QuestEndHandler();
        public QuestEndHandler QuestEndCallBack { get; set; }

        private QuestManager questManager => QuestManager.Instance;
        private DialogueTrigger dialogueTrigger => DialogueTrigger.Instance;

        public void Initialize() 
        {
            this.questState = EQuestState.Start;
            this.goal.Initialize();
        }

        public void Initialize(QuestAsset.Packed pack) 
        {
            this.questState = pack.questState;
            this.goal.Initialize(pack.goal);
        }

        public void Invoke() 
        {
            if (this.questState == EQuestState.Start) { this.Start(); }

            if (this.questState == EQuestState.Progress) { this.Progress(); }

            if (this.questState == EQuestState.Finish) { this.Finish(); }
        }

        private void Start() 
        {
            dialogueTrigger.TriggerDialogue(this.startDialogue);

            questManager.QuestAcceptCallBack = this.OnQuestAccept;

            dialogueTrigger.DialogueEndCallBack = delegate() { QuestManager.Instance.OpenQuestPanel(this); };
        }

        private void Progress() 
        {
            dialogueTrigger.TriggerDialogue(this.progressDialogue);
        }

        private void Finish() 
        {
            dialogueTrigger.TriggerDialogue(this.finishDialogue);

            dialogueTrigger.DialogueEndCallBack = this.OnQuestEnd;
        }

        private void OnQuestAccept() 
        {
            this.questState = EQuestState.Progress;

            questManager.EnemyKillCallBack += this.OnQuestGather;
        }

        private void OnQuestEnd() 
        {
            if(this.QuestEndCallBack != null) { this.QuestEndCallBack.Invoke(); }

            Inventory.Instance.IncreaseGold(goldReward);
        }

        private void OnQuestGather(string targetName) 
        {
            this.goal.AmountGathered(targetName);

            if (this.goal.IsReached()) 
            {
                this.questState = EQuestState.Finish;

                questManager.EnemyKillCallBack -= this.OnQuestGather;
            }
        }
    }

    [CreateAssetMenu(fileName = "Quest Asset", menuName = "Quest/Quest Asset", order = 1)]
    public class QuestAsset : ScriptableObject
    {
        #region Quest Packed

        [System.Serializable]
        public class Packed
        {
            public string quest;
            public EQuestState questState;
            public QuestGoalAsset.Packed goal;

            public Packed() { }

            public Packed(QuestAsset asset) 
            {
                quest = asset.name;
                questState = asset.Quest.QuestState;
                goal = asset.Quest.GoalPack;
            }
        }

        #endregion

        [Header("任務生成對象")]
        [SerializeField]
        private string giver;
        [SerializeField]
        private string targetScene;
        [SerializeField]
        private Quest quest;

        [Header("新開放任務")]
        [SerializeField]
        private QuestAsset[] newQuestAsset = null;

        public string Giver => giver;
        public string TargetScene => targetScene;
        public Quest Quest => quest;
        public Packed Pack => new Packed(this);
        public QuestAsset[] NewQuestAsset => newQuestAsset;

        public void Initialize() => quest.Initialize();

        public void Initialize(Packed pack) => quest.Initialize(pack);
    }
}