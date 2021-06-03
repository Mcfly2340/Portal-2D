using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Animators")]
    [Space]
    public Animator transition;

    [Header("Game Objects")]
    [Space]
    public GameObject pauseMenuUI;
    public GameObject deathScreenUI;
    public GameObject controlPanelUI;
    public GameObject winScreen;
    public GameObject player;

    [Header("Integers")]
    [Space]
    public int transitionTime = 1;
    

    [Header("Rigidbody's")]
    [Space]
    public Rigidbody2D playerRb;

    [Header("Booleans")]
    [Space]
    
    public bool panelEnabled = false;
    public bool hasWon = false;

    [Header("Other")]
    [Space]
    public Text countDownDisplay;
    public Text endTimeDisplay;

    public float maxTimeInSeconds = 151;

    //Static Booleans
    public static bool isPaused = false;
    public static bool startOverPressed = false;

    private void Update()
    {
        //check if in game scene
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            UpdateTimer();
            ControlPanel();

            //trigger pause
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                    if (Portal.portalIsEquiped) Cursor.visible = false;
                }
                else
                {
                    Pause();
                }
            }
            //if is dead then fly up in the air and turn on deathscreen
            if (DeathScript.isDead == true)
            {
                DeathScript.deathScreen.SetActive(true);
                playerRb.gravityScale = -10;
                Portal.portalIsEquiped = false;
            }
        }
    }
    public void ControlPanel()
    {
        //check if in game scene
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {//panel enabling
            if (Input.GetKeyDown(KeyCode.F10))
            {
                if (panelEnabled == false)
                {//enable controls panel
                    panelEnabled = true;
                    controlPanelUI.SetActive(true);
                }
                else if (panelEnabled == true)
                {//disable controls panel
                    panelEnabled = false;
                    controlPanelUI.SetActive(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Menu.startOverPressed)
            {
                controlPanelUI.SetActive(false);
                panelEnabled = false;
            }
        }
    }
    void UpdateTimer()
    {//if higher than 0, decrease timer
        if (maxTimeInSeconds > 0)
        {
            if (!hasWon)
            {
                maxTimeInSeconds -= Time.deltaTime;
            }
        }//if it's 0 then keep 0
        else
        {
            maxTimeInSeconds = 0;
        }
        DisplayTime(maxTimeInSeconds);
    }
    public void DisplayTime(float timeToDisplay)
    {
        //check if in game scene
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {//if displaying timer is lower than 0 then die
            if (timeToDisplay < 0)
            {
                timeToDisplay = 0;
                DeathScript.isDead = true;
            }
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            if (winScreen.activeInHierarchy)
            {//if winscreen is enabled then show still timer on winscreen 
                hasWon = true;
                endTimeDisplay.text = string.Format("Time left: " + "{0:00}:{1:00}", minutes, seconds);
            }
            else
            {//else show ingame
                hasWon = false;
                countDownDisplay.text = string.Format("{0:00}" + Environment.NewLine + "{1:00}", minutes, seconds);
            }
        }
    }

    public void Pause()
    {//for when to pause the game
        pauseMenuUI.SetActive(true);
        deathScreenUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
        isPaused = true;
    }
    public void Resume()
    {//for when to resume the game 
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PlayButton()
    {//for when to play the game
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex + 1, 1));
    }
    public void ToTitleButton()
    {//for when to go the title of the game
        DeathScript.isDead = false;
        StartCoroutine(WaitForSeconds(0, 1));
        pauseMenuUI.SetActive(false);
        deathScreenUI.SetActive(false);
        winScreen.SetActive(false);
        Debug.Log("To Title...");
        Time.timeScale = 1f;
    }
    
    public void StartOver()
    {//for when to start over the game
        deathScreenUI.SetActive(false);
        DeathScript.isDead = false;
        Time.timeScale = 1f;
        Resume();
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex, 1));
    }
    public void QuitButton()
    {//for when to quit the game
        Debug.Log("Exiting Application...");
        Time.timeScale = 0f;
        Application.Quit();
    }
    IEnumerator WaitForSeconds(int levelIndex, int time)
    {//triggering when going through transitions like start over, going to title
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(levelIndex);
    }
}
