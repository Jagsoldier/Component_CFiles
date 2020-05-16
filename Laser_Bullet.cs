using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Bullet : MonoBehaviour
{
    //creating public component variables (accessible through unity's interface)
    public Transform firePoint;
    public Rigidbody2D playerRigidbody;
    public MouseProjectileSystem projectileSystem;
    public CameraShake cameraShake;

    //declaring public prefab variables (used for VFX)
    public GameObject bulletPrefab;
    public GameObject flashPrefab;

    //declaring adjustable weapon stats
    public float bulletSpeed;
    public float fireRate;

    //declaring adjustable weapon kick stats
    public float kickBackForce;
    public bool kickBack;

    //declaring private variable to delay the firerate
    private float fireRateTimer;

    // Update is called once per frame
    void Update()
    {
        //if player presses Mouse0
        if(Input.GetButton("Fire1"))
        {
            //invoke the fire function below
            Fire();
        }
    }

    //the below tracks how long it has been since the last bullet has been fired to enforce a fire rate.
    private void FixedUpdate()
    {
        //once per fixed frame (60 per second) if timer is greater than 0
        if(fireRateTimer > 0.0f)
        {
            //decrement timer by elapsed time
            fireRateTimer -= Time.deltaTime;
        }
    }

    private void Fire()
    {
        //fire rate timer adjusted above
        if(fireRateTimer <= 0.0f)
        {
            //instantiating a bullet prefab from the player's firepoint position and rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //instantiating a VFX prefab to add muzzle flash (quaternion.identity applies default rotation)
            Instantiate(flashPrefab, firePoint.position, Quaternion.identity);

            //temporarily storing the rigidbody of the newly instantiated bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            //adding force to the newly instantiated bullet of type impulse (continues after application)
            rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);

            //playing audio queue from sound manager instance (referenced through singleton)
            SoundManager.instance.PlaySFX(0, 0.1f);
            //if kickback is enabled...
            if(kickBack == true)
            {
                //apply force to the player's rigidbody in the opposed direction to the firepoint's rotation
                playerRigidbody.AddForce(-(firePoint.up * kickBackForce), ForceMode2D.Force);
            }
            //setting the firerate's timer to create fire delay
            fireRateTimer = fireRate;
            //shake the camera for one frame (references camerashake script)
            StartCoroutine(cameraShake.Shake(0.01f, 0.001f));
        }
    }
}
