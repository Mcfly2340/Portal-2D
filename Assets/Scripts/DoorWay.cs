using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorWay : MonoBehaviour
{
    public Animator transition;

    public GameObject spawnPosDoor;
    public GameObject fires;
    public static bool canGoThroughDoor = false;
    
    private void OnTriggerStay2D(Collider2D collision)
    {//if you can go through door (after 1st scene)
        if (canGoThroughDoor)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(Teleport());
            }
        }
    }
    IEnumerator Teleport()
    {//door "loading screen"
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.85f);
        fires.SetActive(false);
        FindObjectOfType<PlayerController>().gameObject.transform.position = new Vector3(spawnPosDoor.transform.position.x, spawnPosDoor.transform.position.y, spawnPosDoor.transform.position.z);
        yield return new WaitForSeconds(0.85f);
        transition.SetTrigger("End");
    }
}
