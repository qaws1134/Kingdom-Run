using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{

    CameraShake S_Camera;
    SpriteRenderer HitLight;

    // 레벨
    [SerializeField] public int player_level = 1;

    //체력
    public float hp = 2.0f;
    public float initHp = 2.0f;
    public float maxHp = 2.0f;

    //이동속도
    public float speed = 300.0f;    //변동 이동속도
    public float initSpeed = 300.0f;    //기본 이동속도

    //공격력
    public float dmg = 1.0f;   // 변동 공격력
    public float initDmg = 1.0f; // 기존 공격력

    //공격 범위
    public int Area = 0;    //변동 공격범위
    public int initArea = 0; //기존 공격범위

    //점수
    public int Score = 0;   //변동 점수
    public int initScore = 0;// 기존 점수

    public float player_exp;
    public float next_exp = 60;   // 다음 레벨로 갈 때 필요한 경험치 초기값.

    // 체력바 관련
    public GameObject healthbarBackground;
    public Image healthfiled;
    public float currentHp = 0;

    //최대 룬슬롯
    public int slot = 0;

    public Rigidbody2D rb;
    public Transform tr;

    //이동
    float h;    // 좌우
    float v;    // 상하

    // 장비 관련
    public float EquipmentInitHp=0;
    public float EquipmentDmg = 0;
    public float EquipmentSpeed = 0;

    private void Awake()
    {
        currentHp = hp;
        healthfiled.fillAmount = 1f;

        switch (CharacterSelect.selected_character) // 캐릭터별 레벨을 가져옴.
        {
            case 0: // 기사
                player_level = PlayerPrefs.GetInt("Player_Level_Knight");
                player_exp = PlayerPrefs.GetFloat("Player_Exp_Knight");
                next_exp = next_exp * Mathf.Pow(1.1f, player_level - 1);
                break;
            case 1: // 궁수
                player_level = PlayerPrefs.GetInt("Player_Level_Archer");
                player_exp = PlayerPrefs.GetFloat("Player_Exp_Archer");
                next_exp = next_exp * Mathf.Pow(1.1f, player_level - 1);
                break;
            case 2: // 마법사
                if (PlayerPrefs.GetInt("Player_Level_Wizard") >= 1)
                    player_level = PlayerPrefs.GetInt("Player_Level_Wizard");
                player_exp = PlayerPrefs.GetFloat("Player_Exp_Wizard");
                next_exp = next_exp * Mathf.Pow(1.1f, player_level - 1);
                break;
        }

        Init_Status(player_level);
        S_Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

    }

    void Update()
    {


        healthfiled.fillAmount = hp / maxHp;
        healthbarBackground.SetActive(true);

        if (hp <= 0)    // 사망
        {
            S_Camera.VibrateFortime(0.1f); // 카메라 흔들림
            SceneManager.LoadScene("End");
        }

        //방향키 이동
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //이동속도
        Vector2 dir = new Vector2(h, v);
        rb.velocity = dir * speed * Time.deltaTime;

        // 화면 밖으로 나가지 못 하게
        float size = Camera.main.orthographicSize;
        float offset = 0.5f;


        if (tr.position.y >= size - offset - 0.3f)        // 화면 상단 넘지 못 하도록.
        {
            tr.position = new Vector3(tr.position.x, size - offset - 0.3f, 0);
        }
        if (tr.position.y <= -size + offset - 0.3f)       // 화면 하단 넘지 못 하도록. 
        {
            tr.position = new Vector3(tr.position.x, -size + offset - 0.3f, 0);
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float wSize = Camera.main.orthographicSize * screenRatio;
        if (tr.position.x >= wSize - offset)
        {
            tr.position = new Vector3(wSize - offset, tr.position.y, 0);
        }
        if (tr.position.x <= -wSize + offset)
        {
            tr.position = new Vector3(-wSize + offset, tr.position.y, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {


        if (coll.CompareTag("ENEMY") || coll.CompareTag("ENEMYBULLET"))
        {
            hp--;
            S_Camera.VibrateFortime(0.1f); // 카메라 흔들림
            StartCoroutine(light());
            if (hp <= 0)    //사망
            {
                SceneManager.LoadScene("End");
            }

        }
    }

    IEnumerator light()
    {
        HitLight = GameObject.Find("HitLight").GetComponent<SpriteRenderer>();
        HitLight.color = new Color(255, 255, 255, 255);
        yield return new WaitForSeconds(0.01f);
        HitLight.color = new Color(255, 255, 255, 0);
    }

    void Init_Status(int level)
    {
        for (int i = 1; i < level; i++)         // 플레이어 레벨에 따른 값 능력치 변화
        {
            initHp += 1;                        //initHp = Mathf.Round(initHp * 1.2f);    // 레벨마다 20%씩 증가.
            initDmg *= 1.2f;
            initSpeed = initSpeed * 1.01f;  // 레벨마다 1%씩 증가
        }

        hp = initHp;    // 게임이 시작될 때 플레이어 HP를 initHp로 초기화 해줌
        maxHp = initHp;
        dmg = initDmg;
        speed = initSpeed;
        Score = initScore;
        Area = initArea;
    }

    public void GetExp(float get_exp)   // 원래는 스테이터스를 이렇게 구현하는게 아니라 따로 클래스 만들어서 아이템 구현한 것처럼 구조체 형태로 능력치를 관리해야 되는데, 이건 매번 능력치를 계산 해야되네. 너무 구조를 크게 바꾸는거라 수정은 힘들고 일단 이렇게 가야할듯.
    {
        player_exp += get_exp;  // 몬스터로부터 획득한 경험치를 플레이어 경험치에 추가.

        while (player_exp >= next_exp)  // 레벨업 조건되면 레벨업 처리. 한번에 큰 경험치를 얻어 여러번 레벨업 할 경우에 대응하기 위해서 이렇게 코드짬.
        {
            // player_level +=(int)(player_exp / next_exp);
            player_level += 1;
            player_exp = player_exp - next_exp;  // 레벨업 후 잔여 경험치 
            next_exp = next_exp * 1.1f;

            // 레벨업으로 인한 능력치 향상
            initHp += 1;
            initDmg += 0.5f;
            initSpeed = initSpeed * 1.01f;  // 레벨마다 1%씩 증가
        }
        update_status();
        saveLevel();

    }

    public void update_status() // 능력치 갱신할 때 사용
    {
        maxHp = initHp + EquipmentInitHp;
        dmg = initDmg + EquipmentDmg;
        speed = initSpeed  + initSpeed * EquipmentSpeed;
    }

    public void saveLevel()
    {
        // Debug.Log("레벨 저장함수 호출");
        switch (CharacterSelect.selected_character)
        {
            case 0: // 기사
                PlayerPrefs.SetInt("Player_Level_Knight", player_level);
                PlayerPrefs.SetFloat("Player_Exp_Knight", player_exp);
                break;
            case 1: // 궁수
                PlayerPrefs.SetInt("Player_Level_Wizard", player_level);
                PlayerPrefs.SetFloat("Player_Exp_Archer", player_exp);
                break;
            case 2: // 마법사
                PlayerPrefs.SetInt("Player_Level_Wizard", player_level);
                PlayerPrefs.SetFloat("Player_Exp_Wizard", player_exp);
                break;
            default:
                Debug.Log("레벨 저장 에러");
                break;
        }
    }
}
