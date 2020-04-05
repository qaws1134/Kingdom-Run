using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletColl : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("PLAYER"))
        {
            Destroy(this.gameObject);
        }
        if (coll.CompareTag("DESTROY"))
        {
            Destroy(this.gameObject);
        }
    }
    // 메인 카메라에서 보이지않을 때
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
