using UnityEngine;

public abstract class EventTrigger : MonoBehaviour
{
    [System.Serializable]
    public enum EInvokeState 
    {
        Start = 0,
        Passive = 1,
        Interact = 2,
    }

    [Header("Trigger Detail")]
    [SerializeField] 
    protected EInvokeState invokeState = EInvokeState.Interact;
    [SerializeField] 
    protected string eventType;
    [SerializeField] 
    protected bool destroy = false;

    protected GameObject hint;

    protected bool isTriggered = false;

    #region Reachable Properties

    public bool IsTriggered => isTriggered;

    public bool IsTriggeredDestroy => destroy;

    #endregion

    #region Trigger Event

    protected abstract void OnTriggerEnter2D(Collider2D collider);

    protected abstract void OnTriggerStay2D(Collider2D collider);

    protected abstract void OnTriggerExit2D(Collider2D collider);

    #endregion

    protected abstract void StartInvoke();

    protected virtual void InteractHint(bool state)
    {
        
    }
}
