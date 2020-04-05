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
public class MoveCtrlOgre : MonoBehaviour
{
   
    public float MoveCool = 2.0f;
    public float DashCool = 5.0f;

    int appear = 0;
    public GameObject Bossbody;
    public float repeatTime = 3.0f;
    float bossPosY = 3.7f;
    public bool Stop_state = false;
    bool isGround;
    public GameObject Boss;

    public bool attack_on = false;


    float initHp;      // 보스 초기 체력 정보 가져오기
    float hp ;              // 보스 현재 체력 정보 가져오기
    Animator ani;
    
    void Start()
    {
        initHp = GameObject.Find("Ogre_Body").GetComponent<BossHp_Ctrl>().initHp;
        ani = GetComponent<Animator>();
    }
    void Update()
    {
      
        hp = GameObject.Find("Ogre_Body").GetComponent<BossHp_Ctrl>().Hp;

        DashCool -= Time.deltaTime;
        MoveCool -= Time.deltaTime;
        Boss_Spawn();

        if (DashCool <= 0 )
        {
            Boss_Dash(initHp, hp);
        }
        
        if (MoveCool <= 0 )
        {
            this.MoveCool = 2f;
            Move_Pattern();
        }

    }
    IEnumerator Move()
    {
        int move = Random.Range(0, 6);  //움직일지 아닐지를 정할변수 
                                       
        if (move != 0 && isGround)
        {
            ani.SetTrigger("Jump");
        }
        yield return new WaitForSeconds(1.0f);
    }



    void Move_Pattern()
    {
        StartCoroutine(Move());
    }

    void Boss_Spawn()
    {
        if (appear++ == 0)
        {                            // 첫 등장시 위치 이동


            iTween.MoveTo(Boss,
                                iTween.Hash("y", bossPosY,
                                "time", 2.0f,
                                "islocal", true,    // 이 코드와
                                "movetopath", false   //이 코드를 넣어줘야 localPosition이 정상적으로 움직인다.
                                                      //,"easetype", iTween.EaseType.easeInOutQuart
                                ));

            ani.SetTrigger("Stay");
        }
    }
    void Boss_Dash( float initHp, float hp)
    {
        //돌진 패턴
        if (hp < initHp * 0.6f)     // 오거 체력이 60% 이하고, 스킬 쿨타임이 되면 아래로 내려왔다가 올라감.
        {
            DashCool = 10.0f;   // 쿨타임 초기화
            ani.SetTrigger("Jump");
            iTween.MoveTo(Boss,
                              iTween.Hash("y", -4,
                              "time", 1.8f,
                              "islocal", true,
                              "movetopath", false,
                               "easetype", iTween.EaseType.easeInOutQuart,
                              "oncomplete", "DashReturn",
                              "oncompletetarget",this.gameObject
                              ));
         
        }

    }
    void DashReturn()
    {
        ani.SetTrigger("Jump");
        iTween.MoveTo(Boss,
                   iTween.Hash("y", bossPosY,
                   "time", 2.0f,
                   "islocal", true,
                   "movetopath", false,
                   "easetype", iTween.EaseType.easeInOutQuart,
                   "oncomplete", "DashEnd",
                   "oncompletetarget", this.gameObject));
    }
    void DashEnd()
    {
        ani.SetTrigger("Jump");
    }

    //카메라 밖으로 나가지 않게 조정 및 좌우 상하 이동
    void Move_Right_or_Left(GameObject boss)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height; 
        float wSize = Camera.main.orthographicSize * screenRatio;   //카메라 가로 크기
        float hSize = Camera.main.orthographicSize;             //카메라 세로 크기 
        float xoffset = 1.5f;        // 캐릭터 x 크기값 
        float yoffset = 3.0f;        //캐릭터 y 크기값
        int Left_Right = (Random.Range(0, 2) == 1) ? 1 : -1;  // 왼쪽으로 움직일지 오른쪽으로 움직일지 정할 변수

        float xmove = 1f * Left_Right;              //x축 방향 선택
        float ymove = 1f * Left_Right;              //y축 방향 선택

        int Xrange = Random.Range(0, 2);        //이동 거리 랜덤 선택
        int Yrange = Random.Range(0, 2);        //이동 거리 랜덤 선택 

        float checkXpos;
        float checkYpos;
        
        checkXpos = gameObject.transform.position.x + xmove * Xrange;   //다음 이동할 위치를 연산한 x값  
        checkYpos = gameObject.transform.position.y + ymove * Yrange;   //다음 이동할 위치를 연산한 y값

        //화면 밖으로 못나가게 설정
        if (checkXpos >= wSize - xoffset)
        {
            if (xmove >= 0)
            {
                xmove = -1.0f;
            }
        }
        else if (checkXpos <= -wSize + xoffset)
        {
            if (xmove <= 0)
            {
                xmove =1.0f;
            }
        }
        if (checkYpos >= hSize - yoffset)
        {
            if (ymove >= 0)
            {
                ymove = -1.0f;
            }
        }
        else if (checkYpos <= hSize / 2)
        {
            if (ymove <= 0)
            {
                ymove = 1.0f;
            }
        }
        //이동
        iTween.MoveBy(boss, iTween.Hash("x", xmove * Xrange, "y", ymove * Yrange, "easeType", iTween.EaseType.easeOutCubic, "time", 0.8f));
       
    }




    void Jump()
    {
        Move_Right_or_Left(Boss);
        Bossbody.GetComponent<Rigidbody2D>().AddForce(Vector2.up* 0.4f, ForceMode2D.Impulse);
    }

    void Jump_end()
    {

        ani.SetTrigger("Stay");
        attack_on = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag =="Ground")
        {
            this.isGround = true;
        }
    }
    //점프
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.tag =="Ground")
        {
            this.isGround = false;
        }
    }



}