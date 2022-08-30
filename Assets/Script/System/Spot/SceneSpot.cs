using UnityEngine;
using UnityEngine.SceneManagement;
using ComponentPool;

public class SceneSpot : Spot, ICroseSceneHandler
{
    [Header("�ؼг����a�I")]
    [SerializeField]
    protected string targetScene;
    [SerializeField]
    protected string targetSpot;

    public string TargetScene => targetScene;
    public string TargetSpot => targetSpot;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player")) 
        {
            CrossScene();
        }
    }

    public void CrossScene() 
    {
        if (!SpotManager.Instance.FirstEnter) { return; }

        GameManager.Instance.UserData.GoTo(targetScene, targetSpot);

        Components.Reset();
        SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
}
