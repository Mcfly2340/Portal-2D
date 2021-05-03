using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public static PortalController portalControlInstance;

    [SerializeField]
    private GameObject Blueportal, OrangePortal;

    [SerializeField]
    private Transform bluePortalSpawnpoint, orangePortalSpawnpoint;
    private Collider2D bluePortalCollider, orangePortalCollider;

    // Start is called before the first frame update
    void Start()
    {
        portalControlInstance = this;
        bluePortalCollider = Blueportal.GetComponent<Collider2D>();
        orangePortalCollider = OrangePortal.GetComponent<Collider2D>();
    }

    /*public void createClone(string whereToCreate)
    {
        if (whereToCreate == "atBlue")
        {
            var instantiatedClone = Instantiate(clone, bluePortalSpawnpoint.position, Quaternion.identity);
            instantiatedClone.gameObject.name = "Clone";
            Debug.Log("Create clone at blue");
        }
        else if(whereToCreate == "atOrange")
        {
            var instantiatedClone = Instantiate(clone, orangePortalSpawnpoint.position, Quaternion.identity);
            instantiatedClone.gameObject.name = "Clone";
            Debug.Log("Create clone at Orange");
        }
    }

    public void DisableCollider(string colliderToDisable)
    {
        if (colliderToDisable == "Orange")
        {
            orangePortalCollider.enabled = false;
        }
        else if (colliderToDisable == "Blue")
        {
            bluePortalCollider.enabled = false;
        }
        Debug.Log("Disable Collider");
    }

    public void EnableColliders()
    {
        orangePortalCollider.enabled = true;
        bluePortalCollider.enabled = true;
        Debug.Log("Enable Collider");
    }*/
}
