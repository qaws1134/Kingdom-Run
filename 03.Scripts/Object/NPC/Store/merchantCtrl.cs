using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class merchantCtrl : MonoBehaviour
{
    public Transform tr;
    public GameObject effect;
    public GameObject store;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("PLAYER"))
            store.SetActive(true);
        if (coll.CompareTag("DESTROY"))
            Destroy(this.gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
