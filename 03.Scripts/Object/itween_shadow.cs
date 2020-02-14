using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itween_shadow : MonoBehaviour
{


    public GameObject obj;
    float repeatTime = 0.4f;
    int turn = -1;
    public float move;  //위아래 이동값
    float rune_ypos;// 룬의 y값 돌아갈 위치

    public Transform tr;

    void Start()
    {    
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        // 잠시 떨어졌다가 다시 붙어
       
        do
        {
            rune_ypos = obj.transform.position.y;
            move *= turn;
            if (move < 0)
            {
                iTween.MoveTo(obj, iTween.Hash("y", move-rune_ypos , "easeType", iTween.EaseType.easeOutQuart));
           
            }
            else
            {
                iTween.MoveTo(obj, iTween.Hash("y", rune_ypos, "easeType", iTween.EaseType.easeInQuad));
            }   
            yield return new WaitForSeconds(repeatTime);
        } while (true);
    }
}




