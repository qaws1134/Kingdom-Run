using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public GameObject pos;
    public GameObject[] BossBullet;     //0번 돌맹이 1번 충격파 2번 몽둥이
    public int PtNum;   //패턴넘버
    public float[] cooltime = { 6.0f, 3.0f, 10.0f };
    bool t_on; // 던지기 판단
    Animator ani;

    public float speed=100f;

    //공격 코루틴
    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        //리스폰위치 지정
        var rm = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();

        do
        {
            //랜덤하게 패턴을 정해줌
            PtNum = Random.Range(0, 8);

            //쿨타임
            for (int i = 0; i < 3; i++)              // 시간이 지날때마다 모든 쿨타임  시간만큼 감소시킴
                cooltime[i] -= Time.deltaTime * 25;   // 30~50초쯤 곱해줘야 현실 시간 1초랑 비슷해짐...


            //충격파 날리기
            if (PtNum == 0 && cooltime[0] <= 0.0f)
            {
                for (int i = 0; i < 8; i++)
                {
                    Instantiate(BossBullet[1], pos.transform.position, pos.transform.rotation);
                }
                cooltime[0] = 8.0f;    // 쿨타임 초기화
                yield return new WaitForSeconds(1.0f);      // 스킬 쓰고난 다음에는 1초간 공격 멈춤
            }
            // 몽둥이 던지기
            else if (PtNum == 1 && cooltime[1] <= 0.0f)
            {
                Instantiate(BossBullet[2], pos.transform.position, pos.transform.rotation);
                cooltime[1] = 5.0f;    // 쿨타임 초기화
                yield return new WaitForSeconds(1.0f);
            }
            //몬스터 군단 소환
            else if (PtNum == 2 && cooltime[2] <= 0.0f)
            {
                if (rm.obj.Length > 0)
                {         // 몬스터 배열이 있을 경우에만 몬스터 군단 소환 가능
                    for (int offset = 1; offset < 3; offset++)     // 두 번 반복 = 부대 두 줄
                        rm.summon_monster(8, offset);      // 8마리 넘어가면 안됨. 배열 크기가 8이라서
                }
                cooltime[2] = 13.0f;    // 쿨타임 초기화
                yield return new WaitForSeconds(1.0f);
            }
            //기본공격 유도
            else if (PtNum == 3)
            {

                ani.SetBool("attack", true);
                GameObject obj;
                if (t_on)
                {
                    obj = Instantiate(BossBullet[0], pos.transform.position, Quaternion.identity);
                    follow(GameObject.Find("Player"), obj);
                    ani.SetBool("attack", false); ;

                    t_on = false;
                    //yield return new WaitForSeconds(2.0f);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
            //기본공격  범위
            else 
            {

                ani.SetBool("attack", true);
                int i = Random.Range(72, 78);
                GameObject obj;
                if (t_on)
                {
                    obj = Instantiate(BossBullet[0], pos.transform.position, Quaternion.identity);
                    obj.GetComponent<Rigidbody2D>().AddForce(
                        new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / 100), 
                                    speed * Mathf.Sin(Mathf.PI * i * 2 / 100)));
                    ani.SetBool("attack", false); ;
                    t_on = false;              
                }
                else
                {     
                    yield return new WaitForSeconds(1f);
                }
            }

            
        } while (true);
    }

    void follow(GameObject _obj,GameObject bull)
    {
        Rigidbody2D temp;
        float digree;

        temp = bull.GetComponent<Rigidbody2D>();
        Vector2 ToPlayerDir = _obj.transform.position - 
            new Vector3(bull.transform.position.x, bull.transform.position.y);
        temp.AddForce(ToPlayerDir.normalized * speed); 

        digree = Mathf.Atan2(bull.transform.position.y - _obj.transform.position.y, 
            bull.transform.position.x - _obj.transform.position.x) * 180f / Mathf.PI;
        //bull.transform.Rotate(0, 0, digree);
    }

    //애니메이션 이벤트 함수
    GameObject stone;
    void summon_stone()
    {
        Vector2 s_point = pos.transform.position;
        s_point += new Vector2(0, 1.3f);
        stone = Instantiate(BossBullet[0], s_point, Quaternion.identity);
    }
    void Destroy_stone()
    {
        Destroy(stone);
    }
    void throw_on()
    {
        t_on = true;
    }

}
