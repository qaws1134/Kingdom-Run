using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알을 생성해주는 스크립트
public class BulletCtrl : MonoBehaviour
{

    public GameObject bullet;
    GameObject bulletclone;
    public GameObject pos;
    public float shooting_delay = 1.5f;   // 투사체 발사 대기 시간
    float speed = 300f;
    Animator ani;

    public GameObject enemy;
    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(Shooting());
    }

    public IEnumerator Shooting()
    {

        var on = enemy.GetComponent<MoveCtrlSpecial>();
        do
        {
            if (on.fire_on == true)
            {
                ani.SetTrigger("attack");
            }
            yield return new WaitForSeconds(shooting_delay);
        } while (true);
    }


    void do_attack()
    {
        int i = Random.Range(74, 78);
        bulletclone = Instantiate(bullet, pos.transform.position, Quaternion.identity);
        bulletclone.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / 100), speed * Mathf.Sin(Mathf.PI * i * 2 / 100)));
    }
}
