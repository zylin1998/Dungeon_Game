using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CategorySelection : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private string category;
    [SerializeField]
    private Color UnSelectColor;
    [SerializeField]
    private Color OnSelectColor;
    [SerializeField]
    private Selectable selectable;
    public ICategoryHandler categoryHandler { private get; set; }

    public string Category => category;

    private void Awake()
    {
        selectable = this.GetComponent<Selectable>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetColor(true);

        categoryHandler.SelectCategory(category);
    }

    public void Select()
    {
        selectable.Select();
    }

    public void DeSelect() 
    {
        SetColor(false);
    }

    private void SetColor(bool state) 
    {
        ColorBlock cb = selectable.colors;
        cb.normalColor = state ? OnSelectColor : UnSelectColor;
        selectable.colors = cb;
    }
}
