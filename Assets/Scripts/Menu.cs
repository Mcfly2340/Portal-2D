using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator transition;
    public GameObject pauseMenuUI;

    public float transitionTime = 1f;

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
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
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
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void ToTitleButton()
    {
        StartCoroutine(WaitForSeconds(0));
        Time.timeScale = 1f;
    }
    public void StartOver()
    {
        Resume();
        WaitForStartOverSeconds(1);
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex));
    }
    public void QuitButton()
    {
        Debug.Log("Exiting Application...");
        Application.Quit();
    }
    
    IEnumerator WaitForSeconds(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    IEnumerator WaitForStartOverSeconds(int time)
    {
        yield return new WaitForSeconds(time);
    }
}
