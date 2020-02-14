using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = 1f;
    
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);    
    }

    public void scrolling_toggle(bool tf)   // true면 스크롤링 on <-> false면 스크롤링 멈춤
    {
        speed = tf ? 1 : 0;
    }
}
