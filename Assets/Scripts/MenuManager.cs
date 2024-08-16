using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameOver;
    public bool automaticCrit = true;
    // Start is called before the first frame update
    private void Start()
    {
        automaticCrit = true;
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenGameOver()
    {
        gameOver.SetActive(true);
    }

    public void CritOption(Toggle toggle)
    {
        if (toggle.isOn)
        {
            automaticCrit = true;
        }
        else
        {
            automaticCrit = false;
        }
    }
}
