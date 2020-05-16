using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //public plugs for target and follower transforms
    public Transform targetTransform;
    public Transform followerTransform;

    //declaring variables to track offset between transforms and velocity of follower object
    private Vector3 offset;
    private Vector3 followerVelocity;

    //float to adjust level of movement dampening
    public float followerDampening = 0.15f;


    // Start is called before the first frame update
    private void Start()
    {
        //set original offset to follower's transform, and set both to target's transform
        offset = followerTransform.position = targetTransform.position;

        //initially push the camera back 10 units on the z axis. (had to implement this due to unity bug)
        followerTransform.position = new Vector3(followerTransform.position.x, followerTransform.position.y, -10);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        //calculating a future X position for the follower using mathf's SmoothDamp function (we can discuss this in-call)
        float dampenedX = Mathf.SmoothDamp
            (followerTransform.position.x, targetTransform.position.x + offset.x, ref followerVelocity.x,
            followerDampening);
        //doing the same for the Y position
        float dampenedY = Mathf.SmoothDamp
            (followerTransform.position.y, targetTransform.position.y + offset.y, ref followerVelocity.y,
            followerDampening);

        //applying calculated positions to the follower's transform while retaining z position
        followerTransform.position = new Vector3(dampenedX, dampenedY, followerTransform.position.z);
    }
}
