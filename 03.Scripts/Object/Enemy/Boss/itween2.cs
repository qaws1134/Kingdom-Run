using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//보스 패턴wave의 이동을 구현
public class itween2 : MonoBehaviour
{
    void Start()
    {
         StartCoroutine(Wave());
    }
    IEnumerator Wave()
    {
        do
        {
            iTween.MoveBy(gameObject,
                    iTween.Hash("y", -1.5f,
                    "easeType", iTween.EaseType.easeOutElastic,
                    "speed",10f
                    ));
            yield return new WaitForSeconds(0.2f);
        } while (true);
    }
}


