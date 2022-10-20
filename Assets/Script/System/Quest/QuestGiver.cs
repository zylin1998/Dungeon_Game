using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RoleSystem;

namespace QuestSystem
{
    public class QuestGiver : MonoBehaviour
    {
        [Header("¥æ¥I¸ê°T")]
        [SerializeField]
        private string giver; 
        [SerializeField]
        private QuestAsset quest;
        
        private IInteractHandler trigger; 

        private void Awake()
        {
            trigger = this.GetComponent<IInteractHandler>();
        }

        private void Start()
        {
            SetQuest();
        }

        private void SetQuest()
        {
            quest = QuestManager.Instance.GetQuest(giver);

            if (quest != null)
            {
                quest.QuestEndCallBack = delegate ()
                {
                    trigger.InteractReset();

                    QuestManager.Instance.RemoveQuest(this.quest);

                    SetQuest();
                };

                if (trigger != null) { trigger.InteractCallBack = quest.Invoke; }
            }
        }
    }

}