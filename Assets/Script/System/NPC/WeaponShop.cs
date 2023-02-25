using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using InventorySystem;

namespace RoleSystem
{
    public class WeaponShop : MonoBehaviour, IInteractable
    {
        public IUpdateUIHandler UpdateUI { get; private set; }

        private IPageStateHandler PageState => this.UpdateUI as IPageStateHandler;

        private void Start()
        {
            this.UpdateUI = CustomContainer.GetContent<WeaponShopUI>("Shop");
        }

        #region IInteractable

        [SerializeField]
        private bool instance;

        public bool Instance => this.instance;

        public void Interact()
        {
            if (!this.PageState.PageState) { this.UpdateUI.UIState(true); }
        }

        #endregion
    }
}