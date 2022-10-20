using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportSpot : InteractSpot, ITeleportSpotHandler
{
    private string sceneName;

    public string SceneName
    { 
        get 
        {
            if (string.IsNullOrEmpty(sceneName)) { sceneName = SceneManager.GetActiveScene().name; }

            return sceneName;
        }

        private set 
        {
            sceneName = value;
        }
    }

    public bool isOn { get; set; }

    protected override void Start()
    {
        base.Start();

        var manager = TeleportManager.Instance;

        this.isOn = manager.SpotIsOn(this);

        InteractCallBack = delegate ()
        {
            manager.UIState(true);

            manager.UpdateSpotsList(this);
        };
    }
}
