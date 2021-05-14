using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringThroughScenesPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerpos;
    private static GameObject instance;
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = gameObject;
        gameObject.transform.position = playerpos.transform.position;
    }
}
