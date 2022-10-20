using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport Spots", menuName = "Teleport/Spot List", order = 1)]
public class TeleportSpots : PackableObject
{
    [System.Serializable]
    public class TSData
    {
        public string scene;
        public string spot;
            
        public TSData() : this(string.Empty, string.Empty) { }

        public TSData(TSData spot) : this(spot.scene, spot.spot) { }

        public TSData(ITeleportSpotHandler spot) : this(spot.SceneName, spot.SpotName) { }

        public TSData(string scene, string spot)
        {
            this.scene = scene;
            this.spot = spot;
        }

        public override string ToString()
        {
            return $"{scene} {spot}";
        }
    }
    
    [System.Serializable]
    public class Pack : PackableObjectPack
    {
        public TSData saveSpot;

        public TSData[] spots;

        public Pack() 
        {
            this.saveSpot = new TSData();

            this.spots = new TSData[0];
        }

        public Pack(TeleportSpots spots) 
        {
            this.saveSpot = spots.SaveSpot;

            this.spots = spots.SpotList.ConvertAll(spot => new TSData(spot)).ToArray();
        }
    }

    [SerializeField]
    private List<TSData> spotList;

    [SerializeField]
    private TSData saveSpot;
    [SerializeField]
    private TSData crossSpot;

    public TSData SaveSpot => this.saveSpot;
    public TSData CrossSpot => this.crossSpot;

    public List<TSData> SpotList => spotList;

    public override IPackableHandler.BasicPack Packed => new Pack(this);

    public override void Initialized()
    {
        crossSpot = new TSData("TownEntry", "Spot 1");

        spotList = new List<TSData>();
    }

    public override void Initialized(IPackableHandler.BasicPack basicPack)
    {
        if (basicPack is Pack pack) 
        {
            pack.UnPacked(this, spots => spots.spotList = pack.spots.ToList());
        }
    }

    public void AddSpot(ITeleportSpotHandler spot) 
    {
        spotList.Add(new TSData(spot));
    }

    public void SaveLocate(IDocumentSpotHandler spot)
    {
        saveSpot = new TSData(spot.SceneName, spot.SpotName);
    }

    public void SetLocate(TSData spot)
    {
        crossSpot = new TSData(spot);
    }

    public void SetLocate(ICroseSceneSpotHandler spot)
    {
        crossSpot = new TSData(spot.TargetScene, spot.TargetSpot);
    }
}
