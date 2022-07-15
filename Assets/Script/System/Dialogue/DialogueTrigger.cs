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

        public Action DialogueStartCallBack { get; set; }
        public Action DialogueEndCallBack { get; set; }

        #region Script Behaviour

        private void Start()
        {
            dialogueManager = Components.GetStaff<DialogueManager>("Manager_Dialogue", EComponentGroup.Script);
        }

        #endregion

        public void TriggerDialogue(DialogueAsset asset)
        {
            dialogueManager.StartDialogue(asset);
        }
    }
}