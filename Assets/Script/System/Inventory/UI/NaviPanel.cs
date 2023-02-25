using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NaviPanel : MonoBehaviour, INaviPanel
{
    [SerializeField]
    protected bool initial;
    [SerializeField]
    protected INaviPanel.Navi naviPanels;

    public INaviPanel.Navi NaviPanels => naviPanels;

    public bool Initial => this.initial;

    public virtual INaviPanel FindPanel(Vector2 direct) 
    {
        return this.naviPanels.FindPanel(direct);
    }
}
