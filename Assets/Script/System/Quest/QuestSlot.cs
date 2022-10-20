using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace QuestSystem
{
    public class QuestSlot : MonoBehaviour, ISlotHandler<QuestAsset>, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
    {
        [SerializeField]
        private Text questName;
        [SerializeField]
        private Text questState;

        public QuestAsset Item { get; private set; }

        public bool Interact { get; private set; }

        public Button Button => this.GetComponent<Button>();

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }

        public void SetSlot(QuestAsset item)
        {
            this.Item = item;
            this.questName.text = Item.Detail.title;
            this.questState.text = Item.Detail.questState;
        }

        public void ClearSlot()
        {
            this.Item = null;
            this.questName.text = string.Empty;
            this.questState.text = string.Empty;
        }

        public void CheckSlot() 
        {
            Interact = Item != null;
        }

        public void UpdateSlot()
        {
            this.questState.text = Item.Detail.questState.First().ToString().ToUpper();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.OnSelect(eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (OnSelectCallBack != null) { OnSelectCallBack.Invoke(); }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitCallBack != null) { OnExitCallBack.Invoke(); }
        }
    } 
}
