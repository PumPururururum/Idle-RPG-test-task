using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    public int maxHealth;
    private int currentHealth;
    public int defenseHead;
    public int defenseBody;
    public int defenseLegs;
    public int additionalDefense;
    public float prepareTime;
    public float critChance;
    public int critDamage;

    //private bool headEquiped;
    //private bool bodyEquiped;
    //private bool legsEquiped;

    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
    }
    public void SetStartingStats(int defense, int health, float time)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        defenseLegs = defense;
        defenseHead = defense;
        defenseBody = defense;
        additionalDefense = defense;
        prepareTime = time;
    }
    public void TakeDamage(int damage)
    {
        int totalDefense = defenseHead + defenseBody + defenseLegs + additionalDefense;
        if (totalDefense < damage) 
        currentHealth -= damage - totalDefense;
    }

    public void AddHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public int GetHealth() 
    {
        return currentHealth;
    
    }
    public void EquipArmor(int defense, ItemsSO.Type type)
    {
        if (type == ItemsSO.Type.DefenseHead)
        {
            defenseHead = defense;
        }
        if (type  == ItemsSO.Type.DefenseBody) 
        {
            defenseBody = defense; 
        }
        if (type == ItemsSO.Type.DefenseLegs)
        {
            defenseLegs = defense;
        }
    }
    public void UnequipArmor(ItemsSO.Type type)
    {
        if (type == ItemsSO.Type.DefenseHead)
        {
            defenseHead = 0;
        }
        if (type == ItemsSO.Type.DefenseBody)
        {
            defenseBody = 0;
        }
        if (type == ItemsSO.Type.DefenseLegs)
        {
            defenseLegs = 0;
        }
    }
}
