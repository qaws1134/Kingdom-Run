using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특수몬스터 배치를 담당하는 스크립트
public class SpecialEnemyArray : HpCtrl
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        var dgr = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();   // 리스폰매니저 오브젝트를 참조

        int[] arr = dgr.spm_chk;   // 특수 몬스터가 있는지 체크하는 배열을 가져옴


        Debug.Log("enemy hp" +Hp);
        if (coll.CompareTag("PLAYER"))
        {
            if (Hp <= 0)
            {
                if (arr[1] != 0)
                {
                    if (arr[0] != 0) arr[0]--;
                    else arr[1]--;
                }
                else
                    arr[0]--;
        
            }
        }
        else if (coll.CompareTag("DESTROY"))
        {
            if (arr[1] != 0)
            {
                if (arr[0] != 0) arr[0]--;
                else arr[1]--;
            }
            else arr[0]--;
        }
    }
}