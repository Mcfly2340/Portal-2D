using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    //Portal assets:
    public Transform portalBlueSpawnPos, portalOrangeSpawnPos;
    public GameObject portalBlue, portalOrange;
    public GameObject portalGreen;
    //Player assets
    public GameObject Player;
    public Collider2D PlayerColl;
    public Camera cam;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        DontDestroyOnLoad(portalBlue);
        DontDestroyOnLoad(portalOrange);
        DontDestroyOnLoad(portalGreen);
        DontDestroyOnLoad(Player);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))//press left mouse button
        {
            StartCoroutine(ShootBlue());
        }
        else if (Input.GetButtonDown("Fire2"))//press right mouse button
        {
            StartCoroutine(ShootOrange());
        }
    }

    void Shoot()
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
    IEnumerator ShootBlue()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            //lineRenderer.SetPosition(0, firePoint.position);
            //lineRenderer.SetPosition(1, hitInfo.point);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (hitInfo.transform.name == "Character")
            {
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else 
            {
                portalBlue.transform.position = new Vector3(hitInfo.transform.position.x, Input.mousePosition.y / 10, 0);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        //enable shooting line
        lineRenderer.enabled = true;
        //wait some time
        yield return new WaitForSeconds(0.02f);
        //disable shooting line
        lineRenderer.enabled = false;
    }
    IEnumerator ShootOrange()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            //lineRenderer.SetPosition(0, firePoint.position);
            //lineRenderer.SetPosition(1, hitInfo.point);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (hitInfo.transform.name == "Character")
            {
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else 
            {
                portalOrange.transform.position = new Vector3(hitInfo.transform.position.x, Player.transform.position.y, 0);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        //enable shooting line
        lineRenderer.enabled = true;
        //wait some time
        yield return new WaitForSeconds(0.02f);
        //disable shooting line
        lineRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
                    
                    break;
                case 1:
                    SceneManager.LoadScene(sceneLoaded.buildIndex - 1);
                    break;
            }
        }
    }
}