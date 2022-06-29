using UnityEngine;
using ComponentPool;

public class TeleportSpots : MonoBehaviour
{
    [SerializeField]
    private string initialSpot;
    
    private TeleportSpot currentSpot;
    
    public Transform CurrentSpot => currentSpot.Spot;

    public bool Flip => currentSpot.Flip;

    private void Awake()
    {
        Components.Add(this, "TeleportSpots", EComponentGroup.Script);
    }

    private void Start()
    {
        SpotInitialize();
    }

    private void SpotInitialize() 
    {
        currentSpot = Components.GetStaff<TeleportSpot>(initialSpot, EComponentGroup.TeleportSpot);
    }

    public void SetSpot(TeleportSpot spot) 
    {
        currentSpot = spot;
    }
}
