using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public TMP_Text popUp;
    public bool AddItem(ItemsSO item)
    {
        foreach (ItemSlot slot in itemSlots)
        {
            
            if (slot.isFull == true && slot.item == item && slot.quantity < item.maxStack)
            {
                slot.AddQuantity();
                return true;
            }
            
        }
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.isFull == false)
            {
                slot.AddItem(item);

                return true;
            }
        }
        return false;
    }

    public bool AddItem(WeaponSO weapon)
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.isFull == false)
            {
                slot.AddItem(weapon);
                return true;
            }
        }
        return false;
    }

    public void DesselectAllSlots()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            slot.itemSelected = false;
        }
    }

    public void UseItem()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.itemSelected && slot.isFull)
            {
                if (!slot.isWeapon && slot.item.type != ItemsSO.Type.Healing) 
                {
                    foreach (ItemSlot slot2 in itemSlots)
                    {
                        if (!slot2.isWeapon && slot2.equiped && slot.item.type == slot2.item.type)
                            slot2.Unequip();

                    }
                    slot.UseItem();
                }
                else if (!slot.isWeapon && slot.item.type == ItemsSO.Type.Healing)
                {
                    slot.UseItem();
                    GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("Healing");
                }
                else
                if (slot.isWeapon && !GameObject.Find("BattleManager").GetComponent<BattleManager>().isBattle)
                {
                    foreach (ItemSlot slot2 in itemSlots)
                    {
                        if (slot2.isWeapon && slot2.equiped && slot.weapon.weaponType == slot2.weapon.weaponType)
                            slot2.Unequip();
                    }
                    slot.UseItem();
                }
                else if (GameObject.Find("BattleManager").GetComponent<BattleManager>().isBattle)
                {
                    Debug.Log("can't equip weapon during battle");
                    StartCoroutine(turnOnPopUp());
                }
                
            }
        }
    }
    public void UseEveryItem()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.isFull)
                slot.UseItem();
        }
    }

    public void DropItem()
    {
        foreach (ItemSlot slot in itemSlots)
        {
            if (slot.itemSelected)
            {
                if (slot.equiped && !slot.isWeapon)
                {
                    slot.EmptySlot();
                    slot.Unequip();
                }
                else
                if (slot.equiped && slot.isWeapon)
                {
                    Debug.Log("cant drop equiped waepon");
                    StartCoroutine (turnOnPopUp());
                    popUp.text = "Нельзя выбросить экипированное оружие";
                }
                else
                    slot.EmptySlot();
            }
        }
    }
    IEnumerator turnOnPopUp()
    {
        popUp.enabled = true;
        yield return new WaitForSeconds(3);
        popUp.enabled = false;
    }
}
