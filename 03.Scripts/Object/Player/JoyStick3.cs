using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//다이나믹 조이스틱을 통한 캐릭터 이동을 구현한 메서드
public class JoyStick3 : MonoBehaviour
{
    public static JoyStick3 Instance
    {
        get
        {
            if(instance==null)
            {
                instance = FindObjectOfType<JoyStick3>();
                if(Instance ==null)
                {
                    var instanceContainer = new GameObject("JoyStick3");
                    instance = instanceContainer.AddComponent<JoyStick3>();
                }
            }
            return instance;
        } 
    }
    private static JoyStick3 instance;

    public GameObject smallStick;   
    public GameObject bGStick;      
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    float stickRadius;

    public bool wiza_attack_cancle = false; //마법사 공격 캔슬 
    public Transform Player;
    private bool MoveFlag;
    public float speed=300;

    //시작 시 조이스틱 반지름 크기 초기화 
    void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        MoveFlag = false;
    }
    void Update()
    {
        if(MoveFlag)
        {
            //플레이어 속도에 따른 위치 조정
            speed = GameObject.Find("Player").GetComponent<PlayerCtrl>().speed;
            Player.transform.Translate(Vector3.forward * Time.deltaTime * speed * 0.15f); // 곱해주는 값이 클수록 속도 증가.
        }
    }

    public void PointDown()
    {
        Vector3 input_pos = Input.mousePosition;

        if (input_pos.y >= 280) // y좌표 280 이상 못 넘어가게 함.
            input_pos.y = 280;

        stickFirstPosition= smallStick.transform.position  // Input.mousePosition 여러번 안 읽고 한 줄로 처리
            = bGStick.transform.position = input_pos;

        wiza_attack_cancle = true;  //마법사 차징공격 애니메이션 취소
    }
    public void Drag(BaseEventData baseEventData)
    {
        
        MoveFlag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        //joyVec = (DragPosition - stickFirstPosition).normalized;

        float stickDistance = Vector2.Distance(DragPosition, stickFirstPosition);

        //스틱 위치 설정
        if (stickDistance < stickRadius)
        {
            joyVec = (DragPosition - stickFirstPosition).normalized;
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;

        }
        else
        {
            bGStick.transform.Translate(joyVec * 20);   //배경 스틱 이동
            stickFirstPosition = bGStick.transform.position;    // 배경 스틱의 위치로 스틱 중앙값 초기화 
            joyVec = (DragPosition - stickFirstPosition).normalized;    //방향값 다시 받아옴
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;  //스몰 스틱 이동 범위는 배경 스틱 지름범위
        }
        Player.Translate(new Vector2(joyVec.x * 0.05f, joyVec.y * 0.05f));

        Player.eulerAngles = new Vector2(joyVec.y * -3, joyVec.x * 3);
        wiza_attack_cancle = true;


    }
    public void Drop()
    {
        joyVec = Vector2.zero;
        MoveFlag = false;
        wiza_attack_cancle = false;
    }

}
