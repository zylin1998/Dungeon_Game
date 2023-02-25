using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCollect : NaviPanel, INavigationCtrl, IContainerGroup
{
    protected virtual void Awake()
    {
        CustomContainer.AddContent(this, this.GroupName);

        this.GetSelectables();
    }

    protected virtual void Start()
    {
        this.SetNavigation();
    }

    #region IContainerGroup

    [SerializeField]
    private string groupName;

    public string GroupName => this.groupName;

    #endregion

    #region INavigationCtrl

    [SerializeField]
    private INavigationCtrl.ENaviDirect direct;
    [SerializeField]
    private float row;
    [SerializeField]
    private float column;
    [SerializeField]
    private bool wrapAround;

    public INavigationCtrl.ENaviDirect Direct => this.direct;
    public float Row => this.row;
    public float Column => this.column;
    public bool WrapAround => this.wrapAround;
    public List<INaviSelectable> Selectables { get; private set; }

    public INaviSelectable FinalSelect { get; private set; }

    public void GetSelectables()
    {
        this.Selectables = this.GetComponentsInChildren<INaviSelectable>().ToList();

        this.Setting();
    }

    public void GetSelectables(Func<INaviSelectable, bool> predicate)
    {
        this.Selectables = this.GetComponentsInChildren<INaviSelectable>().Where(navi => predicate.Invoke(navi)).ToList();

        this.Setting();
    }

    public void SetNavigation()
    {
        if (this.direct == INavigationCtrl.ENaviDirect.None) { return; }

        if (Selectables.Any())
        {
            int r = 1, c = 1;

            this.Selectables.ForEach(s =>
            {
                if (r > this.row || c > this.column) { return; }

                s.ID = new Vector2(r, c);

                if (r < this.row) { r++; }

                if (r >= this.row) { c++; }
            });

            this.Selectables.ForEach(s =>
            {
                var n = new Navigation();

                n.mode = Navigation.Mode.Explicit;

                if (this.direct == INavigationCtrl.ENaviDirect.Vertical || this.direct == INavigationCtrl.ENaviDirect.Flexible)
                {
                    n.selectOnUp = this.Selectables.Find(f => f.ID == (s.ID - Vector2.up))?.Selectable;
                    n.selectOnDown = this.Selectables.Find(f => f.ID == (s.ID - Vector2.down))?.Selectable;
                }

                if (this.direct == INavigationCtrl.ENaviDirect.Horizontal || this.direct == INavigationCtrl.ENaviDirect.Flexible)
                {
                    n.selectOnLeft = this.Selectables.Find(f => f.ID == (s.ID + Vector2.left))?.Selectable;
                    n.selectOnRight = this.Selectables.Find(f => f.ID == (s.ID + Vector2.right))?.Selectable;
                }

                s.Selectable.navigation = n;
            });
        }
    }

    public void SetFinal(INaviSelectable final)
    {
        this.FinalSelect = final;
    }

    public INaviSelectable FindSelectables(Vector2 direct)
    {
        return this.FinalSelect.FindNavi(direct);
    }

    public void Ctrl(Vector2 direct)
    {
        var id = this.FinalSelect.ID;

        if (id.x == 1 && direct.x == -1) { this.FindPanel(Vector2.left); }

        else if (id.x == this.row && direct.x == 1) { this.FindPanel(Vector2.right); }

        else if (id.y == 1 && direct.y == 1) { this.FindPanel(Vector2.up); }

        else if (id.y == this.Column && direct.y == -1) { this.FindPanel(Vector2.down); }

        else { this.FindSelectables(direct).Select(); }
    }

    public void Setting()
    {
        this.Selectables.ForEach(s => s.Belonging = this);

        if (this.direct == INavigationCtrl.ENaviDirect.Horizontal)
        {
            this.row = this.Selectables.Count;
            this.column = 1;
        }

        if (this.direct == INavigationCtrl.ENaviDirect.Vertical)
        {
            this.row = 1;
            this.column = this.Selectables.Count;
        }
    }

    #endregion
}