using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathScript : MonoBehaviour
{
    public static bool isDead = false;
    
    public static GameObject deathScreen;
    
    [Header("Game Objects")]
    [Space]
    public GameObject deathScreenRef;
    public GameObject player;
    
    private void Awake()
    {
        deathScreen = deathScreenRef;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.GetComponent<CapsuleCollider2D>().enabled = false;
        Debug.Log("Dead");
        isDead = true;
    }
}
