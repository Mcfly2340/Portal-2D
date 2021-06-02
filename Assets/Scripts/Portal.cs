using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Portal : MonoBehaviour
{
    private Vector3 mousepos;

    [Header("Animators")]
    [Space]
    public Animator PlayerAnim;
    public Animator CursorAnim;

    [Header("Game Objects")]
    [Space]
    public GameObject portalBlue;
    public GameObject portalOrange;
    public GameObject portalGreen;
    public GameObject impactEffect;

    [Header("Player Assets")]
    [Space]
    public ParticleSystem ps;

    [Header("Transforms")]
    [Space]
    public Transform portalBlueSpawnPos;
    public Transform portalOrangeSpawnPos;

    [Header("Booleans")]
    [Space]
    public bool isCharged = false;

    public bool orangeHasSpawned = false;
    public bool blueHasSpawned = false;

    public static bool portalIsEquiped = false;
    public static bool isInFuture = true;

    [Header("Audio Assets")]
    [Space]
    public AudioSource CancelShootSound;
    public AudioSource ShootSound;

    [Header("Player Assets")]
    [Space]
    public GameObject Player;
    public Collider2D PlayerColl;
    public Camera cam;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    [Header("Floats")]
    [Space]
    public float greenPortalSpawnPos = 28;

    [Header("Cinemachine Assets")]
    [Space]
    public CinemachineVirtualCamera mainCam;

    [Space]

    public SpriteRenderer cursor;
    public Sprite cursorBlue;
    public Sprite cursorOrange;
    public Sprite cursorAll;
    public Sprite cursorNone;

    private void Start()
    {
        cursor.sprite = cursorNone;
    }
    void Update()
    {
        ParticleSystem.MainModule ma = ps.main;
        if (portalIsEquiped)
        {
            mousepos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            if (Input.GetButtonDown("Fire1"))//press left mouse button
            {
                ma.startColor = Color.blue;
                StartCoroutine(ShootBlue());
                ShootSound.Play();
                PlayerAnim.SetBool("isjumping", false);
                PlayerAnim.SetTrigger("isShooting");
                CursorAnim.SetTrigger("Pop");
            }
            else if (Input.GetButtonDown("Fire2"))//press right mouse button
            {
                
                ma.startColor = new Color(255, 0, 0, 255);
                StartCoroutine(ShootOrange());
                ShootSound.Play();
                PlayerAnim.SetBool("isjumping", false);
                PlayerAnim.SetTrigger("isShooting");
                CursorAnim.SetTrigger("Pop");
            }
            else if (Input.GetKeyDown(KeyCode.Mouse2))//press middle mouse button
            {
                
                ma.startColor = Color.green;
                StartCoroutine(ShootGreen());
                ShootSound.Play();
                PlayerAnim.SetBool("isjumping", false);
                PlayerAnim.SetTrigger("isShooting");
            }
            else if (Input.GetKeyDown(KeyCode.R))//press R button
            {
                ma.startColor = Color.white;
                removePortals();
                CancelShootSound.Play();
                Instantiate(impactEffect, Player.transform.position, Quaternion.identity);
                CursorAnim.SetTrigger("Pop");
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Mouse2))//press left mouse button
            {
                CancelShootSound.Play();
            }
        }        
    }
    void FixedUpdate()
    {
        //make camera behind player
        mainCam.m_Lens.NearClipPlane = -5;


        if (portalIsEquiped)
        {
            Cursor.visible = false;
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursor.transform.position = cursorPos;

            if (portalOrange.activeInHierarchy && portalBlue.activeInHierarchy)
            {
                cursor.sprite = cursorAll;
            }
            else if (portalOrange.activeInHierarchy && !portalBlue.activeInHierarchy)
            {
                cursor.sprite = cursorOrange;
            } 
            else if (portalBlue.activeInHierarchy && !portalOrange.activeInHierarchy)
            {
                cursor.sprite = cursorBlue;
            }
            else
            {
                cursor.sprite = cursorNone;
            }
        }
        else
        {
            Cursor.visible = true;
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
        Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, endPos - firePoint.position);
        

        if (hitInfo)
        {
            //change line to blue
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            //set positions of the line
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            //instantiate an impact effect to the place of the impact
            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            if (hitInfo.transform.name == "Character" || hitInfo.transform.name == "BluePortal" || hitInfo.transform.name == "OrangePortal" || hitInfo.transform.name == "GreenPortal")
            {//ignore collisions
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else if (hitInfo.transform.gameObject.tag == "Left & Right")
            {//if facing right (normal rotation)
                if (Player.transform.rotation.eulerAngles.y < 1 && Player.transform.rotation.eulerAngles.y > -1)
                {
                    portalBlue.transform.position = new Vector3(hitInfo.transform.position.x - 1, hitInfo.point.y);
                    portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 0);

                    portalBlue.SetActive(true);
                }
                else //if facing left
                {
                    portalBlue.transform.position = new Vector3(hitInfo.transform.position.x + 1, hitInfo.point.y);
                    portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 180);

                    portalBlue.SetActive(true);
                }
            }
            else if (hitInfo.transform.gameObject.tag == "Ceiling")
            {//if the raycast hit an object with layer ceiling then rotate 90°
                portalBlue.transform.position = new Vector3(hitInfo.point.x, hitInfo.transform.position.y - 1f);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 90);

                portalBlue.SetActive(true);
            }
            else if (hitInfo.transform.gameObject.tag == "Floor")
            {//else if the raycast hit an object with layer floor then rotate -90°
                portalBlue.transform.position = new Vector3(hitInfo.point.x, hitInfo.transform.position.y + 1f);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * -90);

                portalBlue.SetActive(true);
            }
        }
        else
        {//if hit nothing then make invisible
            lineRenderer.startColor = new Color(0, 0, 0, 0);
            lineRenderer.endColor = new Color(0, 0, 0, 0);
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
        Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, endPos - firePoint.position);


        if (hitInfo)
        {
            //change line to orange
            lineRenderer.startColor = new Color(255, 128, 0, 255);
            lineRenderer.endColor = new Color(255, 0, 0, 255);
            //set positions of the line
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            //instantiate an impact effect to the place of the impact
            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            if (hitInfo.transform.name == "Character" || hitInfo.transform.name == "BluePortal" || hitInfo.transform.name == "OrangePortal" || hitInfo.transform.name == "GreenPortal")
            {//ignore collisions
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else if (hitInfo.transform.gameObject.tag == "Left & Right")
            {//if facing right (normal rotation)
                if (Player.transform.rotation.eulerAngles.y < 1 && Player.transform.rotation.eulerAngles.y > -1)
                {
                    portalOrange.transform.position = new Vector3(hitInfo.transform.position.x + 0.3f, hitInfo.point.y);
                    portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 180);

                    //make cursor enable orange side
                    portalOrange.SetActive(true);
                }
                else //if facing left
                {
                    portalOrange.transform.position = new Vector3(hitInfo.transform.position.x - 0.3f, hitInfo.point.y);
                    portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 0);

                    //make cursor enable orange side
                    portalOrange.SetActive(true);
                }
            }

            if (hitInfo.transform.gameObject.tag == "Ceiling")
            {//if the raycast hit an object with tag ceiling then rotate 90°
                portalOrange.transform.position = new Vector3(hitInfo.point.x, hitInfo.transform.position.y + 0.3f);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * -90);

                //make cursor enable orange side
                portalOrange.SetActive(true);
            }
            else if (hitInfo.transform.gameObject.tag == "Floor")
            {//else if the raycast hit an object with tag floor then rotate -90°
                portalOrange.transform.position = new Vector3(hitInfo.point.x, hitInfo.transform.position.y - 0.3f);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 90);

                //make cursor enable orange side
                portalOrange.SetActive(true);
            }
        }
        else
        {//if hit nothing then make invisible
            lineRenderer.startColor = new Color(0, 0, 0, 0);
            lineRenderer.endColor = new Color(0, 0, 0, 0);
        }
        //enable shooting line
        lineRenderer.enabled = true;
        //wait some time
        yield return new WaitForSeconds(0.02f);
        //disable shooting line
        lineRenderer.enabled = false;
    }
    IEnumerator ShootGreen()
    {
        Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, endPos - firePoint.position);

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitInfo.point);
        Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

        if (hitInfo)
        {
            Debug.Log("Shoot!");
            if (hitInfo.transform.name == "Character" || hitInfo.transform.name == "BluePortal" || hitInfo.transform.name == "OrangePortal" || hitInfo.transform.name == "GreenPortal")
            {
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else if (!isInFuture)
            {
                portalGreen.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 1.5f, Input.mousePosition.z + 10);
                portalGreen.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                portalGreen.SetActive(true);
            }
            else
            {
                portalGreen.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 1.5f, Input.mousePosition.z + 10);
                portalGreen.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                portalGreen.SetActive(true);
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
    IEnumerator CinemachineDisable()
    {//if going through green portal, let cinemachine camera catch up
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        Camera.main.transform.position = new Vector3(Player.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        yield return new WaitForSeconds(1.05f);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }
    void removePortals()
    {
        orangeHasSpawned = false;
        blueHasSpawned = false;
        portalBlue.SetActive(false);
        portalOrange.SetActive(false);
        portalGreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "BluePortal")
        {//is orange portal is enabled
            if (portalOrange.activeInHierarchy)
            {
                Player.transform.position = new Vector3(portalOrangeSpawnPos.position.x, portalOrangeSpawnPos.position.y, 0);//teleport to orange portal
            }
            
        }
        else if (gameObject.name == "OrangePortal")
        {//is blue portal is enabled
            if (portalBlue.activeInHierarchy)
            {
                Player.transform.position = new Vector3(portalBlueSpawnPos.position.x, portalBlueSpawnPos.position.y, 0);
            }
                
        }
        else if (gameObject.name == "GreenPortal")
        {
            if (isInFuture == false)
            {
                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - greenPortalSpawnPos, Player.transform.position.z);
                portalGreen.transform.position = new Vector3(portalGreen.transform.position.x, portalGreen.transform.position.y - greenPortalSpawnPos, portalGreen.transform.position.z);
                isInFuture = true;
                StartCoroutine(CinemachineDisable());
            }
            else
            {
                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + greenPortalSpawnPos, Player.transform.position.z);
                portalGreen.transform.position = new Vector3(portalGreen.transform.position.x, portalGreen.transform.position.y + greenPortalSpawnPos, portalGreen.transform.position.z);
                isInFuture = false;
                StartCoroutine(CinemachineDisable());
            }
        }
    }
}