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

        public QuestAsset Content { get; private set; }

        public bool Interact => this.Content != null;

        public Button Button => this.GetComponent<Button>();
        public Selectable Selectable => this.Button;

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }

        public void SetSlot(QuestAsset item)
        {
            this.Content = item;
            this.questName.text = Content.Detail.title;
            this.questState.text = Content.Detail.questState;
        }

        public void ClearSlot()
        {
            this.Content = null;
            this.questName.text = string.Empty;
            this.questState.text = string.Empty;
        }

        public void CheckSlot() 
        {
            
        }

        public void UIState(bool state) 
        {
            this.gameObject.SetActive(false);
        }

        public void UpdateSlot()
        {
            this.questState.text = Content.Detail.questState.First().ToString().ToUpper();
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
