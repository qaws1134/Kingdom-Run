using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform tr;

    public int rt = 0; //회전값;

    private void Awake()
    {
        tr.transform.rotation = Quaternion.Euler(0, 0, rt);
    }


}
