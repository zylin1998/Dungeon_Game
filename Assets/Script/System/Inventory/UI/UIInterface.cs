using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;

/// <summary>
/// Storing and controlling all target ISlots.
/// </summary>
/// <typeparam name="TContent"></typeparam>
public interface ISlotFieldHandler<TContent>
{
    [Serializable]
    public class SlotDetail
    {
        public GameObject slotPrefab;
        public int defaultCount;
        public int increaseCount;

        public override string ToString()
        {
            return $"Default: {defaultCount}, Increase: {increaseCount}";
        }
    }

    public Transform Content { get; }
    public SlotDetail Detail { get; }
    public Button Cancel { get; }
    public List<ISlotHandler<TContent>> Slots { get; set; }
    public Action UICloseCallBack { get; set; }

    public void Initialized()
    {
        Slots = CreateList(Detail.defaultCount);
    }

    public ISlotHandler<TContent> CreateSlot()
    {
        var newObject = GameObject.Instantiate(Detail.slotPrefab, Content);

        var slot = newObject.GetComponent<ISlotHandler<TContent>>();

        return slot;
    }

    public List<ISlotHandler<TContent>> CreateList(int count)
    {
        var slots = new List<ISlotHandler<TContent>>();

        for (int i = 0; i < count; i++)
        {
            slots.Add(CreateSlot());
        }

        return slots;
    }

    public void ExpandList(int count)
    {
        ExpandList(count, null);
    }

    public void ExpandList(int count, Action<ISlotHandler<TContent>> setting) 
    {
        var increase = this.Detail.increaseCount;

        var expand = count / increase + (count % increase != 0 ? 1 : 0) * increase;

        var addList = CreateList(expand);

        if (setting != null) { addList.ForEach(slot => setting.Invoke(slot)); }

        Slots.AddRange(addList);
    }

    public void UIState(bool state);

    public void UIState(bool state, Action close) 
    {
        this.UICloseCallBack = close;

        this.UIState(state);
    }
}

/// <summary>
/// Controlling slot fields.
/// </summary>
/// <typeparam name="TContent"></typeparam>
public interface ISlotFieldCtrlHandler<TContent>
{
    public ISlotFieldHandler<TContent> SlotField { get; }

    public ISlotFieldCtrlHandler<TContent> Controller { get; }

    public void UpdateUI(bool refresh);

    public void UpdateList();

    public void SetSlot(ISlotHandler<TContent> slot);

    public void RefreshList(List<TContent> items)
    {
        var count = items.Count;
        var capacity = SlotField.Slots.Count;

        if (count > capacity)
        {
            var expand = count - capacity;

            SlotField.ExpandList(expand, slot => SetSlot(slot));

            capacity = SlotField.Slots.Count;
        }

        if (count <= capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                var slot = SlotField.Slots[i];

                if (i < count) { slot.SetSlot(items[i]); }

                if (i >= count) { slot.ClearSlot(); }
            }
        }
    }

    public void UIState(bool state);
}

/// <summary>
/// Dispalying details of the content.
/// </summary>
/// <typeparam name="TContent"></typeparam>
public interface IDetailPanelHandler <TContent>
{
    public void SetDetail(TContent item);

    public void Clear();
}

/// <summary>
/// Establish a slot to store a content
/// </summary>
/// <typeparam name="TContent"></typeparam>
public interface ISlotHandler<TContent>
{
    public Button Button { get; }
    public Selectable Selectable { get; }

    public bool Interact { get; }

    public TContent Content { get; }

    public Action OnSelectCallBack { get; set; }
    public Action OnExitCallBack { get; set; }

    public void SetSlot(TContent item);

    public void ClearSlot();

    public void CheckSlot();

    public void UpdateSlot();

    public void UIState(bool state);
}

/// <summary>
/// Classification all category for contens in slotfield
/// </summary>
public interface ICategoryHandler
{
    [Serializable]
    public class Category 
    {
        [SerializeField]
        private List<string> list;

        public Category() { }

        public Category(IEnumerable<string> list) 
        {
            this.list = list.ToList();
        }

        public int GetIntByName(string category) 
        {
            return list.IndexOf(category);
        }

        public string GetNameByInt(int category) 
        {
            return list[category];
        }

        public bool Exist(string category) 
        {
            return list.Exists(c => c.Equals(category));
        }

        public bool Compare(string item, string category) 
        {
            var exist = Exist(category);

            return exist ? item == category : false;
        }

        public bool Compare(string item, IEnumerable<string> categories)
        {
            var compare = categories.ToList().ConvertAll(c => this.Compare(item, c)).Exists(e => e);

            return compare;
        }
    }

    public Category category { get; }

    public List<CategorySelection> Selections { get; }

    public void SelectFirst();

    public void CategoryInitialize();

    public void SelectCategory(string category);
}

/// <summary>
/// Basic func for UI which needs to UpdateUI.
/// </summary>
public interface IUpdateUIHandler 
{
    public Button Cancel { get; }

    public void UIState(bool state);

    public void UpdateUI();
}

/// <summary>
/// For the Selectable which color will fixed after select
/// </summary>
public interface IColoredOnSelect 
{
    [Serializable]
    public class SelectColor 
    {
        [SerializeField]
        private Color deSelect;
        [SerializeField]
        private Color onSelect;

        public SelectColor() 
        {
            this.deSelect = Color.white;
            this.onSelect = Color.white;
        }

        public SelectColor(Color deSelect, Color onSelect) 
        {
            this.deSelect = deSelect;
            this.onSelect = onSelect;
        }

        public Color GetColor(bool state) 
        {
            return state ? onSelect : deSelect;
        }

        public ColorBlock ChangeNormal(ColorBlock colorBlock, bool state) 
        {
            colorBlock.normalColor = GetColor(state);

            return colorBlock;
        }
    }

    public SelectColor SelectedColor { get; }

    public void Select();
}

public interface INaviSelectable 
{
    public Selectable Selectable { get; }

    public bool IsSelected { get; }

    public INavigationCtrl Belonging { get; set; }

    public Vector2 ID { get; set; }

    public void Select();

    public void DeSelect();

    public INaviSelectable FindNavi(Vector2 direct) 
    {
        Selectable select = null;

        if (direct.x == 1) { select = this.Selectable.FindSelectableOnRight(); }

        if (direct.x == -1) { select = this.Selectable.FindSelectableOnLeft(); }

        if (direct.y == 1) { select = this.Selectable.FindSelectableOnUp(); }

        if (direct.y == -1) { select = this.Selectable.FindSelectableOnDown(); }

        return select?.GetComponent<INaviSelectable>();
    }

}

public interface INavigationCtrl 
{
    [SerializeField]
    public enum ENaviDirect 
    {
        None,
        Horizontal,
        Vertical,
        Flexible
    }

    public ENaviDirect Direct { get; }

    public float Row { get; }

    public float Column { get; }

    public bool WrapAround { get; }

    public INaviSelectable FinalSelect { get;}

    public List<INaviSelectable> Selectables { get; }

    public void GetSelectables();

    public void GetSelectables(Func<INaviSelectable, bool> predicate);

    public void SetNavigation();

    public void SetFinal(INaviSelectable final);

    public INaviSelectable FindSelectables(Vector2 direct);
}

public interface INaviPanel
{
    [Serializable]
    public class Navi
    {
        [SerializeField]
        private NaviPanel up;
        [SerializeField]
        private NaviPanel down;
        [SerializeField]
        private NaviPanel left;
        [SerializeField]
        private NaviPanel right;

        public INaviPanel FindPanel(Vector2 direct) 
        {
            INaviPanel select = null;

            if (direct.y == 1) { select = this.up; }

            if (direct.y == -1) { select = this.down; }

            if (direct.x == 1) { select = this.right; }

            if (direct.x == -1) { select = this.left; }

            return select;
        }
    }

    public bool Initial { get; }

    public Navi NaviPanels { get; }

    public INaviPanel FindPanel(Vector2 direct);
}

public interface INaviPanelCtrl 
{
    public List<INaviPanel> NaviPanels { get; }

    public INaviPanel CurrentPanel { get; }

    public INaviPanel Initial { get; }

    public void GetNaviPanels();

    public void SelectPanel(Vector2 direct);

    public void PanelInitialize();
}