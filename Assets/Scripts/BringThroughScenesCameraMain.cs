using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringThroughScenesCameraMain : MonoBehaviour
{
    // Start is called before the first frame update
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
