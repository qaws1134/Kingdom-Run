using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp_Ctrl : HpCtrl
{
    public GameObject Boss;
    public GameObject Boss_body;
    Animator ani;


    void Update()
    {
        Show_Hp_Gage();
        Kill_enemy();
    }

    void Kill_enemy()
    {
        ani = GetComponent<Animator>();
        if( Hp <= 0)
        {
            GameManager.score += Score;
            Boss_body.GetComponent<MoveCtrlOgre>().enabled = false;
            ani.SetTrigger("die");
        }
    }



    void end_die()
    {
        Destroy(Boss);
    }
}
