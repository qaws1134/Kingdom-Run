using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알을 생성해주는 스크립트
public class BulletCtrl : MonoBehaviour
{

    public GameObject bullet;
    public GameObject pos;
    public float shooting_delay = 0.8f;   // 투사체 발사 대기 시간
    public bool fire = false;


    public GameObject enemy;
    void Start()
    {
        StartCoroutine(Shooting());
    }

    public IEnumerator Shooting()
    {

        var on = enemy.GetComponent<MoveCtrlSpecial>();
        do
        {
            if (on.fire_on == true)
            {
                Instantiate(bullet, pos.transform.position, Quaternion.identity);

            }
            yield return new WaitForSeconds(shooting_delay);
        } while (true);
    }

}
