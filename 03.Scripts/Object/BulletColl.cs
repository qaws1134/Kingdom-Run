using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//충돌 시 오브젝트를 삭제시켜주는 메서드 
public class BulletColl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.CompareTag("ENEMY"))
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
