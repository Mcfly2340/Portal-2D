using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorWay : MonoBehaviour
{
    public static bool canGoThroughDoor = false;
    public GameObject spawnPosDoor;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canGoThroughDoor)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<PlayerController>().gameObject.transform.position = new Vector2(spawnPosDoor.transform.position.x, spawnPosDoor.transform.position.y);
            }
        }
        
    }
}
