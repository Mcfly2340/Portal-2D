using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StartCutscene : MonoBehaviour
{
    [Header("Game Objects")]
    [Space]
    public GameObject fireEffect;
    public GameObject explosionEffect;
    public GameObject explosionLight;
    public GameObject fires;
    public GameObject collideWithFire;
    public GameObject Electricity;
    public GameObject Roof;
    public GameObject destoyedRoof;
    public GameObject startText;
    public GameObject openDoorsText;
    public GameObject runText;
    public GameObject CamTrigger;
    public GameObject winScreen;
    public GameObject[] People = new GameObject[8];
    public Camera Cam;

    [Header("Others")]
    [Space]

    public Animator transition;
    public Animator camAnim;
    public Rigidbody2D prb;
    public Animator playerAnimation;
    public Collider2D cutsceneTrigger;
    public AudioSource earRingSound;
    public AudioSource fireAlarm;
    public AudioSource fire;
    public AudioSource chattering;
    public Light2D explosionLight2D;
    public static bool isInCutscene = false;


    private void Start()
    {
        cutsceneTrigger = GetComponent<BoxCollider2D>();
        playerAnimation.enabled = true;
    }
    private void Update()
    {
        if (!PlayerController.isCrawling)
        {
            cutsceneTrigger.isTrigger = true;
        }

        StartCoroutine(IfBuildingCamIsOn());
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(this.gameObject.name == "CutsceneTrigger" && collider.tag == "Player" && PlayerController.isCrawling)
        {
            cutsceneTrigger.isTrigger = false;
            Debug.Log("collider on");
        } 
        else if (this.gameObject.name == "CutsceneTrigger" && collider.tag == "Player" && !PlayerController.isCrawling)
        {
            isInCutscene = true;
            StartCoroutine(startCutscene1());
            Invoke(nameof(stopRunning), 3);
            PlayerController.canCrawl = true;
            PlayerController.canSprint = true;
            DoorWay.canGoThroughDoor = true;
            cutsceneTrigger.enabled = false;
        }
        if (this.gameObject.name == "BuildingTrigger" && collider.tag == "Player")
        {
            Debug.Log("Switching Camera");
            startCutscene2();
        }
        if (this.gameObject.name == "Entrance" && collider.tag == "Player")
        {
            Debug.Log("Switching Camera");
            StartCoroutine(startCutscene3());
        }
        if (this.gameObject.name == "MACHINE" && collider.tag == "Player")
        {
            Debug.Log("Finishing game");
            StartCoroutine(startCutscene4());
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
    IEnumerator startCutscene1()
    {
        camAnim.SetBool("cutscene1", true);

        Electricity.SetActive(true);
        //walking for scene
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            prb.transform.Translate(0.1f, 0, 0, 0);
        }
        //player can stand still after walking
        stopRunning();
        Machine.shouldNotBeMoving = true;
        playerAnimation.enabled = true;

        //wait 5 seconds
        yield return new WaitForSeconds(4);

        //stop chattering
        chattering.Stop();

        //set electricity on when entering the room
        Electricity.SetActive(false);

        //explosion effect
        explosionEffect.SetActive(true);

        //wait 2 seconds
        yield return new WaitForSeconds(2);

        //set explosion to active
        explosionLight.SetActive(true);
        fireEffect.SetActive(true);

        //fires to activate
        fires.SetActive(true);

        //death collider to active
        collideWithFire.SetActive(true);

        //set roof to disabled
        Roof.SetActive(false);

        //set destroyed roof to enabled
        destoyedRoof.SetActive(true);

        for (int i = 0; i < People.Length; i++)
        {
            People[i].transform.Rotate(0, 0, 90);
            People[i].transform.position = new Vector3(People[i].transform.position.x, People[i].transform.position.y - 1, People[i].transform.position.z);
        }

        //set texts to on or off
        startText.SetActive(false);
        openDoorsText.SetActive(true);
        runText.SetActive(true);

        //lower earring sound
        //lower light
        float j = 1;
        for (float i = 0.3f; i >= 0; i -= 0.01f)
        {
            earRingSound.volume = i;
            yield return new WaitForSeconds(0.5f);

            j -= 0.0333f;
            
            explosionLight2D.intensity = j;
        }

        //can walk again
        Machine.shouldNotBeMoving = false;
        camAnim.SetBool("cutscene1", false);
        playerAnimation.enabled = true;

        //set firealarm volume higher
        for (float i = 0f; i <= 1; i += 0.05f)
        {
            yield return new WaitForSeconds(0.5f);
            fireAlarm.volume = i;
            fire.volume = i;
        }
    }
    void stopRunning()
    {
        isInCutscene = false;
        playerAnimation.enabled = false;
    }
    void startCutscene2()
    {
        camAnim.SetBool("PortalCamIsEnabled", true);
        prb.transform.Translate(2f, 0, 0, 0);
        Machine.shouldNotBeMoving = true;
    }
    IEnumerator startCutscene3()
    {
        yield return new WaitForSeconds(0.5f);
        camAnim.SetBool("LastSceneIsEnabled", true);
    }
    IEnumerator startCutscene4()
    {
        yield return new WaitForSeconds(0.85f);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.85f);
        transition.SetTrigger("End");
        winScreen.SetActive(true);
        Portal.portalIsEquiped = false;
        Cursor.visible = true;
        Destroy(this);
    }

    IEnumerator IfBuildingCamIsOn()
    {
        if (Input.GetButtonDown("Fire1") && Machine.shouldNotBeMoving == true || Input.GetButtonDown("Fire2") && Machine.shouldNotBeMoving == true)
        {
            yield return new WaitForSeconds(0.5f);
            camAnim.SetBool("PortalCamIsEnabled", false);
            Machine.shouldNotBeMoving = false;
            Destroy(CamTrigger);
        }
    }
}
