using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using ComponentPool;
using DialogueSystem;

public class NPCTrigger : MonoBehaviour
{
    [SerializeField]
    private string basicDialogue;
    [SerializeField]
    private DialogueAsset basicDialogueAsset;

    private Vector3 scale;

    private DialogueTrigger dialogueTrigger => DialogueTrigger.Instance;

    public delegate void TriggeredHandler();
    public TriggeredHandler OnTriggeredCallBack { private get; set; }
    
    float vertical => CheckInput();

    private void Awake()
    {
        basicDialogueAsset = Resources.Load<DialogueAsset>(Path.Combine("Dialogue", basicDialogue));

        scale = transform.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        float side = collision.transform.position.x <= transform.position.x ? -1f : 1f;

        Flip(side);
        
        if (vertical != 0 && !GameManager.Instance.pause) 
        {
            if (OnTriggeredCallBack == null) { dialogueTrigger.TriggerDialogue(basicDialogueAsset); }

            if (OnTriggeredCallBack != null) { OnTriggeredCallBack.Invoke(); }
        }
    }

    public void Flip(float flip)
    {
        var newScale = scale;

        newScale.x *= flip;

        transform.localScale = newScale;
    }

    public void TriggerCallBackReset() 
    {
        OnTriggeredCallBack = null;
    }

    public float CheckInput() 
    {
#if UNITY_STANDALONE_WIN
        return KeyManager.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
        return 0;
#endif
    }
}
