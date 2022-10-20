using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestAcceptDetail : MonoBehaviour, IDetailPanelHandler<QuestAsset>
    {
        [SerializeField]
        private Text title;
        [SerializeField]
        private Text description;
        [SerializeField]
        private Text reward;

        private void Awake()
        {
            CustomContainer.AddContent(this, "QuestAccept");
        }

        public void SetDetail(QuestAsset quest) 
        {
            var detail = quest.Detail;

            this.title.text = detail.title;
            this.description.text = detail.description;
            this.reward.text = $"{detail.goldReward}G";
        }

        public void Clear() 
        {
            this.title.text = string.Empty;
            this.description.text = string.Empty;
            this.reward.text = string.Empty;
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "QuestAccept");
        }
    }
}