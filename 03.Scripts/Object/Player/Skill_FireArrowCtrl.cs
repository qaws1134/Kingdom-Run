using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FireArrowCtrl : MonoBehaviour
{
    static int i = 0;
    void OnTriggerStay2D(Collider2D target)
    {
        var enemyCtrl = target.gameObject.GetComponent<HpCtrl>();
        if (target.CompareTag("ENEMY"))
        {
            i += 1;
            Debug.Log("불과 적이 충돌"+i);
            enemyCtrl.Hp -= 0.02f;
        }
    }
}
