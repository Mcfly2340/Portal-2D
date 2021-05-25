using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    [Header("Game Objects")]
    [Space]
    public GameObject fireEffect;
    public GameObject explosionEffect;
    public GameObject explosionLight;
    public GameObject fires;
    public GameObject collideWithFire;
    public GameObject Roof;
    public GameObject destoyedRoof;
    public GameObject startText;

    [Header("Others")]
    [Space]
    
    public Animator camAnim;
    public Rigidbody2D prb;
    public Animator playerAnimation;
    public Collider2D cutsceneTrigger;
    public AudioSource earRingSound;
    public AudioSource fireAlarm;
    public static bool isInCutscene = false;


    private void Start()
    {
        cutsceneTrigger = GetComponent<BoxCollider2D>();
        stopRunning();

    }
    private void Update()
    {
        if (!PlayerController.isCrawling)
        {
            cutsceneTrigger.isTrigger = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player" && PlayerController.isCrawling)
        {
            cutsceneTrigger.isTrigger = false;
            Debug.Log("collider on");
        } else if (collider.tag == "Player" && !PlayerController.isCrawling)
        {
            isInCutscene = true;
            StartCoroutine(startCutscene());
            Invoke(nameof(stopRunning), 3);
            PlayerController.canCrawl = true;
            PlayerController.canSprint = true;
            DoorWay.canGoThroughDoor = true;
            cutsceneTrigger.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == null)
        {
            cutsceneTrigger.isTrigger = false;
            Debug.Log("collider on");
        }
    }
    IEnumerator startCutscene()
    {
        camAnim.SetBool("cutscene1", true);
        //walking for scene
        for (int i = 0; i < 200; i++)
        {
            yield return new WaitForSeconds(0.01f);
            prb.transform.Translate(0.1f, 0, 0, 0);
        }
        //player can stand still after walking
        Machine.isStandingStill = true;

        //wait 5 seconds
        yield return new WaitForSeconds(4);

        //explosion effect
        explosionEffect.SetActive(true);

        Debug.Log("It's Going To Explode!");

        //wait 2 seconds
        yield return new WaitForSeconds(2);

        //fires to activate
        fires.SetActive(true);

        //death collider to active
        collideWithFire.SetActive(true);

        //set explosion to active
        explosionLight.SetActive(true);
        fireEffect.SetActive(true);

        

        //wait 2 seconds
        yield return new WaitForSeconds(2);

        //lower earring sound
        for (float i = 0.3f; i >= 0; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.5f);
            earRingSound.volume = i;
        }
        for (float i = 0f; i <= 1; i += 0.2f)
        {
            yield return new WaitForSeconds(0.5f);
            fireAlarm.volume = i;
        }
        //set sound to false when volume is 0
        startText.SetActive(false);

        //wait 3 seconds
        yield return new WaitForSeconds(3);

        //set light to deactivate
        explosionLight.SetActive(false);

        //set roof to disabled
        Roof.SetActive(false);

        //set destroyed roof to enabled
        destoyedRoof.SetActive(true);
        
        //can walk again
        Machine.isStandingStill = false;
    }
    void stopRunning()
    {
        camAnim.SetBool("cutscene1", false);
        isInCutscene = false;
    }
    void enableFires()
    {/*
        for (int i = 0; i < fire.Length; i++)
        {
            fire[i].SetActive(true);
        }*/
    }
}
