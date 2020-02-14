using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaintCtrl : MonoBehaviour
{
    [SerializeField]
    public GameObject recovery;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("PLAYER"))
            recovery.SetActive(true);
        if (coll.CompareTag("DESTROY"))
            Destroy(this.gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
