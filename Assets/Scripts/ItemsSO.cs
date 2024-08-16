using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemsSO : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;
    public Sprite sprite;
    public int maxStack;
    public int chance;
    public Type type = new Type();
    public int statChange;
    public enum Type
    {
        Healing,
        DefenseHead,
        DefenseBody,
        DefenseLegs,

    }

    public void UseItem()
    {
        if(type == Type.Healing)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().playerSO.AddHealth(statChange);
        }
        if(type == Type.DefenseHead || type == Type.DefenseBody || type == Type.DefenseLegs)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().playerSO.EquipArmor(statChange, type);
        }
    }
    public void UnequipArmor()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().playerSO.UnequipArmor(type);
    }
}
