using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ISceneSpot : InteractSpot, ICroseSceneHandler
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

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!SpotManager.Instance.FirstEnter) { return; }

        base.OnTriggerStay2D(collision);
    }

    public void CrossScene()
    {
        if (!SpotManager.Instance.FirstEnter) { return; }

        GameManager.Instance.UserData.GoTo(targetScene, targetSpot);

        ComponentPool.Components.Reset();
        SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
}
