using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAtStart : MonoBehaviour
{
    private void Awake()
    {
        PlayerController.FindObjectOfType<PlayerController>().enabled = true;
    }
}
