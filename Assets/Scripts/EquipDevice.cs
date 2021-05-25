using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDevice : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Device is equiped");
        Portal.portalIsEquiped = true;
        Destroy(gameObject);
    }
}
