using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour
{
    #region Singleton

    public static TeleportManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance) { return; }

        Instance = this;

        TeleportSpots = GameManager.Instance.UserData.GetPackables<TeleportSpots>();
    }

    #endregion

    public TeleportSpots TeleportSpots { get; private set; }
    
    public ISlotFieldCtrlHandler<TeleportSlot, TeleportSpots.TSData> TeleportUI { get; private set; }

    public Action<bool> SpotOpenCallBack = delegate { };

    private void Start()
    {
        TeleportUI = CustomContainer.GetContent<TeleportUI>("Teleport");
    }

    public void UIState(bool state) 
    {
        TeleportUI.SlotField.UIState(state);
    }

    public bool SpotIsOn(ITeleportSpotHandler spot) 
    {
        return TeleportSpots.SpotList.Exists(data => data.scene.Equals(spot.SceneName));
    }

    public void UpdateSpotsList(ITeleportSpotHandler spot)
    {
        if (!spot.isOn)
        {
            spot.isOn = true;

            TeleportSpots.AddSpot(spot);
        }

        SpotOpenCallBack.Invoke(true);
    }

    public void Teleport(TeleportSpots.TSData spot) 
    {
        if (spot.scene == SceneManager.GetActiveScene().name) { return; }

        TeleportSpots.SetLocate(spot);

        SceneManager.LoadScene(spot.scene, LoadSceneMode.Single);
    }
}
