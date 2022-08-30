using UnityEngine;
using ComponentPool;

public class SpotManager : MonoBehaviour
{
    #region Singleton

    public static SpotManager Instance { get; private set; }

    private void Awake()
    {
        FirstEnter = false;

        if (Instance != null) { return; }

        Instance = this;
    }

    #endregion

    [SerializeField]
    private string initialSpot;
   
    public bool FirstEnter { get; set; }

    public Spot CurrentSpot { get; private set; }

    private void Start()
    {
        SpotInitialize();
    }

    private void SpotInitialize() 
    {
        var spot = GameManager.Instance.UserData.Spot;

        if (spot != string.Empty) { initialSpot = spot; }

        CurrentSpot = Components.GetStaff<Spot>(initialSpot, EComponentGroup.Spots);
    }

    public void SetSpot(Spot spot) 
    {
        CurrentSpot = spot;
    }
}
