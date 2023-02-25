using System;
using UnityEngine;
using ComponentPool;

namespace DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        #region Singleton

        public static DialogueTrigger Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { return; }

            Instance = this;
        }

        #endregion

        private DialogueManager dialogueManager;

        #region Script Behaviour

        private void Start()
        {
            dialogueManager = CustomContainer.GetContent<DialogueManager>("Dialogue");
        }

        #endregion

        public void TriggerDialogue(DialogueAsset dialogue)
        {
            dialogueManager.StartDialogue(dialogue);
        }

        public void TriggerDialogue(DialogueAsset dialogue, Action onEnd) 
        {
            dialogueManager.StartDialogue(dialogue, onEnd);
        }
    }
}