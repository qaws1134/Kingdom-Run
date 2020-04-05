using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrlSpecial : MonoBehaviour
{
    public Transform tr;

    Animator ani;
    public GameObject FirePos;
    public float speed = -2f;
    public float bullet_speed = -3f;
    public bool fire_on = false;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
       var dgr = GameObject.Find("RespawnManager2").GetComponent<RespawnManager2>();       // 리스폰매니저 오브젝트를 참조
        int[] arr = dgr.spm_chk;   // 특수 몬스터가 있는지 체크하는 배열을   가져옴
       
        float size = Camera.main.orthographicSize;
        float offset = 2.8f;
        if (arr[0] != 0 && arr[1] != 0) 
            offset -= 1f;    // 특수몬스터 1열이 있다면 2열은 오프셋만큼 뒤에 출력

        if (Mathf.Round(tr.position.y) == Mathf.Round(size - offset))
        {     // 해당 위치까지 내려오면 정지
            speed = 0.0f;
            FirePos.SetActive(true);
            fire_on = true;
        }
        var hp = GetComponent<HpCtrl>().Hp;
        var initHp = GetComponent<HpCtrl>().initHp;
 
        if (hp < initHp * 0.5)
        {
            ani.SetBool("attack", false);
            FirePos.SetActive(false);
            fire_on = false;
            speed = -6.0f;
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float wSize = Camera.main.orthographicSize * screenRatio;


            float moveItween = 3f;
            if (gameObject.transform.position.x + moveItween >= wSize - 2f)            // 우측 화면 넘지 않도록
            {
                moveItween *= -1;
            }
            else if (gameObject.transform.position.x + moveItween <= -wSize + 2f)   // 좌측 화면 넘지 않도록
            {
                moveItween *= -1;
            }

            iTween.MoveBy(gameObject, iTween.Hash("x", moveItween, "time", 1.7f, "easeType", iTween.EaseType.easeInOutBounce));   // 특수 몬스터가 좌우로 이동하면서 돌진
        }
        tr.Translate(Vector3.up * speed * Time.deltaTime); // Vector3.up 방향으로 speed값의 속도로 총알의 위치를 바꿔줌
    }
}
