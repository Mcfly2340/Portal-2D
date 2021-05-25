using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableText : MonoBehaviour
{
    public GameObject text;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        text.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        text.SetActive(false);
    }
}
