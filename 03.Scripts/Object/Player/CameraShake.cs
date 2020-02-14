using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//피격시 카메라 흔들림
public class CameraShake : MonoBehaviour
{

    public float shakeAmount;
    
    float ShakeTime;

    Vector3 initialPosition;


    public void VibrateFortime(float time)
    {
        ShakeTime = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = new Vector3(0f, 0f, -5f);
    }
    // Update is called once per frame
    void Update()
    {
     if(ShakeTime>0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = initialPosition;
        }
    }
}
