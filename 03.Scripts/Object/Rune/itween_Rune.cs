using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//룬의 이동을 담당하는 스크립트
public class itween_Rune : MonoBehaviour
{

    public GameObject obj;
    float repeatTime = 0.4f;
    int turn = -1;
    public float move;  //위아래 이동값
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        do
        {
             move *= turn;
            if (move > 0)
            {
                iTween.MoveBy(obj, iTween.Hash("y", move, "easeType", iTween.EaseType.easeOutQuart));
            }
            else
            {
                iTween.MoveBy(obj, iTween.Hash("y", move, "easeType", iTween.EaseType.easeInQuad));
            }
                yield return new WaitForSeconds(repeatTime);
        } while (true);
    }
}
