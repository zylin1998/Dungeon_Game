using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportSlots : MonoBehaviour, ISlotFieldHandler<TeleportSpots.TSData>, IPageStateHandler, INaviPanelCtrl
{
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

    #region ISlotFieldHandler

    [SerializeField]
    private Transform content;
    [SerializeField]
    private Button cancel;
    [SerializeField]
    ISlotFieldHandler<TeleportSpots.TSData>.SlotDetail detail;

    public Transform Content => content;
    public Button Cancel => cancel;

    public ISlotFieldHandler<TeleportSpots.TSData>.SlotDetail Detail => detail;

    public List<ISlotHandler<TeleportSpots.TSData>> Slots { get; set; }
    public Action UICloseCallBack { get; set; }

    public void UIState(bool state) 
    {
        if(!state && UICloseCallBack != null) { UICloseCallBack.Invoke(); }

        if (state) { this.PanelInitialize(); }

        this.PageState = state;

        this.gameObject.SetActive(state);
    }

    #endregion

    #region IPageStateHandler

    public bool PageState { get; private set; }

    #endregion

    #region INaviPanelCtrl

    public List<INaviPanel> NaviPanels { get; private set; }

    public INaviPanel CurrentPanel { get; private set; }

    public INaviPanel Initial => this.NaviPanels.Find(n => n.Initial);

    public void GetNaviPanels()
    {
        this.NaviPanels = CustomContainer.GetContents<SelectableCollect>("Teleport").ConvertAll(c => c as INaviPanel);
    }

    public void SelectPanel(Vector2 direct)
    {
        var selDir = new Vector2(0, direct.y);

        if (selDir != Vector2.zero)
        {
            var select = (this.CurrentPanel as SelectableCollect)?.FinalSelect?.FindNavi(selDir);

            this.INaviSelectableSelect(select);
        }
    }

    public void PanelInitialize()
    {
        if (this.Initial is SelectableCollect initial) { initial.Selectables.First()?.Select(); }
    }

    private void INaviSelectableSelect(INaviSelectable selectable)
    {
        if (selectable == null) { return; }

        (selectable as UnityEngine.EventSystems.ISelectHandler).OnSelect(null);

        selectable.Selectable.Select();
    }

    #endregion
}
