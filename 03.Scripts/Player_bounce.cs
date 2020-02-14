using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_bounce : MonoBehaviour
{
    private readonly Rigidbody2D rd;

    private void Update()
    {
        bounce();
    }
    void bounce() { 
        rd.AddForce(new Vector3(0, 0.2f, 0));
    }
}
