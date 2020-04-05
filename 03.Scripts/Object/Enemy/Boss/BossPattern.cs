using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * 움직이거나 만다

움직이거나 말았다는 상태를 전달

공격 패턴을 정한다

애니메이션을 실행
동시에 공격상태를 전달
애니메이션이 끝마치면 총알을 생성

총알을 생성하면 스테이 애니메이션으로 옮김

스테이 상태가 끝나면 움직이라고 전달

 * 
 */
public class BossPattern : MonoBehaviour
{

    enum Boss_Attack
    {
        Smash_Wave,
        Stick,
        Stone,
        Summon
    }

    public GameObject pos;
    public GameObject[] BossBullet;     //0번 돌맹이 1번 충격파 2번 몽둥이
    public int PtNum;   //패턴넘버
    public float[] cooltime = { 4.0f, 5.0f, 4.0f, 6.0f };
    bool follow_on = false;

    GameObject stone;
    GameObject stick;
    Animator ani;

    CameraShake S_Camera;



    public float speed = 300f;

    //공격 코루틴
    void Start()
    {
        ani = GetComponent<Animator>();
        S_Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }

    void Update()
    {
        var attack_on = GameObject.Find("Ogre_Body").GetComponent<MoveCtrlOgre>();
      
        //쿨타임
        for (int i = 0; i < 3; i++)              // 시간이 지날때마다 모든 쿨타임  시간만큼 감소시킴
            cooltime[i] -= Time.deltaTime;   // 30~50초쯤 곱해줘야 현실 시간 1초랑 비슷해짐...

        if(attack_on.attack_on)
        {
            attack_on.attack_on=false;
            Attack_Pattern();
        }

    }


    void Attack_Pattern()
    {

   

        PtNum = Random.Range(0, 4); //랜덤하게 패턴을 정해줌

        if (PtNum == (int)Boss_Attack.Smash_Wave && cooltime[0] <= 0.0f)
        {
            // ani.SetBool("Wave", true) ;
            ani.SetTrigger("Wave");
            cooltime[(int)Boss_Attack.Smash_Wave] = 8.0f;
        }
        else if (PtNum == (int)Boss_Attack.Stick && cooltime[1] <= 0.0f)
        {
            ani.SetTrigger("Stick");

            cooltime[(int)Boss_Attack.Stick] = 5.0f;
        }
        else if (PtNum == (int)Boss_Attack.Summon && cooltime[2] <= 0.0f)
        {
            ani.SetTrigger("Summon");

            cooltime[2] = 13.0f;    // 쿨타임 초기화
        }
        else if (PtNum == (int)Boss_Attack.Stone)
        {
            follow_on = true;
            ani.SetTrigger("Attack");
        }
        else
        {
            ani.SetTrigger("Attack");
        }

    }

    //유도탄 
    void follow(GameObject _obj,GameObject bull)
    {
        Rigidbody2D temp;
        temp = bull.GetComponent<Rigidbody2D>();
        Vector2 ToPlayerDir = _obj.transform.position - new Vector3(bull.transform.position.x, bull.transform.position.y);
        temp.AddForce(ToPlayerDir.normalized * speed); 
    }



    void start_summon()
    {
        S_Camera.VibrateFortime(0.6f);
        var rm = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();  //리스폰위치 지정

        if (rm.obj.Length > 0)
        {         // 몬스터 배열이 있을 경우에만 몬스터 군단 소환 가능
            for (int offset = 1; offset < 3; offset++)     // 두 번 반복 = 부대 두 줄
                rm.summon_monster(8, offset);      // 8마리 넘어가면 안됨. 배열 크기가 8이라서
        }

    }
   void start_Stick()
    {
        Vector2 s_point = pos.transform.position;
        s_point += new Vector2(-0.65f, 1.5f);
        stick = Instantiate(BossBullet[(int)Boss_Attack.Stick], s_point, pos.transform.rotation);
    }


    void do_Stick()
    {
        stick.GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed);
        stick.GetComponent<Rigidbody2D>().AddTorque(500f);
    }


    void do_Wave()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(BossBullet[(int)Boss_Attack.Smash_Wave], pos.transform.position, pos.transform.rotation);
        }
    }
    
    void jump_wave()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.01f, ForceMode2D.Impulse);
    }
    void summon_stone()
    {
        Vector2 s_point = pos.transform.position;
        s_point += new Vector2(0, 1.3f);
        stone = Instantiate(BossBullet[(int)Boss_Attack.Stone], s_point, Quaternion.identity);
    }

    void throw_on()
    {
        if (follow_on)
        {

            follow(GameObject.Find("Player"), stone);
            follow_on = false;
        }
        else
        {
            int i = Random.Range(72, 78);
            stone.GetComponent<Rigidbody2D>().
                AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / 100),speed * Mathf.Sin(Mathf.PI * i * 2 / 100)));
        }
    }

    void end_pattern(int Pattern_num)
    {
        ani.SetTrigger("Stay");
    }
}
