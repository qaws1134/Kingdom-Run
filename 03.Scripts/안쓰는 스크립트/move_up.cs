using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_up : MonoBehaviour
{
    public Transform tr;
    public float speed = -5;


    // Update is called once per frame
    void Update()
    {
        tr.GetComponent<Rigidbody2D>().AddForce(new Vector2(5 * Mathf.Cos(Mathf.PI * 2 * 5 / 10), 5 * Mathf.Sin(Mathf.PI * 5 * 2 / 10)));

    }
}
