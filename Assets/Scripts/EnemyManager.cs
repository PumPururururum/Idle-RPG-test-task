using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemySO currentEnemy;
    public EnemySO[] enemySOs;
    private PlayerManager player;
    public bool canAttack;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public HealthBar healthBar;
    public Timer timer;
    private bool timerOn;
    private float timeLeft;
    public bool isDead;
    private bool veryDead = false;

    void Start()
    {
        

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
       
       
        
    }
    private void FixedUpdate()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timer.SetTime(timeLeft);
            }
        }
        healthBar.SetHealth(currentEnemy.GetHealth(), currentEnemy.maxHealth);
        if (currentEnemy.GetHealth() <= 0 && !veryDead)
        {
            Death();
        }
    }

    // Update is called once per frame
    public void StartAttack()
    {
        timer.gameObject.SetActive(true);
        healthBar.gameObject.SetActive(true);
        NewEnemy();
        StartCoroutine(PrepareAttack());
    }
    public void StopAttack()
    {
        timerOn = false;
        StopAllCoroutines();
        timer.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
    }

    public IEnumerator PrepareAttack()
    {
        //Debug.Log("enemy preparing");
        SetTimer(currentEnemy.prepareTime);
        timerOn = true;
        timer.ChangeColorPrepare();
        WaitForSeconds wait = new WaitForSeconds(currentEnemy.prepareTime);
        yield return wait;
        StartCoroutine(Attack());
    }
    public IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("ZombieAttack");
        SetTimer(currentEnemy.attackTime);
        timer.ChangeColorAttack();
       // Debug.Log("enemy attacking");
        WaitForSeconds wait = new WaitForSeconds(currentEnemy.attackTime);
        yield return wait;
        player.playerSO.TakeDamage(currentEnemy.attackDamage);
        StartCoroutine(PrepareAttack());
    }
    private void SetTimer(float time)
    {
        timer.SetMaxTime(time);
        timer.SetTime(time);
        timeLeft = time;
    }

    private void Death()
    {
        isDead = true;
        veryDead = true;
        StopAttack();
        animator.SetBool("isDead", true);
    }

    public void NewEnemy()
    {
        veryDead = false;
        float random = Random.value;
        animator.SetBool("isDead", false);
        if (random <= 0.6f)
            currentEnemy = enemySOs[0];
        else
        if (random <= 0.9f)
            currentEnemy = enemySOs[1];
        else
        if (random <= 1f)
            currentEnemy = enemySOs[2];
        Debug.Log(currentEnemy.enemyName);
        currentEnemy.SetHealthToMax();
        healthBar.SetMaxHealth(currentEnemy.maxHealth);
        currentEnemy.spriteColor.a = 255;
        spriteRenderer.color = currentEnemy.spriteColor;
    }
}
