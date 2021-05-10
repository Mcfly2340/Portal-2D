using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    //enviroment asssets
    public Vector3 mousepos;
    //Portal assets:
    public Transform portalBlueSpawnPos, portalOrangeSpawnPos;
    public GameObject portalBlue, portalOrange;
    public GameObject portalGreen;

    //[SerializeField]
    public bool portalIsEquiped = false;
    public bool isCharged = false;
    public AudioSource CancelShootSound;
    public AudioSource ShootSound;
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
        DontDestroyOnLoad(Camera.main);
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (portalIsEquiped)
        {
            mousepos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
            if (Input.GetButtonDown("Fire1"))//press left mouse button
            {
                StartCoroutine(ShootBlue());
                ShootSound.Play();
            }
            else if (Input.GetButtonDown("Fire2"))//press right mouse button
            {
                StartCoroutine(ShootOrange());
                ShootSound.Play();
            }
            else if (Input.GetKeyDown(KeyCode.F))//press f button
            {
                StartCoroutine(ShootGreen());
                ShootSound.Play();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.F))//press left mouse button
            {
                CancelShootSound.Play();
            }
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

        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;

        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            //lineRenderer.SetPosition(0, firePoint.position);
            //lineRenderer.SetPosition(1, hitInfo.point);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (hitInfo.transform.name == "Character" || hitInfo.transform.name == "BluePortal" || hitInfo.transform.name == "OrangePortal" || hitInfo.transform.name == "GreenPortal")
            {
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else if (Player.transform.rotation.eulerAngles.y < 1 && transform.rotation.eulerAngles.y > -1)//if going right
            {
                portalBlue.transform.position = new Vector3(hitInfo.transform.position.x - 1, mousepos.y);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
            }
            else
            {
                portalBlue.transform.position = new Vector3(hitInfo.transform.position.x + 1, mousepos.y);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            }
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("UP"))
            {
                portalBlue.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y - 1f);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("DOWN"))
            {
                portalBlue.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y + 1f);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            }
            else
            {
            }
        }
        else
        {//if hit nothing then stretch 100x to the right
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

        //color to orange
        lineRenderer.startColor = new Color(255, 0, 0, 255);
        lineRenderer.endColor = new Color(255, 0, 0, 255);

        if (hitInfo)
        {
            //lineRenderer.SetPosition(0, firePoint.position);
            //lineRenderer.SetPosition(1, hitInfo.point);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (hitInfo.transform.name == "Character" || hitInfo.transform.name == "BluePortal" || hitInfo.transform.name == "OrangePortal" || hitInfo.transform.name == "GreenPortal")
            {
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else if (Player.transform.rotation.eulerAngles.y < 1 && transform.rotation.eulerAngles.y > -1)//if going right
            {
                portalOrange.transform.position = new Vector3(hitInfo.transform.position.x + 0.3f, mousepos.y);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            }
            else
            {
                portalOrange.transform.position = new Vector3(hitInfo.transform.position.x - 0.3f, mousepos.y);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 0);

            }
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("UP"))
            {
                portalOrange.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y + 0.3f);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            }
            else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("DOWN"))
            {
                portalOrange.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y - 0.3f);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }
        }
        else
        {//if hit nothing then stretch 100x to the right
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }
        //enable shooting line
        lineRenderer.enabled = true;
        //wait some time
        yield return new WaitForSeconds(0.04f);
        //disable shooting line
        lineRenderer.enabled = false;
    }
    IEnumerator ShootGreen()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        if (hitInfo)
        {
            
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
                portalGreen.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 222, Input.mousePosition.z + 10));
                //, firePoint.transform.position.y + 0.1f, 0);
                portalGreen.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + Camera.main.ScreenToWorldPoint(Input.mousePosition) * 100);
        }
        //enable shooting line
        lineRenderer.enabled = true;
        //wait some time
        yield return new WaitForSeconds(0.04f);
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