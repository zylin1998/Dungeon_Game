using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomInput;

public class TeleportUI : MonoBehaviour, ISlotFieldCtrlHandler<TeleportSpots.TSData>, IInputClient
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

    #region IInputClient

    [SerializeField]
    private List<IInputClient.RequireAxes> axes;

    public List<IInputClient.RequireAxes> Axes => axes;

    public bool IsCurrent { get; set; }

    private float holdFast = 0.1f;
    private float holdSlow = 0.5f;
    private float holding = 0f;

    private int count = 0;

    public void GetValue(IEnumerable<AxesValue<float>> values)
    {
        var direct = Vector2.zero;

        values.ToList().ForEach(v =>
        {
            if (v.AxesName == "Vertical" && v.Value != 0) { direct.y = v.Value; }
        });

        if (this.SlotField is INaviPanelCtrl ctrl)
        {
            if (holding == 0f)
            {
                ctrl.SelectPanel(direct);

                count++;
            }

            holding += Time.deltaTime;
        }

        if (holding >= (count <= 2 ? holdSlow : holdFast) || direct == Vector2.zero)
        {
            holding = 0;

            if (direct == Vector2.zero) { count = 0; }
        }
    }

    public void GetValue(IEnumerable<AxesValue<bool>> values)
    {
        values.ToList().ForEach(v =>
        {
            if (v.AxesName == "Attack" && v.Value) { this.UIState(false); }
        });
    }

    #endregion

    #region ISlotFieldCtrlHandler

    public ISlotFieldHandler<TeleportSpots.TSData> SlotField { get; private set; }

    public ISlotFieldCtrlHandler<TeleportSpots.TSData> Controller => this;

    public void UpdateUI(bool refresh)
    {
        if (refresh) { Controller.RefreshList(TeleportManager.TeleportSpots.SpotList); }

        UpdateList();

        if (refresh)
        {
            var navi = SlotField.Content.GetComponent<INavigationCtrl>();

            navi.GetSelectables(n => (n as ISlotHandler<TeleportSpots.TSData>).Interact);
            navi.SetNavigation();
        }
    }

    public void UpdateList()
    {
        SlotField.Slots.ForEach(slot => 
        {
            if (slot is TeleportSlot teleportSlot)
            {
                teleportSlot.UIState(teleportSlot.Interact && teleportSlot.Content.scene != SceneManager.GetActiveScene().name);
            }
        });
    }

    public void SetSlot(ISlotHandler<TeleportSpots.TSData> slot)
    {
        slot.Button.onClick.AddListener(() => { TeleportManager.Instance.Teleport(slot.Content); });
    }

    public void UIState(bool state) 
    {
        this.SlotField.UIState(state);
        
        KeyManager.SetCurrent(this, state);
    }

    #endregion

    private void OnDestroy()
    {
        CustomContainer.RemoveContent(this, "Teleport");
    }
}
