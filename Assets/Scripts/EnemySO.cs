using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    public string enemyName;

    public int maxHealth;
    public int currentHealth;
    public int defense;
    public int attackDamage;
    public float spawnChance;
    public float prepareTime;
    public float attackTime;
    public int expForKill;

    public Color spriteColor;
    public void TakeDamage(int damage)
    {
        currentHealth -= damage - defense;
    }
    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
    }
    public int GetHealth()
    {
        return currentHealth;

    }
}
