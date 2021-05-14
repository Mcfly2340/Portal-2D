using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipDevice : MonoBehaviour
{
    public Portal portalGreen;
    public Portal portalBlue;
    public Portal portalOrange;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yes");
        portalGreen.portalIsEquiped = true;
        portalBlue.portalIsEquiped = true;
        portalOrange.portalIsEquiped = true;
        Destroy(gameObject);
    }
}
