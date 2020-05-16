using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Flash : MonoBehaviour
{
    //declaring adjustable variable to control the VFX's lifespan
    public float lifeSpan;

    //declaring private variable that will measure ellapsed time
    private float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        //on first frame, set timer to lifespan declared above
        lifeTimer = lifeSpan;
    }

    // Update is called once per fixed frame (60 per second)
    void FixedUpdate()
    {
        //if timer is greater than 0, decrement by ellapsed time
        if (lifeTimer > 0.0f)
        {
            lifeTimer -= Time.deltaTime;
        }
        else
            //else, destroy the VFX object
            Destroy(gameObject);
    }
}
