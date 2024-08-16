using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;


[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    [TextArea]
    public string description;
    public WeaponType weaponType = new WeaponType();
    public int damage;
    public float attackSpeed;
    public int chance;
    public Sprite sprite;

    public enum WeaponType
    {
        Melee,
        Ranged
    }
    public void UseWeapon()
    {
        if (weaponType == WeaponType.Melee)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().weapons[0] = this ;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().weapons[1] = this;
        }
        
    }

}
