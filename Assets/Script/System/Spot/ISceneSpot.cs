using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISceneSpot : InteractSpot, ICroseSceneSpotHandler
{
    [Header("目標場景地點")]
    [SerializeField]
    protected string targetScene;
    [SerializeField]
    protected string targetSpot;

    public string TargetScene => targetScene;
    public string TargetSpot => targetSpot;

    protected override void Awake()
    {
        base.Awake();

        InteractCallBack = CrossScene;
    }

    public void CrossScene()
    {
        if (!SpotManager.Instance.FirstEnter) { return; }

        var spots = GameManager.Instance.UserData.GetPackables<TeleportSpots>();

        spots.SetLocate(this);

        TeleportManager.Instance.Teleport(spots.CrossSpot);
    }
}
