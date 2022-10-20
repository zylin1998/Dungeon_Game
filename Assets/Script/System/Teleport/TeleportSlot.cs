using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportSlot : MonoBehaviour, ISlotHandler<TeleportSpots.TSData>
{
    public TeleportSpots.TSData Item { get; private set; }

    public bool Interact { get; private set; }

    public Button Button => this.GetComponent<Button>();
    private Text Title => this.Button?.GetComponentInChildren<Text>();

    public Action OnSelectCallBack { get; set; }
    public Action OnExitCallBack { get; set; }

    private void Start()
    {
        if (Button)
        {
            Button.onClick.AddListener(() => TeleportManager.Instance.Teleport(Item));
        }
    }

    public void SetSlot(TeleportSpots.TSData item) 
    {
        this.Item = item;

        Title.text = Item.spot;
    }

    public void ClearSlot()
    {
        this.Item = null;
        this.Title.text = string.Empty;
    }

    public void CheckSlot() 
    {
        Interact = Item != null;
    }

    public void UpdateSlot() 
    {
        Title.text = this.Item.spot;
    }
}
