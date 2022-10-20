using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportSlots : MonoBehaviour, ISlotFieldHandler<TeleportSlot>, IPageStateHandler
{
    #region ISlotFieldHandler

    [SerializeField]
    private Transform content;
    [SerializeField]
    private Button cancel;
    [SerializeField]
    ISlotFieldHandler<TeleportSlot>.SlotDetail detail;

    public Transform Content => content;
    public Button Cancel => cancel;

    public ISlotFieldHandler<TeleportSlot>.SlotDetail Detail => detail;

    public List<TeleportSlot> Slots { get; set; }
    public Action UICloseCallBack { get; set; }

    public void UIState(bool state) 
    {
        if(!state && UICloseCallBack != null) { UICloseCallBack.Invoke(); }

        this.PageState = state;

        this.gameObject.SetActive(state);
    }

    #endregion

    #region IPageStateHandler

    public bool PageState { get; private set; }

    #endregion

    private void Awake()
    {
        CustomContainer.AddContent(this, "Teleport");
    }

    private void Start()
    {
        GameManager.Instance.AddPage(this);

        cancel.onClick.AddListener(delegate { UIState(false); });

        UIState(false);
    }

    private void OnDestroy()
    {
        CustomContainer.RemoveContent(this, "Teleport");
    }
}
