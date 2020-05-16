using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //preparing a singleton so that the camera's shake component can be access from anywhere
    public static CameraShake instance;

    private void Awake()
    {
        //allocating instance of camera shake for singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    //preparing a coroutine so that the function can be be applied for more than one frame
    //user inputs duration and magnitude of shake
    public IEnumerator Shake (float duration, float magnitude)
    {
        //setting original position to prevent perminant camera tilt/kilter
        Vector3 originalPosition = transform.localPosition;

        //declaring time-tracking variable
        float elapsed = 0.0f;

        //so long as the elapsed time (tracked by coroutine) is less than the user's duration...
        while(elapsed < duration)
        {
            //declaring random float variables multiplied by the user's magnitude
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;

            //adjust the camera's transform by the above values without adjusting its z
            transform.localPosition += new Vector3(x, y, originalPosition.z);

            //increment elapsed by time based
            elapsed += Time.deltaTime;

            //pause to allow the next frame to pass (part of the coroutine)
            yield return null;
        }
        //upon completion, refer to the stored location to reset camera
        transform.localPosition = originalPosition;
    }

}
