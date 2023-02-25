using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CategorySelection : MonoBehaviour, ISelectHandler, IColoredOnSelect, INaviSelectable
{
    [SerializeField]
    private string category;
    
    public string Category => this.category;

    public ICategoryHandler categoryHandler { get; set; }


    private void Awake()
    {
        this.Selectable = this.GetComponent<Selectable>();
    }
    
    #region INaviSelectables

    public Selectable Selectable { get; private set; }

    public bool IsSelected { get; private set; }

    public INavigationCtrl Belonging { get; set; }

    public Vector2 ID { get; set; }

    #endregion

    #region IColoredOnSelect

    [SerializeField]
    private IColoredOnSelect.SelectColor selectedColor;

    public IColoredOnSelect.SelectColor SelectedColor => this.selectedColor;

    public void Select()
    {
        this.Selectable.Select();

        UIState(true);

        if (this.categoryHandler != null) { this.categoryHandler.SelectCategory(category); }

        if (this.Belonging != null) { this.Belonging.SetFinal(this); }
    }

    public void DeSelect() 
    {
        UIState(false);
    }

    #endregion

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    private void UIState(bool state) 
    {
        this.IsSelected = state;

        this.Selectable.colors = selectedColor.ChangeNormal(this.Selectable.colors, state);
    }
}
