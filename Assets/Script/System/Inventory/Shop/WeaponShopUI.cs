using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;
using RoleSystem;

namespace InventorySystem
{
    public class WeaponShopUI : MonoBehaviour
    {
        [SerializeField]
        private CharacterDetailAsset detailAsset;
        [SerializeField]
        private Transform weaponShopUI;
        [SerializeField]
        private Text cost;
        [SerializeField]
        private Button upgrade;
        [SerializeField]
        private Button cancel;
        [SerializeField]
        private int basicCost;
        [SerializeField]
        private WeaponDetail weapon;

        private int paid;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) 
            {
                if (KeyManager.GetAxis("Vertical") != 0 && !weaponShopUI.gameObject.activeSelf) 
                {
                    weaponShopUI.gameObject.SetActive(true);

                    GameManager.Instance.shopMode = true;

                    UpdateUI();
                }

                if (KeyManager.GetKeyDown("Jump") && weaponShopUI.gameObject.activeSelf)
                {
                    weaponShopUI.gameObject.SetActive(false);

                    GameManager.Instance.shopMode = false;
                }
            }
        }

        private void Start()
        {
            weapon = detailAsset.SingleWeapon;

            if (upgrade) { upgrade.onClick.AddListener(Upgrade); }

            if (cancel) { cancel.onClick.AddListener(Cancel); }
        }

        private void Upgrade() 
        {
            Inventory.Instance.DecreaseGold(paid);

            weapon.Upgrade();

            UpdateUI();
        }

        private void Cancel() 
        {
            weaponShopUI.gameObject.SetActive(false);

            GameManager.Instance.shopMode = false;
        }

        private void UpdateUI()
        {
            upgrade.interactable = false;

            if (weapon != null)
            {
                paid = System.Convert.ToInt32(basicCost * weapon?.Rate);
            
                upgrade.interactable = Inventory.Instance.IsEnough(paid) && !weapon.IsMax;
            }

            if (cost)
            {
                cost.text = weapon.IsMax ? "µ¥¯Å¤wº¡" : string.Format("{0}G", paid);
            }
        }
    }
}