using UnityEngine;
using ComponentPool;

public class TeleportSpot : MonoBehaviour
{
    [SerializeField]
    protected string spotName;
    [SerializeField]
    protected bool flip;

    protected EComponentGroup group = EComponentGroup.TeleportSpot;

    #region Properties

    public string SpotName => spotName;

    public bool Flip => flip;

    public Transform Spot => this.transform;

    public EComponentGroup Group => group;

    #endregion

    protected virtual void Awake()
    {
        Components.Add(this, spotName, group);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var teleportSpots = Components.GetStaff<TeleportSpots>("TeleportSpots", EComponentGroup.Script);

            teleportSpots.SetSpot(this);
        }
    }
}
