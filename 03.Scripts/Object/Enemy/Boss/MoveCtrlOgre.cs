using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrlOgre : MonoBehaviour
{
    public Transform tr;

    int appear = 0;
    public float cool = 3.0f;
    float speed = 0.0f;
    public GameObject obj;
    float repeatTime = 3.0f;
    float bossPosY = 3.7f;
    void Start()
    {
        StartCoroutine(Move());
    }
    void Update()
    {

        float size = Camera.main.orthographicSize;

        float initHp = GameObject.Find("1-1. Ogre(Clone)").GetComponent<HpCtrl>().initHp;      // 보스 초기 체력 정보 가져오기
        float hp = GameObject.Find("1-1. Ogre(Clone)").GetComponent<HpCtrl>().Hp;              // 보스 현재 체력 정보 가져오기

        cool -= Time.deltaTime;
        //Debug.Log("\nhp : " + hp);
        //Debug.Log("gameObject.transform.position.y : " + gameObject.transform.position.y); // 오거 y 절대 위치

        if (appear++ == 0)
        {                            // 첫 등장시 위치 이동
            iTween.MoveTo(base.gameObject,
                                iTween.Hash("y", bossPosY,
                                "time", 2.0f,
                                "islocal", true,    // 이 코드와
                                "movetopath", false   //이 코드를 넣어줘야 localPosition이 정상적으로 움직인다.
                                                      //,"easetype", iTween.EaseType.easeInOutQuart
                                ));
        }

        //돌진 패턴
        if (hp < initHp * 0.6 && cool <= 0)     // 오거 체력이 60% 이하고, 스킬 쿨타임이 되면 아래로 내려왔다가 올라감.
        {
            iTween.MoveTo(base.gameObject,
                              iTween.Hash("y", -4,
                              "time", 1.8f,
                              "islocal", true,
                              "movetopath", false,
                              "easetype", iTween.EaseType.easeInOutQuart));
            cool = 20.0f;   // 쿨타임 초기화
        }

        if (gameObject.transform.position.y <= -4.0f)
        {
            speed = 0;
            iTween.MoveTo(base.gameObject,
                               iTween.Hash("y", bossPosY,
                               "time", 2.0f,
                               "islocal", true,
                               "movetopath", false,
                               "easetype", iTween.EaseType.easeInOutQuart));
        }
        // tr.Translate(Vector3.up * speed * Time.deltaTime); // Vector3.up 방향으로 speed값의 속도로 총알의 위치를 바꿔줌
    }
    IEnumerator Move()
    {

        do
        {
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float wSize = Camera.main.orthographicSize * screenRatio;
            float offset = 1.5f;        // offset과 moveitween 값이 애매하게 엇물리면 화면밖으로 나가게 되니 조심. 
            int temp = (Random.Range(0, 2) == 1) ? 1 : -1;

            float moveItween = 2.5f * temp;

            if (gameObject.transform.position.x + moveItween >= wSize - offset)

            {
                moveItween -= 4.5f;
            }
            else if (gameObject.transform.position.x + moveItween <= -wSize + offset)
            {
                moveItween += 4.5f;
            }
            iTween.MoveBy(obj, iTween.Hash("x", moveItween, "time", 2.0f));

            repeatTime = Random.Range(1.0f, 4.0f);     // 움직임 패턴 반복 주기를 랜덤으로 설정
            yield return new WaitForSeconds(repeatTime);
        } while (true);
    }
}