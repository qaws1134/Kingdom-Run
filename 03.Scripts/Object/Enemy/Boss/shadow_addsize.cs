using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadow_addsize : MonoBehaviour
{
    public Transform tr;

    // Update is called once per frame
    void Update()
    {
        tr.position += new Vector3(0,0.01f,0);
        tr.localScale += new Vector3(0.06f, 0.03f, 0);

    }
}
