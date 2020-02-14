using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



//플레이어의 각 직업별 총알 발사를 위한 슬라이드 패턴을 관리해주는 스크립트
public class FireCtrl : MonoBehaviour
{

    //선택한 직업의 총알 열거
    enum Select
    {
        Knight ,
        Archer,
        Wizard,
        Wizard_midle_bullet,
        Wizard_large_bullet
    }
   //싱글샷과 더블샷 상태 열거
    public enum bullet_shot
    {
        one_shot,
        double_shot_ready,
        double_shot,
        one_shot_ready
    }
    
    public GameObject[] bullet; //각 총알을 담을 배열
    public GameObject[] pos; //총알이 발사할 위치를 담을 배열
    public GameObject charge; //차지 이팩트를 담을 오브젝트
    
    Animator ani;
 
    float minSwipeDist;
    Vector2 swipeDirection;

    public int Bullet_State;

    public AudioClip sfx;
    public AudioSource audioSource;

    
    float hold_time; //터치 홀드 시간을 저장 할 변수       
    public bool skill_on = false;
    public bool effect_on = true;


    //아처의 스킬 애니메이션을 적용 할 변수
    bool archer_ani_on = false;


    //마법사 공격을 위한 변수
    bool stick_on;  
    float wizard_charging;
    bool wizard_ani_start = false;

    bool swiped = false;

    //마우스 조작을 위한 변수들
    Vector2 touchDownPos;
    Vector3 mouseDownPos;

    void Awake()
    {
        Bullet_State = (int)bullet_shot.one_shot;
        ani = GetComponent<Animator>(); //애니메이션 컴포넌트 
        Vector2 screenSize = new Vector2(Screen.width, Screen.height); //스크린 사이즈를 입력받음
         minSwipeDist = Mathf.Max(screenSize.x, screenSize.y) / 16f; //
    }
    private void Update()
    {
#if UNITY_ANDROID
        processMobileInput();
      
#elif UNITY_EDITOR
         processInput();
#endif
        //테스트를 위한 키보드 입력
        if (Input.GetKey(KeyCode.Space))    // 키보드 스페이스바
        {
            ani.SetBool("Attack_m", true);
            Fire();
        }
    }

     //유니티 에디터일때 마우스 조작를 통한 테스트
    void processInput()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                mouseDownPos = Input.mousePosition;
                skill_on = true;
                swiped = false;
            }
            else if (Input.GetMouseButton(0) == true)
            {
                bool swipeDected = checkSwipe(mouseDownPos, Input.mousePosition);
                swipeDirection = (Input.mousePosition - mouseDownPos).normalized;
                if (swipeDected)
                    swiped = true;
            }
            else if (Input.GetMouseButtonUp(0) == true)
            {
                if (swiped == true)
                    onSwipeDectected(swipeDirection);

             
                swiped = true;
            }
        }
      
    }

    //모바일 환경일 때
    void processMobileInput()
    {
        float cooltime = GameObject.Find("Player").GetComponent<Skill>().skill_cooltime;

        //터치 입력이 없고 마법사일 때
        if (CharacterSelect.selected_character == (int)Select.Wizard)        //마법사일때 
        {
            stay(); //가만히 있을 시 마법사 애니메이션 실행
        }
        //터치 입력 시
        if (Input.touchCount > 0)
        {
            if (IsPointerOverUIObject(Input.GetTouch(Input.touchCount - 1).position) == false)
            {
                Touch t = Input.GetTouch(Input.touchCount - 1);
                 //Began터치 시작
                if (t.phase == TouchPhase.Began)
                {
                    touchDownPos = new Vector2(t.position.x, t.position.y); //터치한 위치를 TouchDownPos에 입력
                     swiped = false; //터치가 시작되면 swiped 를 false로 해서 checkSwipe메서드에 터치중이라는 것을 알림                     
                }
                //Statinary 터치 유지할 시 
                else if (t.phase == TouchPhase.Stationary)
                {
                    hold_time += Time.deltaTime;    //hold_time으로 터치한 시간을 세줌
                    if (hold_time > 1.0f && cooltime <= 0)  //스킬의 쿨타임이 됐고 터치 유지시간 조건이 맞을 때
                    {
                        //궁수일때 활 시위를 땡긴상태로 유지하는 애니메이션
                        if (CharacterSelect.selected_character == (int)Select.Archer) 
                        {
                            if (!archer_ani_on) 
                            {
                                ani.SetBool("Skill_Start", false);
                                ani.SetBool("Skill_Ready", true);   //스킬 준비동작 애니메이션 동작
                                archer_ani_on = true;
                            }
                        }
                        if (effect_on)
                        {
                            //스킬을 발동한다는 이펙트를 나타냄
                            for (int i = 0; i < 3; i++)
                                Instantiate(charge, pos[2].transform.position, Quaternion.identity);
                            effect_on = false;
                        }
                    }
                }
                //Moved 터치 이동 시
                else if (t.phase == TouchPhase.Moved)
                {
                    Vector2 currendtTouchPos = new Vector2(t.position.x, t.position.y);//이동중인 현재 위치를 받음

                    bool swipeDetected = checkSwipe(touchDownPos, currendtTouchPos); //checkSwipe메서드를 사용해서 직업별 스와이프를 판단
                    swipeDirection = (currendtTouchPos - touchDownPos).normalized; //방향값을 정규화해서 스와이프 방향을 체크
                    if (swipeDetected)  //각 직업별 스와이프가 맞으면 swiped를 true로 바꿔줌
                        swiped = true;
                }
                //Ended 터치 끝마쳤을 때
                else if (t.phase == TouchPhase.Ended)
                {
                    //아처 스킬애니메이션이 true일 때 스킬을 발사 
                    if (archer_ani_on)
                    {
                        if (CharacterSelect.selected_character == (int)Select.Archer)        //궁수일때 
                        {
                            //궁수 스킬 발동 시 애니메이션 동작
                            ani.SetBool("Skill_Start", true);   
                            ani.SetBool("Skill_Ready", false);
                            archer_ani_on = false;
                        }
                        skill_on = true;
                    }
                    if(CharacterSelect.selected_character == (int)Select.Knight         
                        || CharacterSelect.selected_character == (int)Select.Archer) //기사,궁수일 때
                    {
                        //checkswipe()에서 swiped한 상태라고 돌려받으면 총알 발사
                        if (swiped)
                        {
                            onSwipeDectected(swipeDirection);     
                        }
                    }
                    else if (CharacterSelect.selected_character == (int)Select.Wizard)  //마법사일때 
                    {
                        if (wizard_ani_start)
                        {
                            w_charging();               
                        }
                    }
                    effect_on = true;
                    hold_time = 0;
                    swiped = true;
                }
            }
        }
    }
    void stay() //움직임 판단
    {
        stick_on = GameObject.Find("JoyStickBase").GetComponent<JoyStick3>().wiza_attack_cancle;

        if (stick_on==false) 
        {
            ani.SetBool("Attack_cancle", false);
            ani_ready();   //애니시작 함수 -> 한번만 실행 될 수 있도록 시작후 false 로 돌림  차징시간을 샘  
       
        }
        else   
        {
            wizard_ani_start = false;
            ani.SetBool("Attack_ready", false);
            ani.SetBool("Attack_cancle", true);
            wizard_charging = 0;
           
        }
    }

    void ani_ready() //가만히 있으면 공격 준비 애니메이션을 동작
    {
        if (!wizard_ani_start)
        {
            ani.SetBool("Attack_ready", true);
            wizard_ani_start = true;
        }
        wizard_charging += Time.deltaTime;
    }

    public void w_charging()
    {
        //차징 시간에 따른 마법사 총알 변동 
        if (wizard_charging < 1.0f)   
        {
            ani.SetBool("Attack_small", true);
            CharacterSelect.selected_character = (int)Select.Wizard;
            Fire();
           
        }
        else if (wizard_charging < 2.0f)    
        {
            ani.SetBool("Attack_middle", true);
            CharacterSelect.selected_character= (int)Select.Wizard_midle_bullet;
            Fire();  
        }
        else
        {
            ani.SetBool("Attack_large", true);
            CharacterSelect.selected_character = (int)Select.Wizard_large_bullet;
            bullet[CharacterSelect.selected_character].transform.localScale = new Vector3(0.25f,0.25f,0); // 1/10크기로 커짐 시간당
            bullet[CharacterSelect.selected_character].transform.localScale += new Vector3(wizard_charging / 5f, wizard_charging / 5f, 0); // 1/10크기로 커짐 시간당
            Fire();
           
        }
        stick_on = false;
        wizard_charging = 0;
        wizard_ani_start = false;
    }



    //UI클릭 시 다른 터치 인식을 못하게 하는 메서드
    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    //캐릭터별 터치슬라이드가 입력됐는지 판단하는 메서드
    bool checkSwipe(Vector3 downPos, Vector3 currentPos)
    {

        float distanceX = downPos.x - currentPos.x;      
        float distanceY = downPos.y - currentPos.y;
        if (swiped == true)
            return false;

        Vector2 currentSwipe = currentPos - downPos;

        if (CharacterSelect.selected_character == (int)Select.Knight)        //기사일때
        {
            //가로 슬라이드 입력을 판단함
            if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY) && Mathf.Abs(distanceX) > minSwipeDist) //
            {
                return true;
            }
        }
        else if (CharacterSelect.selected_character == (int)Select.Archer)   //궁수일떄
        {
            if (Mathf.Abs(distanceY) > Mathf.Abs(distanceX) && Mathf.Abs(distanceY) > minSwipeDist) // 궁수는 y축 차이가 x축 차이보다 커야 공격으로 인식.
            {
                return true;
            }
        }
        else if (CharacterSelect.selected_character == (int)Select.Wizard)   //마법사일떄
        {
            return true;
        }
        return false;
    }

    //기본 공격을 발사하는 메서드
    void onSwipeDectected(Vector2 swipeDirection)
    {
        ani.SetBool("Attack", true);
        Fire();
        swiped = true;
    }

    //기본 총알과 더블샷총알을 생성하는 메서드
    void Fire()
    {
        audioSource.PlayOneShot(sfx, 0.2f);

        if (Bullet_State == (int)bullet_shot.one_shot) //기본 공격
        {
            pos[1].SetActive(false);
            Instantiate(bullet[CharacterSelect.selected_character], pos[0].transform.position, Quaternion.identity);
        }
        else if (Bullet_State == (int)bullet_shot.double_shot_ready)  //노랑 룬 등룩(더블샷 룬)
        {
            pos[1].SetActive(true);
            pos[0].transform.Translate(-0.5f, 0, 0);
            pos[1].transform.Translate(0.5f, 0, 0);
            Instantiate(bullet[CharacterSelect.selected_character], pos[0].transform.position, Quaternion.identity);
            Instantiate(bullet[CharacterSelect.selected_character], pos[1].transform.position, Quaternion.identity);
            Bullet_State = (int)bullet_shot.double_shot;
        }
        else if(Bullet_State == (int)bullet_shot.double_shot) //더블샷 적용
        {
            pos[1].SetActive(true);
            Instantiate(bullet[CharacterSelect.selected_character], pos[0].transform.position, Quaternion.identity);
            Instantiate(bullet[CharacterSelect.selected_character], pos[1].transform.position, Quaternion.identity);
         }
        else if(Bullet_State == (int)bullet_shot.one_shot_ready)    //노랑 룬 해제(더블샷 룬)
        {
            pos[1].SetActive(false);
            pos[0].transform.Translate(0.5f, 0, 0);
            pos[1].transform.Translate(-0.5f, 0, 0);
            Instantiate(bullet[CharacterSelect.selected_character], pos[0].transform.position, Quaternion.identity);
            Bullet_State = (int)bullet_shot.one_shot;
        }
    }   

}
