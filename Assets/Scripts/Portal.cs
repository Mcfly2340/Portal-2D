using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Rigidbody2D enteredRigidbody;
    public Transform portalBlueSpawnPos, portalOrangeSpawnPos;
    public GameObject portalBlue, portalOrange;
    public GameObject portalGreen;
    public GameObject Player;
    


    void Update()
    {
        Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        if (Input.GetMouseButtonUp(0))
        {
            portalBlue.transform.position = new Vector3(mouse.x, mouse.y, 0);
        }
        if (Input.GetMouseButtonUp(1))
        {
            portalOrange.transform.position = new Vector3(mouse.x, mouse.y, 0);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            portalGreen.transform.position = new Vector3(mouse.x, mouse.y, 0);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        enteredRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();


        if (gameObject.name == "BluePortal")
        {
            Player.transform.position = new Vector3(portalOrangeSpawnPos.position.x, portalOrangeSpawnPos.position.y, 0);//teleport to orange portal
        }
        else if (gameObject.name == "OrangePortal")
        {
            Player.transform.position = new Vector3(portalBlueSpawnPos.position.x, portalBlueSpawnPos.position.y, 0);
        }
        else if (gameObject.name == "GreenPortal")
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            Scene sceneLoaded = SceneManager.GetActiveScene();
            switch (buildIndex)
            {

                case 0:
                    SceneManager.LoadScene(sceneLoaded.buildIndex + 1);
                    DontDestroyOnLoad(portalBlue);
                    DontDestroyOnLoad(portalOrange);
                    DontDestroyOnLoad(portalGreen);
                    DontDestroyOnLoad(Player);
                    break;
                case 1:
                    SceneManager.LoadScene(sceneLoaded.buildIndex - 1);
                    break;
            }
        }
    }
}