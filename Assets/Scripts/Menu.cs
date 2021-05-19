using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Animators")]
    [Space]
    public Animator transition;

    [Header("Game Objects")]
    [Space]
    public GameObject pauseMenuUI;
    public GameObject deathScreenUI;
    public GameObject player;

    [Header("Integers")]
    [Space]
    public int transitionTime = 1;

    [Header("Rigidbody's")]
    [Space]
    public Rigidbody2D playerRb;
    
    //Static Booleans
    public static bool isPaused = false;
    public static bool startOverPressed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (DeathScript.isDead == true)
        {
            DeathScript.deathScreen.SetActive(true);
            playerRb.gravityScale = -10;
            //playerRb.gravityScale = 3;

        } else if (DeathScript.isDead == true)
        {
            DeathScript.deathScreen.SetActive(false);
            playerRb.transform.Translate(0, 0, 0);
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        deathScreenUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        GetComponent<ControlsPanel>().Panel();
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PlayButton()
    {
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex + 1, 1));
    }
    public void ToTitleButton()
    {
        pauseMenuUI.SetActive(false);
        deathScreenUI.SetActive(false);
        DeathScript.isDead = false;
        StartCoroutine(WaitForSeconds(0, 1));
        Debug.Log("To Title...");
        Time.timeScale = 1f;
    }
    
    public void StartOver()
    {
        deathScreenUI.SetActive(false);
        DeathScript.isDead = false;
        Time.timeScale = 1f;
        Resume();
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex, 1));
    }
    public void QuitButton()
    {
        Debug.Log("Exiting Application...");
        Time.timeScale = 0.0001f;
        Application.Quit();
    }
    IEnumerator WaitForSeconds(int levelIndex, int time)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(levelIndex);
    }
}
