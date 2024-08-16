using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{
    public GameObject itemPrefab;
    private PlayerManager player;
    public EnemyManager enemy;

    public Button battleButton;
    public Button escapeButton;
    public Button healButton;
    public Button critButton;
    public Button changeWeaponButton;
    public bool isBattle;

    private ExperienceManager experienceManager;
    private MenuManager menuManager;
    public bool critButtonPressed;
    public bool automaticCrit;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        experienceManager = GameObject.Find("ExperienceManager").GetComponent<ExperienceManager>(); 
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        // enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyManager>();


    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isDead)
        {
            enemy.isDead = false;
            experienceManager.AddExperience(enemy.currentEnemy.expForKill);
            SpawnItem();
            StartCoroutine(ChangeEnemy());
        }
        if (player.isDead)
        {
            StopBattle();
            player.isDead = false;
            menuManager.OpenGameOver();
        }
        if (menuManager.automaticCrit)
        {
            critButton.gameObject.SetActive(false);

            automaticCrit = true;
        }
        else
        {
            critButton.gameObject.SetActive(true);
            automaticCrit= false;
        }
    }

    public void StopBattle()
    {
        isBattle = false;
        enemy.StopAttack();
        player.StopAttack();
        StopAllCoroutines();
        enemy.gameObject.SetActive(false);
        battleButton.interactable = true;
        escapeButton.interactable = false;
        healButton.interactable = true;
    }

    public void StartBattle()
    {
        isBattle=true;
        enemy.gameObject.SetActive(true);
        player.StartAttack();
        enemy.StartAttack();
        battleButton.interactable = false;
        escapeButton.interactable = true;
        healButton.interactable = false;
    }

    public void FullHeal()
    {
        player.playerSO.SetHealthToMax();
        GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("Healing");
    }


    public void ChangeWeapon()
    {
        player.StartWeaponChange();
        StartCoroutine(BlockButton());
    }
    private void SpawnItem()
    {
        int rand = Random.Range(0, 2);
        if (rand == 1)
        {
            Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Debug.Log("Spawned loot");
        }
    }
    public void TurnOnCrit()
    {
        critButtonPressed = true;
        //Debug.Log(critButtonPressed);
        critButton.interactable = false;
    }
    public void TurnOffCrit()
    {
        critButtonPressed = false;
        critButton.interactable = false;
    }

    private IEnumerator ChangeEnemy()
    {
        player.StopAttack();
        yield return new WaitForSeconds(3);

        StartBattle();
        
    }
    private IEnumerator BlockButton()
    {
        changeWeaponButton.interactable = false;
        yield return new WaitForSeconds(2);
        changeWeaponButton.interactable = true;
    }
}
