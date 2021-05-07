using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringThroughScenesCameraMain : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    private static GameObject instance;
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = gameObject;
    }
    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
