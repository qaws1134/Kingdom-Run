using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMoveCtrl : MonoBehaviour
{
    public Transform tr;
    public float speed = 10f;

    void Update()
    {
        tr.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        tr.transform.Rotate(0,0,15);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("DESTROY"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
 