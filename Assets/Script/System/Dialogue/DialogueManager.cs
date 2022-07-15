using System.Collections.Generic;
using UnityEngine;
using ComponentPool;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<Dialogue> conversation;

        private DialoguePanel panel;
        
        private void Awake()
        {
            Components.Add(this, "Manager_Dialogue", EComponentGroup.Script);
        }

        private void Start()
        {
            panel = Components.GetStaff<DialoguePanel>("Panel_Dialogue", EComponentGroup.Script, true);
        }

        public void StartDialogue(DialogueAsset asset)
        {
            conversation = asset.Conversation;

            GameManager.Instance.dialogueMode = true;

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
    }
}