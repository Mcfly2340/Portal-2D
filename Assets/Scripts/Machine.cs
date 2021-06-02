using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public float transitionTime = 5f;
    [Header("Game Objects")]
    [Space]
    public GameObject machineSound;
    public GameObject ambientSound;
    public GameObject player;
    public GameObject futureSpawnPoint;
    public GameObject fireAlarm;

    [Header("Animators")]
    [Space]
    public Animator MachineShake;
    public Animator ClockRotation;
    public Animator transition;

    //booleans
    private bool alreadyPlayed = false;
    public static bool shouldNotBeMoving = false;


    private void Awake()
    {
        MachineShake.GetComponent<Animator>();
        ClockRotation.GetComponent<Animator>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        StartCoroutine(WaitForSeconds());
    }
    IEnumerator WaitForSeconds()
    {
        
        if (!alreadyPlayed)
        {//play only once
            machineSound.SetActive(true);
            alreadyPlayed = true;
        }

        //disable player movements
        shouldNotBeMoving = true;

        //rotate machine
        MachineShake.SetBool("HasEntered", true);
        ClockRotation.SetBool("HasEntered", true);

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("Start");

        //stop firealarm when in future
        fireAlarm.SetActive(false);

        yield return new WaitForSeconds(transitionTime);

        //teleport to future
        player.transform.position = futureSpawnPoint.transform.position;

        transition.SetTrigger("End");

        ambientSound.SetActive(true);

        shouldNotBeMoving = false;
        Portal.isInFuture = true;
    }
}
