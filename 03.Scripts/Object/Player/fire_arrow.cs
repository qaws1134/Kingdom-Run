using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_arrow : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
