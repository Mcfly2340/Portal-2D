using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Machine : MonoBehaviour
{
    public float transitionTime = 5f;
    public AudioSource MachineSound;
    public GameObject machinesound;
    public Animator MachineShake;
    public Animator ClockRotation;
    private bool alreadyPlayed = false;

    private void Awake()
    {
        MachineShake.GetComponent<Animator>();
        ClockRotation.GetComponent<Animator>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        StartCoroutine(WaitForSeconds(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator WaitForSeconds(int levelIndex)
    {
        
        if (!alreadyPlayed)
        {
            machinesound.SetActive(true);
            MachineSound.Play();
            alreadyPlayed = true;
        }

        //disable player movements
        PlayerController.FindObjectOfType<PlayerController>().enabled = false;
        MachineShake.SetBool("HasEntered", true);
        ClockRotation.SetBool("HasEntered", true);
        DontDestroyOnLoad(MachineSound);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

    }
}
