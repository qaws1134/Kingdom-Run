using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//각 캐릭터의 스킬을 관리하는 스크립트
public class Skill : MonoBehaviour
{
    public List<GameObject> obj = new List<GameObject>();    // 적 객체가 생성되면 들어갈 곳.
    public int summon_count = 2;

    [HideInInspector]   // public이어도 설정창에서 감추도록 하는 기능. 리스폰 위치는 자동으로 읽어들이므로 건들지 않아도 돼서.
    public Transform[] respawnTr; // 리스폰 위치

    // 궁수꺼
    public GameObject pos;           // 플레이어 투사체 발사 위치
    public GameObject FireArrow;    // 불화살 오브젝트
    public Vector3 touchPos;
    GameObject fireObj;
    SpriteRenderer HitLight;

    // 플레이어 스킬 쿨타임
    public GameObject CooltimeBarBackground;
    public Image CooltimeFilled;
    public float remain_time;
    public float skill_cooltime = 15.0f;
    public float init_cooltime = 15.0f;
    public float reduce_cooltime = 1.0f;    // 1배는 변화 없음. 1.2배는 20% 빠르게. 
    Animator ani;
    static private int selected_character;
    public float skill_duration = 8.0f; // 스킬 지속시간
    bool skill_on;
    bool arrow_on = false;
    public int player_level=1;


        // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        selected_character = CharacterSelect.selected_character; // 어떤 캐릭터 선택했는지 가져옴.
        player_level = GameObject.Find("Player").GetComponent<PlayerCtrl>().player_level;   // 플레이어 레벨 가져오기
        respawnTr = GameObject.Find("Player's_SummonPoint").GetComponentsInChildren<Transform>();
        skill_cooltime = init_cooltime;
        CooltimeFilled.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        var skill_on = GameObject.Find("Player").GetComponent<FireCtrl>();


        if (Input.GetKeyDown(KeyCode.V) /*&& skill_cooltime<=0*/)
        {
            if (skill_on)
            {
                switch (selected_character)
                {
                    case 0: // 기사
                        summon_count = player_level / 10 + 3;     // 레벨 10 단위로 1씩 증가. // +n는 레벨1때부터 등장하는 기사수 n명.
                        Debug.Log("summon_count:" + summon_count + "\nrespawn.len : " + respawnTr.Length);
                        if (summon_count > respawnTr.Length - 1)    // 생성 최대치를 넘으면 최대치로 고정.
                            summon_count = respawnTr.Length - 1;
                        summon_knights(summon_count);
                        break;
                    case 1: // 궁수
                            // Debug.Log(Mathf.Pow(1.025f, 10)); // Pow(스킬배율, 레벨)
                        ani.SetBool("Attack", true);
                        skill_duration = skill_duration * Mathf.Pow(1.025f, player_level);     // 궁수 레벨 가져와서 레벨마다 2.5%씩 시간 증가. 30레벨쯤 2배.      
                        fire_arrow();
                        break;
                    case 2: // 마법사
                        summon_servant(5);
                        break;
                    default:
                        break;
                }

                skill_cooltime = init_cooltime; // 스킬 사용 후 쿨타임 초기화
            }
        }


        if (skill_on.skill_on && skill_cooltime <= 0)
        {
            switch (selected_character)
            {
                case 0: // 기사
                    summon_count = player_level / 10 + 3;     // 레벨 10 단위로 1씩 증가. // +n는 레벨1때부터 등장하는 기사수 n명.
                    if (summon_count > respawnTr.Length - 1)    // 생성 최대치를 넘으면 최대치로 고정.
                        summon_count = respawnTr.Length - 1;
                    summon_knights(summon_count);
                    break;
                case 1: // 궁수
                        // Debug.Log(Mathf.Pow(1.025f, 10)); // Pow(스킬배율, 레벨)
                    skill_duration = skill_duration * Mathf.Pow(1.025f, player_level);     // 궁수 레벨 가져와서 레벨마다 2.5%씩 시간 증가. 30레벨쯤 2배.
                    fire_arrow();

                    break;
                case 2: // 마법사
                    summon_servant(5);
                    break;
                default:
                    break;
            }
            skill_cooltime = init_cooltime; // 스킬 사용 후 쿨타임 초기화
            skill_on.skill_on = false;
            skill_on.effect_on = true;
        }
        // 쿨타임 계산
        skill_cooltime -= Time.deltaTime * reduce_cooltime;
        if (skill_cooltime <= 0)
            skill_cooltime = 0;
        CooltimeFilled.fillAmount = 1 - skill_cooltime / init_cooltime;

    }


    public void summon_knights(int summon_count)                                                        // 매개 변수만큼 기사 생성
    {
        List<int> respawn_index = new List<int>();
        for (int i = 1; i <= respawnTr.Length; i++)
            respawn_index.Add(i);
        
        for (int i = 1; i <= summon_count; i++)                                                                              // 1번 인덱스부터 해야함. 0번은 부모라서. 0번부터 하면 부모 위치까지 생성됨.
        {
            int rand = Random.Range(0,respawn_index.Count);
            Instantiate(obj[selected_character], 
                respawnTr[respawn_index[rand]-1].transform.position, Quaternion.identity);
            respawn_index.RemoveAt(rand);
        }
    }


    public void fire_arrow() // 시간초 만큼 정면 몇 미터 앞에 불화살
    {
        Debug.Log("불화살");
        // 스크린 좌표를 월드 좌표로 변환함
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        touchPos = new Vector3(wp.x, wp.y, -0.35f);

        //// 오브젝트의 위치를 갱신함
        //firetransform.position = touchPos;

        // fa : fireArrow 약자
        GameObject fa = Instantiate(FireArrow, pos.transform.position, Quaternion.identity);
        Vector3 temp = wp - transform.position; // 플레이어와 해당 지점간 차이를 아래 angle에서 각도로 변환할 거임.

        // 클릭 지점과 플레이어 사이의 각도를 구함(아크 탄젠트 공식을 활용)
        float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
        fa.transform.Rotate(0, 0, angle - 90);

        iTween.MoveTo(fa, iTween.Hash("x", wp.x, "y", wp.y,
             "oncomplete", "ttemp", "oncompleteparams", fa,
             "oncompletetarget", this.gameObject));
    }

    void ttemp(GameObject fa)
    {
        light();
        Invoke("light",0.15f);
        Invoke("light", 0.3f);
        Destroy(fa); // 불화살 파괴
        
        // 불 생성
        fireObj = Instantiate(obj[selected_character], touchPos, Quaternion.identity) as GameObject;   // as는 왜 쓴건지 모름
        Destroy(fireObj.gameObject, skill_duration);    // n초 뒤 사라짐.

        for(int i =0; i < 4; i++)   // 위에서 기본 생성 후 추가로 주변에 불 몇 개 생성할건지 지정
        {
            Vector3 vec = touchPos;
            float randomX=0;
            float randomY = 0;
            do {
                randomX = Random.Range(-0.3f, 0.3f);    // 생성위치가 x축 0.6 반경내에 위치 하도록 랜덤값
            } while(Mathf.Abs(randomX) < 0.12f); // 원래 불과 근처에 있지 않게 떨어뜨림
            do
            {
                randomY = Random.Range(-0.3f, 0.3f);    // 생성위치가 y축 0.6 반경내에 위치 하도록 랜덤값
            } while (Mathf.Abs(randomY) < 0.12f); // 원래 불과 근처에 있지 않게 떨어뜨림

            vec += new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f),0); // 랜덤한 위치
            fireObj = Instantiate(obj[selected_character], vec, Quaternion.identity) as GameObject;   // as는 왜 쓴건지 모름
            Destroy(fireObj.gameObject, Random.Range(skill_duration-1, skill_duration));    // n초 뒤 사라짐.
        }
    }

    void light()
    {
        HitLight = GameObject.Find("HitLight").GetComponent<SpriteRenderer>();
        HitLight.color = new Color(255, 255, 255, 255);
        Invoke("lightOff",0.05f);
    }
    void lightOff()
    {
        HitLight.color = new Color(255, 255, 255, 0);   
    }
    void follow(Vector3 _obj, GameObject bull)
    {
        Rigidbody2D temp;
        float digree;

        temp = bull.GetComponent<Rigidbody2D>();
        Vector2 ToPlayerDir = _obj -
            new Vector3(bull.transform.position.x, bull.transform.position.y);
        temp.AddForce(ToPlayerDir.normalized * 2f);

        digree = Mathf.Atan2(bull.transform.position.y - _obj.y,
            bull.transform.position.x - _obj.x) * 180f / Mathf.PI;
        //bull.transform.Rotate(0, 0, digree);


    }
    public void summon_servant(int damage) // 마법사 스킬. 소환수였나?? 일단 매개변수로 소환수 공격력이나 체력 받으면 될듯. 아니면 시간이나.
    {
        Debug.Log("마법사 스킬");
        Vector3 pos = transform.position;
        pos.x += 1.0f;
        GameObject wizard_servant = Instantiate(obj[selected_character], pos, Quaternion.identity) as GameObject;
    } 
}