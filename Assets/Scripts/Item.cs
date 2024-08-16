using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemsSO item;
    public WeaponSO weaponItem;
    public List<ItemsSO> items = new List<ItemsSO>();
    public List<WeaponSO> weaponItems = new List<WeaponSO>();
    private InventoryManager inventoryManager;
    private SpriteRenderer spriteRenderer;

    private bool isWeapon = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, 2);
        if (rand == 1)
        {   
            isWeapon = false;
            item = GetRandomItem();
            spriteRenderer.sprite = item.sprite;
        }
        else 
        {
            isWeapon = true;
            weaponItem = GetRandomWeapon();
            spriteRenderer.sprite = weaponItem.sprite;
        }

        inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
    }

    ItemsSO GetRandomItem()
    {
  
            int rand = Random.Range(1, 101);
            List<ItemsSO> possibleItems = new List<ItemsSO>();

            foreach (ItemsSO item in items)
            {
                if (rand <= item.chance)
                {
                    possibleItems.Add(item);
                }
            }
            if (possibleItems.Count > 0)
            {
                int minChance = 1000;
                ItemsSO droppedItem = null;
                foreach (ItemsSO item in possibleItems)
                {
                    if (item.chance < minChance)
                    {
                        minChance = item.chance;
                        droppedItem = item;
                    }
                }

                return droppedItem;
            }
            return null;

        
        
    }
    WeaponSO GetRandomWeapon()
    {
        int rand = Random.Range(1, 101);
        List<WeaponSO> possibleItems = new List<WeaponSO>();

        foreach (WeaponSO weapon in weaponItems)
        {
            if (rand <= weapon.chance)
            {
                possibleItems.Add(weapon);
            }
        }
        if (possibleItems.Count > 0)
        {
            int minChance = 1000;
            WeaponSO droppedItem = null;
            foreach (WeaponSO weapon in possibleItems)
            {
                if (weapon.chance < minChance)
                {
                    minChance = weapon.chance;
                    droppedItem = weapon;
                }
            }

            return droppedItem;
        }
        return null;

    }

    public void PickUpItem()
    {
        if (isWeapon)
            if (inventoryManager.AddItem(weaponItem))
                Destroy(gameObject);
            else Debug.Log("No room in inventory");
        if (!isWeapon)
            if (inventoryManager.AddItem(item))
                Destroy(gameObject);
            else Debug.Log("No room in inventory");
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("PickUp");
    }
}
