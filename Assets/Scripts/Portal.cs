using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector3 mousepos;

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
    public static bool portalIsEquiped = false;
    public bool isCharged = false;
    public static bool isInFuture = false;

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
            }
            else if (Input.GetButtonDown("Fire2"))//press right mouse button
            {
                ma.startColor = new Color(255, 0, 0, 255);
                StartCoroutine(ShootOrange());
                ShootSound.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Mouse2))//press middle mouse button
            {
                ma.startColor = Color.green;
                StartCoroutine(ShootGreen());
                ShootSound.Play();
            }
            else if (Input.GetKeyDown(KeyCode.R))//press R button
            {
                ma.startColor = Color.white;
                removePortals();
                CancelShootSound.Play();
                Instantiate(impactEffect, Player.transform.position, Quaternion.identity);
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
            lineRenderer.SetPosition(1, endPos);
            //instantiate an impact effect to the place of the impact
            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            if (hitInfo.transform.name == "Character" || hitInfo.transform.name == "BluePortal" || hitInfo.transform.name == "OrangePortal" || hitInfo.transform.name == "GreenPortal")
            {//ignore collisions
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {//if facing right (normal rotation)
                if (Player.transform.rotation.eulerAngles.y < 1 && Player.transform.rotation.eulerAngles.y > -1)
                {
                    portalBlue.transform.position = new Vector3(hitInfo.transform.position.x - 1, mousepos.y);
                    portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                }
                else //if facing left
                {
                    portalBlue.transform.position = new Vector3(hitInfo.transform.position.x + 1, mousepos.y);
                    portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                }
            }
            
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ceiling"))
            {//if the raycast hit an object with layer ceiling then rotate 90°
                portalBlue.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y - 1f);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {//else if the raycast hit an object with layer floor then rotate -90°
                portalBlue.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y + 1f);
                portalBlue.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
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

        //color to orange
        lineRenderer.startColor = new Color(255, 0, 0, 255);
        lineRenderer.endColor = new Color(255, 0, 0, 255);
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, endPos);
        Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

        if (hitInfo)
        {
            

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
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ceiling"))
            {
                portalOrange.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y + 0.3f);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            }
            else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                portalOrange.transform.position = new Vector3(mousepos.x, hitInfo.transform.position.y - 0.3f);
                portalOrange.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
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
        yield return new WaitForSeconds(0.04f);
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
        lineRenderer.SetPosition(1, endPos);
        Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

        if (hitInfo)
        {

            if (hitInfo.transform.name == "Character")
            {
                Physics2D.IgnoreCollision(PlayerColl, PlayerColl, true);
            }
            else 
            {
                portalGreen.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, hitInfo.transform.position.y + 120, Input.mousePosition.z + 10));
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
    void removePortals()
    {
        portalBlue.transform.position = new Vector3(1000, 1000, 1000);
        portalOrange.transform.position = new Vector3(1000, 1000, 1000);
        portalGreen.transform.position = new Vector3(1000, 1000, 1000);
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
            if (isInFuture == false)
            {
                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 29.5f, Player.transform.position.z);
                portalGreen.transform.position = new Vector3(portalGreen.transform.position.x, portalGreen.transform.position.y - 29.5f, portalGreen.transform.position.z);
                isInFuture = true;
            }
            else
            {
                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 29.5f, Player.transform.position.z);
                portalGreen.transform.position = new Vector3(portalGreen.transform.position.x, portalGreen.transform.position.y + 29.5f, portalGreen.transform.position.z);
                isInFuture = false;
            }
        }
    }
}