using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;

namespace InventorySystem
{
    public class ShopFuncUI : MonoBehaviour
    {
        [SerializeField]
        private Transform shopFuncUI;
        [SerializeField]
        private ItemShopUI itemShopUI;
        [SerializeField]
        private Button perchase;
        [SerializeField]
        private Button sell;
        [SerializeField]
        private Button cancel;

        private void Start()
        {
            SetButtons();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!GameManager.Instance.shopMode && KeyManager.GetAxis("Vertical") != 0) { FuncWindowState(true); }

                if (GameManager.Instance.shopMode && KeyManager.GetKeyDown("Jump")) { FuncWindowState(false); }
            }
        }

        private void FuncWindowState(bool state) 
        {
            shopFuncUI.gameObject.SetActive(state);

            GameManager.Instance.shopMode = state;
        }

        private void PerchaseClicked() 
        {
            itemShopUI.ShopWindowState(true, ItemShopUI.EShopType.Perchase);
            shopFuncUI.gameObject.SetActive(false);
        }

        private void SellClicked()
        {
            itemShopUI.ShopWindowState(true, ItemShopUI.EShopType.Sell);
            shopFuncUI.gameObject.SetActive(false);
        }

        private void CancelClicked()
        {
            shopFuncUI.gameObject.SetActive(false);
        }

        private void SetButtons()
        {
            if (perchase) { perchase.onClick.AddListener(PerchaseClicked); }
            if (sell) { sell.onClick.AddListener(SellClicked); }
            if (cancel) { cancel.onClick.AddListener(CancelClicked); }
        }
    }
}