using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    [Space]public Vector3 offset;

    [Space][SerializeField]private Quaternion myRotation;

    [Space] private static GameObject instance;

    void Start()
    {//check if there is a second camera
        if (instance != null && instance != this)
        {//destroy camera
            Destroy(gameObject);
            return;
        }
        instance = gameObject;
        myRotation = this.transform.rotation;
    }
    void LateUpdate()
    {
        this.transform.rotation = myRotation;
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
