using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoleSystem;

namespace InventorySystem
{
    public class WeaponShopUI : MonoBehaviour, IPageStateHandler, IUpdateUIHandler
    {
        [SerializeField]
        private CharacterDetailAsset detailAsset;
        [SerializeField]
        private Text cost;
        [SerializeField]
        private Button upgrade;
        [SerializeField]
        private Button cancel;
        [SerializeField]
        private int basicCost;
        
        private WeaponDetail Weapon { get; set; }

        private int Paid { get; set; }

        #region IPageStateHandler

        public bool PageState { get; private set; }

        #endregion

        #region IUpdateUIHandler

        public Button Cancel => cancel;

        public void UpdateUI()
        {
            upgrade.interactable = false;

            if (Weapon != null)
            {
                this.Paid = System.Convert.ToInt32(basicCost * Weapon.Rate);

                upgrade.interactable = Inventory.Instance.IsEnough(Paid) && !Weapon.IsMax;
            }

            if (cost)
            {
                cost.text = Weapon.IsMax ? "µ¥¯Å¤wº¡" : string.Format("{0}G", Paid);
            }
        }

        public void UIState(bool state)
        {
            this.gameObject.SetActive(state);

            this.PageState = state;

            if (state) { UpdateUI(); }
        }

        #endregion

        private void Awake()
        {
            CustomContainer.AddContent(this, "Shop");
        }

        private void Start()
        {
            Weapon = detailAsset.SingleWeapon;

            if (upgrade) { upgrade.onClick.AddListener(UpgradeClick); }

            if (cancel) { cancel.onClick.AddListener(CancelClick); }

            GameManager.Instance.AddPage(this);

            UIState(false);
        }

        private void Update()
        {
            if (this.PageState && CustomInput.KeyManager.GetKeyDown("Attack")) { this.UIState(false); }
        }

        #region Button Events

        private void UpgradeClick() 
        {
            var paid = Inventory.Instance.DecreaseGold(this.Paid);

            if (paid)
            {
                Weapon.Upgrade();

                UpdateUI();
            }
        }

        private void CancelClick() 
        {
            UIState(false);
        }

        #endregion

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Shop");
        }
    }
}