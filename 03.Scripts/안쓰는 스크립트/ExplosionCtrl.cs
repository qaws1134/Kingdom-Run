using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCtrl : MonoBehaviour
{
    void Start()
    {
        Invoke("destroy", 0.75f); // 0.75초 뒤에 destroy 함수 호출
    }
    void destroy()
    {
        Destroy(this.gameObject);
    }
}
