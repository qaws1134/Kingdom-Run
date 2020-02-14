using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Servant_Knight_Ctrl : MonoBehaviour
{
    public Animator ani;
    public float init_speed = 2;
    public float enemy_init_speed = -2;
    public float speed;
    public List<Collider2D> enemy_coll = new List<Collider2D>();
    //public Collider2D enemy_coll;
    public float dmg = 1f;

    public GameObject healthbarBackground;
    public Image healthfiled;
    public float summon_time = 15;
    public float remain_time;
    

    // Start is called before the first frame update
    void Start()
    {
        remain_time = summon_time;
        healthfiled.fillAmount = 1f;

        speed = init_speed;
        ani = GetComponent<Animator>();
        InvokeRepeating("Attack", 0.5f, 1.0f);
        InvokeRepeating("Speed_Reset", 0.5f, 1.0f);     // 적들이 없는지 검사 후 원래 스피드로 전진
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.up * speed * Time.deltaTime);  // 속도에 따라 이동
        remain_time -= Time.deltaTime;
        healthfiled.fillAmount = remain_time / summon_time;
        if (remain_time <= 0)
        {
            foreach (Collider2D c in enemy_coll)  // 충돌한 리스트에 있는 적들의 체력을 소환수의 공격력만큼 감소 시킨다. 
            {
                try
                {
                    c.GetComponent<MoveCtrl>().speed = enemy_init_speed;
                }
                catch
                {
                    Debug.Log("Error : Servant_Knight_Ctrl");
                }
            }
            Destroy(this.gameObject);
        }

            
    }

    void Attack()   // 공격
    {
        foreach (Collider2D c in enemy_coll)    // 제거된 적은 리스트에서 지움.
        {
            if (c == null)
            {
                enemy_coll.Remove(c);
                break;
            }
        }
        ani.SetBool("Attack", true);                // 공격 모션 재생

        foreach (Collider2D c in enemy_coll)  // 충돌한 리스트에 있는 적들의 체력을 소환수의 공격력만큼 감소 시킨다. 
        {
            try
            {
                c.GetComponent<HpCtrl>().Hp -= dmg;
            }

            catch
            {
                Debug.Log("Error : Servant_Knight_Ctrl");
            }

        }

    }

    void Speed_Reset()              // 속도 원상 복구
    {
        if (enemy_coll == null)
            speed = init_speed;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("ENEMY"))
        {
            enemy_coll.Add(coll);                           // 충돌한 적들을 리스트에 담는다.
            speed = 0;                                      // 적과 충돌하면 기사의 속도를 0으로 변경하여 멈추도록 한다.
            remain_time -= 2.0f;
            try
            {
                enemy_init_speed = coll.GetComponent<MoveCtrl>().speed; // 적 원래 스피드를 변수에 넣어둠. 근데 가장 마지막에 갱신된 값 하나로 통일됨. 실제 원래 스피드는 아님
                coll.GetComponent<MoveCtrl>().speed = 0;    // 충돌한 적 멈춤
            }
            catch
            {
                Debug.Log("오류");
            }
        }

        if (coll.CompareTag("DESTROY"))
        {
            Destroy(this.gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("ENEMY"))
        {
            speed = init_speed;
        }
    }


}
