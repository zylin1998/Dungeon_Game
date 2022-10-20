using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;

namespace InventorySystem
{
    public class ShopFunc : MonoBehaviour, IPageStateHandler, IUpdateUIHandler
    {
        [SerializeField]
        private Button perchase;
        [SerializeField]
        private Button sell;
        [SerializeField]
        private Button cancel;

        private ShopManager ShopManager { get; set; }

        #region IPageStateHandler

        public bool PageState { get; private set; }

        #endregion

        #region IUpdateUIHandler

        public Button Cancel => cancel;

        public void UpdateUI() 
        {
            var hasShop = ShopManager != null;

            perchase.interactable = hasShop;
            sell.interactable = hasShop;
        }

        public void UIState(bool state)
        {
            this.gameObject.SetActive(state);

            this.PageState = state;
        }

        #endregion

        private void Awake()
        {
            CustomContainer.AddContent(this, "Shop");
        }

        private void Start()
        {
            ShopManager = ShopManager.Instance;

            GameManager.Instance.AddPage(this);

            SetButtons();

            UpdateUI();

            UIState(false);
        }

        private void Update()
        {
            if (this.PageState && CustomInput.KeyManager.GetKeyDown("Attack")) { this.UIState(false); }
        }

        #region Button Events

        private void PerchaseClicked() 
        {
            ShopManager.Instance.OpenShop(EShopType.Perchase);
            this.UIState(false);
        }

        private void SellClicked()
        {
            ShopManager.Instance.OpenShop(EShopType.Sell);
            this.UIState(false);
        }

        private void CancelClicked()
        {
            this.UIState(false);
        }

        private void SetButtons()
        {
            if (perchase) { perchase.onClick.AddListener(PerchaseClicked); }
            if (sell) { sell.onClick.AddListener(SellClicked); }
            if (cancel) { cancel.onClick.AddListener(CancelClicked); }
        }

        #endregion

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Shop");
        }
    }
}