using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private List<Spot> spots = new List<Spot>();
   
    public bool FirstEnter { get; set; }

    public bool IsReady { get; private set; }

    public Spot InitialSpot => spots.Find(spot => spot.SpotName == initialSpot);

    private void Start()
    {
        Initialize();
    }

    private void Initialize() 
    {
        var spot = GameManager.Instance.UserData.GetPackables<TeleportSpots>().CrossSpot.spot;

        if (spot != string.Empty) { initialSpot = spot; }

        IEnumerator SpotCheck() 
        {
            bool waitForSpot = true;

            while (waitForSpot) 
            {
                IsReady = spots.Exists(match => match.SpotName == initialSpot);

                if (IsReady) { waitForSpot = false; }

                yield return null;
            }
        }

        StartCoroutine(SpotCheck());
    }

    public void AddSpot(Spot spot) 
    {
        if (!spots.Contains(spot)) { spots.Add(spot); }
    }

    public void RemoveSpot(Spot spot) 
    {
        spots.Remove(spot); 
    }
}
