using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeleportSlot : MonoBehaviour, ISlotHandler<TeleportSpots.TSData>, INaviSelectable, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    #region ISlotHandler

    public TeleportSpots.TSData Content { get; private set; }

    public Button Button => this.GetComponent<Button>();
    public Selectable Selectable => this.Button;
    private Text Title => this.Button?.GetComponentInChildren<Text>();

    public Action OnSelectCallBack { get; set; }
    public Action OnExitCallBack { get; set; }

    public bool Interact => this.Content != null;

    public void SetSlot(TeleportSpots.TSData item) 
    {
        this.Content = item;

        Title.text = Content.spot;
    }

    public void ClearSlot()
    {
        this.Content = null;
        this.Title.text = string.Empty;
    }

    public void CheckSlot() 
    {
        
    }

    public void UpdateSlot() 
    {
        Title.text = this.Content.spot;
    }

    public void UIState(bool state) 
    {
        this.gameObject.SetActive(state);
    }

    #endregion

    #region INaviSelectable

    public INavigationCtrl Belonging { get; set; }

    public Vector2 ID { get; set; }

    public bool IsSelected { get; private set; }

    public void Select()
    {
        this.IsSelected = true;

        if (Belonging != null) { Belonging.SetFinal(this); }
    }

    public void DeSelect()
    {
        this.IsSelected = false;
    }

    #endregion

    #region PointerEvents

    public void OnPointerEnter(PointerEventData baseEvent) 
    {
        this.OnSelect(baseEvent);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (OnSelectCallBack != null) { OnSelectCallBack.Invoke(); }

        this.Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExitCallBack != null) { OnExitCallBack.Invoke(); }

        this.DeSelect();
    }

    #endregion
}
