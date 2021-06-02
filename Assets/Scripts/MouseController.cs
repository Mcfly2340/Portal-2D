using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject mousePointer;
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        //spawn in custom mouse pointer
        Instantiate(mousePointer, transform.position, Quaternion.identity);
    }
}
