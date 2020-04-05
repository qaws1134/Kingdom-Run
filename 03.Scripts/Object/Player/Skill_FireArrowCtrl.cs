using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FireArrowCtrl : MonoBehaviour
{
    static int i = 0;


    void Update()
    {
        var map1 = GameObject.Find("Map1").GetComponent<BackgroundScroll>();
        var map2 = GameObject.Find("Map2").GetComponent<BackgroundScroll>();
        //맵이 스크롤중인지 판단
        if(map1.scroll_on == true|| map2.scroll_on == true) 
        {
            transform.Translate(Vector3.down * 1 * Time.deltaTime);
        }
        
    }

    void OnTriggerStay2D(Collider2D target)
    {
        var enemyCtrl = target.gameObject.GetComponent<HpCtrl>();
        if (target.CompareTag("ENEMY"))
        {
            i += 1; 
            enemyCtrl.Hp -= 0.02f;
        }
    }
}
