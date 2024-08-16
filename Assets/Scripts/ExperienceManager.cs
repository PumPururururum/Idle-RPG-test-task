using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    public AnimationCurve experienceCurve;
    public int currentLevel, totalExperience;
    public int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceText;
    public Image experienceSlider;

    private PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        UpdateLevel();
    }

    // Update is called once per frame
    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }
    private void CheckForLevelUp()
    {
        if(totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
            PlayerEnhancement();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().Play("LevelUp");
        }
    }
    private void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
        
    }

    private void UpdateInterface()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;

        levelText.text = currentLevel.ToString();
        experienceText.text = start + " exp / " + end + " exp";
        experienceSlider.fillAmount = (float)start / (float)end;
    }
    public void PlayerEnhancement()
    {
        player.playerSO.maxHealth += 5;
        player.playerSO.additionalDefense += 1;
        player.playerSO.critChance += 0.05f;
        if (player.playerSO.prepareTime > 1)
        {
            player.playerSO.prepareTime -= 0.2f;

        }
    }
}
