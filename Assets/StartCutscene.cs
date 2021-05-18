using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public static bool isInCutscene = false;
    public Animator camAnim;
    public Rigidbody2D prb;
    public Animator playerAnimation;
    public Collider2D coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player" && !PlayerController.isCrawling)
        {
            isInCutscene = true;
            StartCoroutine(startCutscene());
            Invoke(nameof(stopCutscene), 3);
            coll.isTrigger = true;
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
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(playerAnimation.GetFloat("speed"));
            playerAnimation.SetFloat("speed", 0);
        }
    }
    void stopCutscene()
    {
        camAnim.SetBool("cutscene1", false);
        isInCutscene = false;
        Debug.Log("HEY!");
    }
}
