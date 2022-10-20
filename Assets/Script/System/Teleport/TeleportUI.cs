using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomInput;

public class TeleportUI : MonoBehaviour, ISlotFieldCtrlHandler<TeleportSlot, TeleportSpots.TSData>
{
    private TeleportManager TeleportManager => TeleportManager.Instance;

    private void Awake()
    {
        CustomContainer.AddContent(this, "Teleport");
    }

    private void Start()
    {
        SlotField = CustomContainer.GetContent<TeleportSlots>("Teleport");
        SlotField.Initialized();

        TeleportManager.SpotOpenCallBack += UpdateUI;

        if (TeleportManager.TeleportSpots.SpotList.Any()) { UpdateUI(true); }
    }

    private void Update()
    {
        var isOpen = (SlotField as TeleportSlots).gameObject.activeSelf;

        if (isOpen && KeyManager.GetKeyDown("Attack"))
        {
            SlotField.UIState(false);
        }
    }

    #region ISlotFieldCtrlHandler

    public ISlotFieldHandler<TeleportSlot> SlotField { get; private set; }

    public ISlotFieldCtrlHandler<TeleportSlot, TeleportSpots.TSData> Controller => this;

    public void UpdateUI(bool refresh)
    {
        if (refresh) { Controller.RefreshList(TeleportManager.TeleportSpots.SpotList); }

        UpdateList();
    }

    public void UpdateList()
    {
        SlotField.Slots.ForEach(slot => 
        {
            slot.CheckSlot();

            slot.gameObject.SetActive(slot.Interact && slot.Item.scene != SceneManager.GetActiveScene().name); 
        });
    }

    public void SetSlot(TeleportSlot teleportSlot)
    {
        teleportSlot.Button.onClick.AddListener(() => { TeleportManager.Instance.Teleport(teleportSlot.Item); });
    }

    #endregion

    private void OnDestroy()
    {
        CustomContainer.RemoveContent(this, "Teleport");
    }
}
