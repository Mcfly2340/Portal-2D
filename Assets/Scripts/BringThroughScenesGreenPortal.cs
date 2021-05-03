using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringThroughScenesGreenPortal : MonoBehaviour
{
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
}
