using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSpot : InteractSpot, IDocumentSpotHandler
{
    public string SceneName => SceneManager.GetActiveScene().name;

    protected override void Awake()
    {
        base.Awake();

        InteractCallBack = Documental;
    }

    public void Documental() 
    {
        TeleportManager.Instance.TeleportSpots.SaveLocate(this);

        GameManager.Instance.SaveUserData();
    }
}
