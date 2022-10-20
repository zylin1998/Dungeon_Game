using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour, IPageStateHandler
    {
        private Queue<Dialogue> conversation;

        private DialoguePanel panel;

        #region IPageStateHandler

        public bool PageState { get; set; }

        #endregion

        private void Awake()
        {
            CustomContainer.AddContent(this, "Dialogue");
        }

        private void Start()
        {
            GameManager.Instance.AddPage(this);

            panel = CustomContainer.GetContent<DialoguePanel>("Dialogue");
        }

        public void StartDialogue(DialogueAsset asset)
        {
            conversation = asset.Conversation;

            this.PageState = true;

            panel.PanelState(true);
        }

        public void DisplayNextSentence()
        {
            if (conversation.Count == 0) 
            { 
                EndDialogue();
                return; 
            }

            Dialogue dialogue = conversation.Dequeue();

            panel.DisplayDialogue(dialogue);
        }

        public void EndDialogue()
        {
            panel.PanelState(false);
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Dialogue");
        }
    }
}