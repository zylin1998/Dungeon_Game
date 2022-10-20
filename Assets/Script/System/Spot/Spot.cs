using UnityEngine;
using ComponentPool;

public class Spot : MonoBehaviour
{
    [Header("基本地點資訊")]
    [SerializeField]
    protected string spotName;
    [SerializeField]
    protected bool flip;

    #region Properties

    public string SpotName => spotName;
    public bool Flip => flip;

    public Vector2 Position => this.transform.position;
    
    #endregion

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        SpotManager.Instance.AddSpot(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Enter Action
    }
    
    protected virtual void OnTriggerStay2D(Collider2D collision) 
    {
        //Stay Action
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) 
    {
        SpotManager.Instance.FirstEnter = true;
    }
}
