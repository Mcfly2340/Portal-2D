using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject destination;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = destination.transform.position;
        Debug.Log("teleported");
    }
}
