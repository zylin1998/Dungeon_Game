using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;
using ComponentPool;

namespace DialogueSystem
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField]
        private Text charaName;
        [SerializeField]
        private Text dialogue;
        [SerializeField]
        private Button next;
        [SerializeField]
        private Button skip;

        private Animator animator;

        private DialogueManager dialogueManager;

        private GameManager gameManager => GameManager.Instance;
        private DialogueTrigger dialogueTrigger => DialogueTrigger.Instance;

        private void Awake()
        {
            CustomContainer.AddContent(this, "Dialogue");

            animator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            dialogueManager = CustomContainer.GetContent<DialogueManager>("Dialogue");

            SetButton();
        }

        public void PanelState(bool isOpen)
        {
            animator.SetBool("isOpen", isOpen);

            charaName.text = string.Empty;
            dialogue.text = string.Empty;

            StartCoroutine(AnimatorPlaying(isOpen));
        }

        public void DisplayDialogue(Dialogue dialogue) 
        {
            charaName.text = dialogue.Name;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogue.Sentence));
        }

        #region Coroutine Functions

        private IEnumerator TypeSentence(string sentence)
        {
            dialogue.text = string.Empty;

            foreach (char letter in sentence)
            {
                //while (IsPause()) { yield return null; }

                if (CheckInput()) { dialogueManager.DisplayNextSentence(); }

                dialogue.text += letter;

                yield return new WaitForSeconds(0.02f);
            }

            StartCoroutine(WaitKey());
        }

        private IEnumerator WaitKey()
        {
            while (true)
            {
                if (CheckInput()) { break; }

                yield return null;
            }

            Invoke("NextClicked", 0.15f);
        }

        private IEnumerator AnimatorPlaying(bool isOpen)
        {
            var name = isOpen ? "Open" : "Close"; 

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName($"Panel_Dialogue_{name}")) { yield return null; }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName($"Panel_Dialogue_{name}")) 
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95) { break; }

                yield return null; 
            }

            if (name == "Open") { dialogueManager.DisplayNextSentence(); }

            if (name == "Close") 
            { 
                dialogueManager.PageState = false;

                if (dialogueTrigger.DialogueEndCallBack != null) 
                { 
                    dialogueTrigger.DialogueEndCallBack.Invoke();
                    dialogueTrigger.DialogueEndCallBack = null;
                }
            }
        }

        #endregion

        #region Initialize

        private void NextClicked()
        {
            dialogueManager.DisplayNextSentence();
        }

        private void SkipClicked()
        {
            dialogueManager.EndDialogue();
        }

        private void SetButton()
        {
            if (next != null) { next.onClick.AddListener(NextClicked); }
            if (skip != null) { skip.onClick.AddListener(SkipClicked); }
        }

        #endregion

        private bool CheckInput() 
        {
#if UNITY_STANDALONE_WIN
            return KeyManager.GetKeyDown("Jump");
#endif

#if UNITY_ANDROID
            return MobileInput.GetButtonDown("Jump");
#endif
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Dialogue");
        }
    }
}