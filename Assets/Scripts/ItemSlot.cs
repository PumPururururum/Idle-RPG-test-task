using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemsSO item;
    public WeaponSO weapon;
    public int quantity;
    public bool isFull;
    
    public bool equiped;
    public bool isWeapon;

    public TMP_Text quantityText;
    public Image itemImage;
    public Image equipedIndicator;
    public bool itemSelected;
    public Sprite emptySprite;
    

    public Image descriptionImage;
    public TMP_Text descriptionText;
    public TMP_Text statText;
    public TMP_Text nameText;
    public Button buttonUse;
    public Button buttonDrop;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
    }

    public void AddItem(ItemsSO item)
    {
        this.item = item;
        isFull = true;
        isWeapon = false;
        AddQuantity();
        itemImage.sprite = item.sprite;

    }
    public void AddItem(WeaponSO weapon)
    {
        this.weapon = weapon;
        isFull = true;
        isWeapon = true;
        AddQuantity();
        itemImage.sprite = weapon.sprite;

    }
    public void AddQuantity()
    {
        quantity++;
        if (quantity > 1)
            quantityText.enabled = true;
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFull)
        {
            if (!isWeapon)
            {
                descriptionImage.sprite = item.sprite;
                descriptionText.text = item.description;
                nameText.text = item.itemName;
                if(item.type == ItemsSO.Type.Healing)
                {
                    statText.text = "Лечение: " + item.statChange;
                }
                else
                    statText.text = "Защита: " + item.statChange;

            }
            else
            {
                descriptionImage.sprite = weapon.sprite;
                descriptionText.text = weapon.description;
                nameText.text = weapon.weaponName;
                statText.text = "Урон: " + weapon.damage;
            }
            
            inventoryManager.DesselectAllSlots();
            itemSelected = true;
            descriptionImage.enabled = true;
            buttonUse.interactable = true;
            buttonDrop.interactable = true;
        }
        else
        {
            itemSelected = true;
        }
    }

    public void UseItem()
    {
        if (!isWeapon)
        {

            item.UseItem();
            if (item.type == ItemsSO.Type.Healing)
            {
                quantity--;
                quantityText.text = quantity.ToString(); 
                if (quantity <= 0)
                    EmptySlot();

            }
            else
            {
                //Debug.Log("equiped");
                equiped = true;
                equipedIndicator.enabled = true;
            
            }
        }
        else
        {
            weapon.UseWeapon();
            equiped = true;
            equipedIndicator.enabled = true;
        }
    }

    public void EmptySlot()
    {
        isFull = false;
        isWeapon = false;
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        descriptionImage.enabled = false;
        descriptionText.text = "";
        nameText.text = "";
        buttonUse.interactable = false;
        buttonDrop.interactable = false;
        quantity = 0;
    }
    public void Unequip()
    {
        equiped = false;
        equipedIndicator.enabled = false;
        if(!isWeapon)
            if (item.type != ItemsSO.Type.Healing)
                item.UnequipArmor();
    }
    public void Equip()
    {
        equiped = true;
        equipedIndicator.enabled = true;
    }
}
