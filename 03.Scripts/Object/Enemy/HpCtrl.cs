using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
//일반몬스터를 관리하는 스크립트 
public class HpCtrl : MonoBehaviour
{
    public float Hp = 5;        
    public float initHp = 5;

    public Transform tr;    
    public GameObject die_effect;

    /*체력게이지*/
    public GameObject healthbarBackground;
    public Image healthfiled;

    /*스코어 변수*/
    public int Score = 0;   //몬스터별 스코어 

    public GameObject gold_obj;
    public int goldValue = 0;       // Random.Range(OO , goldValue); 식으로 랜덤하게 활용하는 골드의 최대값
    public int monster_exp = 0;     // 몬스터 경험치

    void Start()
    {
        healthfiled.fillAmount = 1f;            //fillAmout 초기화
    }

    void Update()
    {
        Show_Hp_Gage();
        Kill_enemy();
    }

    public void Show_Hp_Gage()
    {
        if (Hp != initHp)
        {
            healthfiled.fillAmount = Hp / initHp;   //체력게이지를 체력/초기 체력으로 나눠서 fillAmount 에 넣어줌
            healthbarBackground.SetActive(true);    //체력게이지를 보이게 함
        }
    } 

    public void Kill_enemy()
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
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        var pc = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        
        if (coll.CompareTag("BULLET"))      //BULLET 태그에 닿을 시 Hp를 pc.dmg 만큼 깎음
        {
            Hp -= pc.dmg;
        }
        else if (coll.CompareTag("DESTROY"))    //DESTROY 태그에 닿을 시 Destroy를 호출
        {
            var ResManager = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();
            ResManager.enemy_obj.Remove(this.gameObject);   // 적 사망시 리스폰 매니저의 적오브젝트 리스트에서 제거.
            Destroy(this.gameObject);
        }
    }

}
