using UnityEngine;

public class CrossSceneSpot : TeleportSpot
{
    [SerializeField]
    private string targetScene;
    [SerializeField]
    private string targetSpot;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Player")) 
        {
            Debug.Log($"{other.name} goes to {targetScene} at {targetSpot}.");

            //Set Target Scene
            //Set Target Spot
            //Load Target Scene at Target Spot
        }
    }
}
