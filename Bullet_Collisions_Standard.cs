using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Collisions_Standard : MonoBehaviour
{
    //public GameObject hitVFX;

    //creating gameobject plug for the impact VFX prefab
    public GameObject impactPrefab;

    //runs for every collision between objects (only registers that the collisions occur, but doesn't apply physics)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if collision object's tag is not the player's...
        if (collision.tag != "Player")
        {
            //Gameobject effect = Instantiate(hitVFX, transform.position, Quaternion.identity);
            //Destroy(effect, 5.0f);

            //instantiate the above stored impact VFX using its default rotation
            Instantiate(impactPrefab, gameObject.transform.position, Quaternion.identity);
            //call coroutine through camera shake's singleton
            StartCoroutine(CameraShake.instance.Shake(0.01f, 0.1f));
            //destroy the bullet (because it has collided with a surface that is not the player
            Destroy(gameObject);
        }
    }
}
