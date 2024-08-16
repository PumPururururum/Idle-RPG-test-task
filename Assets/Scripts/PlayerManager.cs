using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public PlayerSO playerSO;
    public WeaponSO[] weapons;

    public WeaponSO currentWeapon;
    public bool canAttack = false;
    private EnemyManager enemy;

    private Animator animator;
    public bool flyArrow;

    public HealthBar healthBar;
    public TMP_Text defenseText;
    public TMP_Text weaponText;
    public Timer timer;
    public bool timerOn;
    private float timeLeft;
    private float timeLeftTemp;
    private float timeMaxTemp;

    private bool attackCoroutineRunning;
    private bool prepareCoroutineRunning;

    private bool weaponChangeNext;
    private bool pausedCoroutine = false;

    IEnumerator enumeratorPrepare;
    private BattleManager battleManager;
    private SoundManager soundManager;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        playerSO.SetHealthToMax();
        playerSO.SetStartingStats(0, 100, 2);
        healthBar.SetMaxHealth(playerSO.maxHealth);
        animator = GetComponent<Animator>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();    
        foreach (WeaponSO weapon in weapons)
        {
            GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().AddItem(weapon);
        }
            GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().UseEveryItem();
    }
    private void Update()
    {
        if(!battleManager.isBattle) 
        {
            if (currentWeapon.weaponType == WeaponSO.WeaponType.Melee)
            {
                currentWeapon = weapons[0];
            }
            else
                currentWeapon = weapons[1];
        }
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
        defenseText.text = (playerSO.defenseBody + playerSO.defenseHead + playerSO.defenseLegs + playerSO.additionalDefense).ToString();
        weaponText.text = currentWeapon.weaponName;
        healthBar.SetHealth(playerSO.GetHealth(), playerSO.maxHealth);
        if (playerSO.GetHealth() <= 0)
        {
            Death();
        }
    }

    public void StartAttack()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyManager>();
        timer.gameObject.SetActive(true);
        enumeratorPrepare = PrepareAttack();
        StartCoroutine(enumeratorPrepare);
    }
    public void StopAttack()
    {
        timerOn = false;
        StopAllCoroutines();
        attackCoroutineRunning = false;
        prepareCoroutineRunning = false;
        timer.gameObject.SetActive(false);
    }

    public bool IsCrit()
    {
        float rand = Random.value;
        if(rand <= playerSO.critChance)
        {
            
            Debug.Log("crit");
            return true;
        }
        else
            return false;

        
    }
    public void StartWeaponChange()
    {
        if (attackCoroutineRunning)
        {
            weaponChangeNext = true;
        }
        else if (prepareCoroutineRunning)
        {
            timeLeftTemp = timeLeft;
            timeMaxTemp = timer.slider.maxValue;
            StopCoroutine(enumeratorPrepare);
            pausedCoroutine = true;
            Debug.Log("stopped coroutine at " + timeLeftTemp + "seconds left");
            StartCoroutine(WeaponChangeTimer());
        }
        else
            ChangeWeapon();
    }
    private void ChangeWeapon()
    {
        if (currentWeapon.weaponType == WeaponSO.WeaponType.Melee)
        {
            currentWeapon = weapons[1];
        }
        else
            currentWeapon = weapons[0];
        soundManager.Play("ChangeWeapon");
    }
    private void DealDamage()
    {
       // Debug.Log("pressed crit button = " + battleManager.critButtonPressed);
        if(battleManager.automaticCrit)
        {

            if (IsCrit())
            {
                enemy.currentEnemy.TakeDamage(currentWeapon.damage + playerSO.critDamage);
                battleManager.TurnOffCrit();
            }
            else
                enemy.currentEnemy.TakeDamage(currentWeapon.damage);
        }
        else if (battleManager.critButtonPressed)
        {
            enemy.currentEnemy.TakeDamage(currentWeapon.damage + playerSO.critDamage);
            battleManager.TurnOffCrit();
        }
        else
        {
            enemy.currentEnemy.TakeDamage(currentWeapon.damage);
        }
       // Debug.Log("pressed crit button = " + battleManager.critButtonPressed);
    }
    private void Death()

    {
        animator.SetTrigger("Death");
        isDead = true;
    }
    private void SetTimer( float time)
    {
        timer.SetMaxTime(time);
        timer.SetTime(time);
        timeLeft = time;
    }
    private IEnumerator PrepareAttack()
    {
        //Debug.Log("preparing");
        prepareCoroutineRunning = true;
        SetTimer(playerSO.prepareTime);
        timer.ChangeColorPrepare();
        timerOn = true;
        WaitForSeconds wait = new WaitForSeconds(playerSO.prepareTime);
        yield return wait; 
        StartCoroutine(Attack());
        prepareCoroutineRunning = false;
        
    }
    private IEnumerator Attack()
    {
        attackCoroutineRunning = true;
        if (currentWeapon.weaponType == WeaponSO.WeaponType.Melee)
        {
            animator.SetTrigger("Attack");
            soundManager.Play("MeleeAttack");
        }
        else
        {
            animator.SetTrigger("AttackRanged");
            soundManager.Play("RangedAttack");
            flyArrow = true;
        }
        if (IsCrit() && !battleManager.automaticCrit)
        {
            battleManager.critButton.interactable = true;
        }
        SetTimer(currentWeapon.attackSpeed);
        timer.ChangeColorAttack();
        WaitForSeconds wait = new WaitForSeconds(currentWeapon.attackSpeed); 
        yield return wait;
        DealDamage();
        if (!weaponChangeNext)
        {
            enumeratorPrepare = PrepareAttack();
            StartCoroutine(enumeratorPrepare);
        }
        else
        {
            StartCoroutine(WeaponChangeTimer());
            
        }
        attackCoroutineRunning = false;
        flyArrow = false;
    }

    private IEnumerator WeaponChangeTimer()
    {
        ChangeWeapon();
        SetTimer(2);
        timer.ChangeColorWeapon();
        Debug.Log("changing weapon");
        weaponChangeNext = false;
        WaitForSeconds wait = new WaitForSeconds(2);
        yield return wait;

        if (pausedCoroutine)
        {
            timer.SetMaxTime(timeMaxTemp);
            timer.SetTime(timeLeftTemp);
            timeLeft = timeLeftTemp;
            timer.ChangeColorPrepare();
            pausedCoroutine = false;
            StartCoroutine(Timer(timeLeftTemp));
        }
        else
        {
            enumeratorPrepare = PrepareAttack();
            StartCoroutine(enumeratorPrepare);
        }

         

    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(enumeratorPrepare);
    }

}
