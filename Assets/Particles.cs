using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Particles : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject Light;
    public Animation LightAnim;
    // Start is called before the first frame update
    void Start()
    {
        Light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(particles.particleCount > 30)
        {
            Light.SetActive(true);
            LightAnim.Play();
        }
    }
}
