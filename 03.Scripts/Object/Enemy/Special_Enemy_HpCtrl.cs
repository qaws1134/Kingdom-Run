using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//특수몬스터를 관리하는 스크립트
public partial class Special_Enemy_HpCtrl : HpCtrl
{
    public GameObject[] Runes;  //룬아이템을 담을 배열

    //update override
     void Update()
    {
        base.Show_Hp_Gage();
       // Kill_enemy();
    }

    void Kill_enemy()
    {
        if (Hp <= 0)
        {
            var ResManager = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();
            var pc = GameObject.Find("Player").GetComponent<PlayerCtrl>();
            pc.GetExp(monster_exp); // 몬스터 사망시 플레이어 경험치 증가.

            GameManager.score += Score;

            Instantiate(die_effect, tr.position, Quaternion.identity);

            ResManager.enemy_obj.Remove(this.gameObject);   // 적 사망시 리스폰 매니저의 적오브젝트 리스트에서 제거.

            if ((int)Random.Range(1, 4) == 1)     // 1~3 중 1일 경우. 즉 33% 확률로 골드 드랍.
            {
                GameObject obj = Instantiate(gold_obj, tr.position, Quaternion.identity);   // 생성된 골드를 obj에 담는다.
                goldValue = (int)Random.Range(goldValue * 0.3f, goldValue);    // goldValue 최대치의 30%부터 최대치까지 랜덤하게 나옴.
                obj.GetComponent<Gold>().goldValue = goldValue;                // 해당 골드 obj의 금화 가치를 위에서 구한 goldValue로 설정한다.
            }

            int[] enemy_Spawnarray = ResManager.spm_chk;   // 특수 몬스터가 있는지 체크하는 배열을 가져옴
            if (enemy_Spawnarray[1] > 0)
            {
                enemy_Spawnarray[1]--;
                /*
                if (enemy_Spawnarray[0] != 0)
                    enemy_Spawnarray[0]--;
                else 
                    enemy_Spawnarray[1]--;
                 */
            }
            else if(enemy_Spawnarray[0] > 0)
            {
                enemy_Spawnarray[0]--;
            }
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        Droprune();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var pc = GameObject.Find("Player").GetComponent<PlayerCtrl>();

        if (coll.CompareTag("BULLET"))
        {
            Hp -= pc.dmg;
            Kill_enemy();
        }
        else if (coll.CompareTag("DESTROY"))
        {
            var ResManager = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();   // 리스폰매니저 오브젝트를 참조

            ResManager.enemy_obj.Remove(this.gameObject);   // 적 사망시 리스폰 매니저의 적오브젝트 리스트에서 제거.
           
            int[] enemy_Spawnarray = ResManager.spm_chk;   // 특수 몬스터가 있는지 체크하는 배열을 가져옴
            if (enemy_Spawnarray[1] > 0)
            {
                enemy_Spawnarray[1]--;
                /*
                if (enemy_Spawnarray[0] != 0)
                    enemy_Spawnarray[0]--;
                else 
                    enemy_Spawnarray[1]--;
                 */
            }
            else if (enemy_Spawnarray[0] > 0)
            {
                enemy_Spawnarray[0]--;
            }

            Destroy(this.gameObject);
        }
    }

    //룬을 드랍하는 메서드
    void Droprune()
    {
        var rm = GameObject.Find("Player").GetComponent<RuneManager>(); //플레이어의 RuneManager을 참조

        int ran ;
        GameObject selectRune;

        if (rm.Ground_RuneNum < 2 )  //땅에 있는 룬이 1개 이하일 때
         {
            rm.Ground_RuneNum++;
            ran = Rune_Check(); //바닥에 있는 룬과 플레이어가 가지고 있는 룬을 검사해서 중복되지 않는 것을 넣음
            selectRune = Runes[ran];        
            
            if (rm.NowSlot < 1 )    //플레이어의 룬이 없을 때
            {
                Instantiate(selectRune, transform.position, Quaternion.identity);
            }
            else if (rm.NowSlot >= 1 ) //플레이어의 룬이 1개 이상일 때
            {
                //확률드랍
                //if (Random.Range(0, 10) == 0) // 1/10 확률로 룬을 드랍함
                    Instantiate(selectRune, transform.position, Quaternion.identity);
            }
           
        }

    }

    //중복된 룬을 방지하는 메서드
    int Rune_Check()
    {
        var Ground_Red = GameObject.Find("RedRune(Clone)");
        var Ground_Blue = GameObject.Find("BlueRune(Clone)");
        var Ground_Yellow = GameObject.Find("YellowRune(Clone)");
        var Ground_Purple = GameObject.Find("PurpleRune(Clone)");
        var rm = GameObject.Find("Player").GetComponent<RuneManager>();
        int ran;

        bool check = false;

        while (true)
        {
            ran = Random.Range(0, Runes.Length);    // 랜덤으로 룬을 선택
        
            switch (ran)    //땅에있는 룬 중복 검사
            {
                case 0:
                    if (Ground_Red != null)  
                        check = true;
                    break;
                case 1:
                    if (Ground_Blue != null)
                        check = true;
                    break;
                case 2:
                    if (Ground_Yellow != null)
                        check = true;
                    break;
                case 3:
                    if (Ground_Purple != null)
                        check = true;
                    break;
            }

            //플레이어가 현재 가지고 있는 마지막 슬롯의 룬을 검사 
            //만약 2개라면 첫번째 룬은 교체되기때문에 마지막것만 검사하면 됨
            if (rm.slot[rm.NowSlot] == ran)
                check = true;

            //검사에서 하나라도 중복되면 continue
            if (check == true)
            {
                check = false;
                continue;
            }
            else
            {
                return ran;
            }

        }
    }   
}
    