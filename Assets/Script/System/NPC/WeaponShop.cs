using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using InventorySystem;

namespace RoleSystem
{
    public class WeaponShop : MonoBehaviour
    {
        public IUpdateUIHandler UpdateUI { get; private set; }

        private IPageStateHandler PageState => this.UpdateUI as IPageStateHandler;

        private void Start()
        {
            this.UpdateUI = CustomContainer.GetContent<WeaponShopUI>("Shop");
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }

            if (!this.PageState.PageState && KeyManager.GetAxis("Vertical") != 0) { this.UpdateUI.UIState(true); }
        }
    }
}