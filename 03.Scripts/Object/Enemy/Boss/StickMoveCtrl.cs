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
        tr.transform.Rotate(0, 0, 15);
    }
}
 