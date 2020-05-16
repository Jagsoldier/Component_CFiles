using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseProjectileSystem : MonoBehaviour
{
    //declaring variable plugs for the cursor and player muzzle's rigidbodies
    public Rigidbody2D objectRigidbody;
    public Rigidbody2D muzzleRigidbody;

    //declaring private vector3 variables to store mouse's position, and mouse direction opposed to the muzzle
    private Vector3 mousePosition;
    private Vector3 direction;

    //creating public plugs for the cursor (that this script is attached to) and the player's camera
    public GameObject cursor;
    public Camera currentCamera;

    //adjustable variable to play with cursor's movespeed
    public float moveSpeed;
    //private variable to store the muzzle's angle (which will determine how bullets are rotated)
    private float muzzleAngle;

    //once per client-side frame...
    private void Update()
    {
        //getting mouse position using the plugged camera's space
        mousePosition = currentCamera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        //setting the cursor object's transform to that of the user's cursor
        cursor.transform.position = new Vector2(mousePosition.x, mousePosition.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //declaring temorary vector2 to get direction by subtracting mouse's transform from muzzle's
        Vector2 lookDir = mousePosition - muzzleRigidbody.transform.position;
        //setting a float variable using the acquired direction, and then converting it from radians to degrees
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90.0f;
        //setting muzzle's rotation to the angle defined above
        muzzleRigidbody.rotation = angle;

        //setting the 'muzzleAngle' float to the above calculated angle for referencing
        muzzleAngle = angle;
    }
}
