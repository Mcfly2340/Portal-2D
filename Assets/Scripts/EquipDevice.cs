using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDevice : MonoBehaviour
{
    public Portal portal;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yes");
        portal.portalIsEquiped = true;
        Destroy(gameObject);
    }
}
