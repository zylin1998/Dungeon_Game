using UnityEngine;
using ComponentPool;

public class Spot : MonoBehaviour, IDocumentHandler
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
        Components.Add(this, spotName, EComponentGroup.Spots);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Documental();
        }
    }
    
    protected virtual void OnTriggerStay2D(Collider2D collision) 
    {
        //Do Something
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) 
    {
        SpotManager.Instance.FirstEnter = true;
    }

    public virtual void Documental() 
    {
        SpotManager.Instance.SetSpot(this);
    }
}
