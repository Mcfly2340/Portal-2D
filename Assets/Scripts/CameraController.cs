using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator camAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {//if this script belongs to gameobject with name ShowoffPortalTrigger
        if (this.gameObject.name == "ShowoffPortalTrigger")
        {
            camAnim.SetBool("PortalCamIsEnabled", true);
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {//if clicking then disable camera
                camAnim.SetBool("PortalCamIsEnabled", false);
            }
        }
    }
}
