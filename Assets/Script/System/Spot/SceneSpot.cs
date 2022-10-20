using UnityEngine;

public class SceneSpot : Spot, ICroseSceneSpotHandler
{
    [Header("目標場景地點")]
    [SerializeField]
    protected string targetScene;
    [SerializeField]
    protected string targetSpot;

    public string TargetScene => targetScene;
    public string TargetSpot => targetSpot;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            CrossScene();
        }
    }

    public void CrossScene() 
    {
        if (!SpotManager.Instance.FirstEnter) { return; }

        var spots = GameManager.Instance.UserData.GetPackables<TeleportSpots>();

        spots.SetLocate(this);

        TeleportManager.Instance.Teleport(spots.CrossSpot);
    }
}
