using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISlotFieldHandler<TSlot>
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
    public List<TSlot> Slots { get; set; }
    public Action UICloseCallBack { get; set; }

    public void Initialized()
    {
        Slots = CreateList(Detail.defaultCount);
    }

    public TSlot CreateSlot()
    {
        var newObject = GameObject.Instantiate(Detail.slotPrefab, Content);

        var slot = newObject.GetComponent<TSlot>();

        return slot;
    }

    public List<TSlot> CreateList(int count)
    {
        var slots = new List<TSlot>();

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

    public void ExpandList(int count, Action<TSlot> setting) 
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

public interface ISlotFieldCtrlHandler<TSlot, TItem> where TSlot : ISlotHandler<TItem>
{
    public ISlotFieldHandler<TSlot> SlotField { get; }

    public ISlotFieldCtrlHandler<TSlot, TItem> Controller { get; }

    public void UpdateUI(bool refresh);

    public void UpdateList();

    public void SetSlot(TSlot slot);

    public void RefreshList(List<TItem> items)
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
}

public interface IDetailPanelHandler <TItem>
{
    public void SetDetail(TItem item);

    public void Clear();
}

public interface ISlotHandler <TItem>
{
    public Button Button { get; }

    public bool Interact { get; }

    public TItem Item { get; }

    public Action OnSelectCallBack { get; set; }
    public Action OnExitCallBack { get; set; }

    public void SetSlot(TItem item);

    public void ClearSlot();

    public void CheckSlot();

    public void UpdateSlot();
}

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

public interface IUpdateUIHandler 
{
    public Button Cancel { get; }

    public void UIState(bool state);

    public void UpdateUI();
}