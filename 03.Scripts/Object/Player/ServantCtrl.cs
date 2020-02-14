using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServantCtrl : MonoBehaviour
{
    public GameObject bullet;
    public GameObject firepos;

    public GameObject healthbarBackground;
    public Image healthfiled;

    public float summon_time = 7.0f;
    public float remain_time;
    public int ch=1;    // 좌우로 미사일 쏘는 방향 왔다갔다 할 때, 좌우 끝에 도달하면 토글용 변수 (1, -1)

    public Transform player;
    public Vector2 pos;
    public float offset_x = 0;
    public float offset_y = 0; // 소환수 y축 위치가 이상할 경우 조정값

    public int oneShotNum = 20;
    public int speed = 70;

    public int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        summon_time = summon_time * Mathf.Pow(1.025f, PlayerPrefs.GetInt("Player_Level_Wizard"));     // 위자드 레벨 가져와서 레벨마다 2.5%씩 시간 증가. 30레벨쯤 2배.
        remain_time = summon_time;
        healthfiled.fillAmount = 1f;

        player = GameObject.Find("Player").GetComponent<Transform>();
        InvokeRepeating("Fire2", 0.5f, 0.18f);
        Invoke("Destroy_Servant", summon_time);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            offset_x *= -1;

        // 소환수가 플레이어 주변 회전하지 않고 고정. 근데 이거 해줄거면 스크립트 중에 로테이트
        pos = player.position;
        pos.x = pos.x + offset_x;
        pos.y = pos.y + offset_y;
        this.transform.position = pos;

        remain_time -= Time.deltaTime;
        healthfiled.fillAmount = remain_time / summon_time;
    }

    public void Fire()
    {
        i *= -1;
        GameObject obj;
        obj = (GameObject)Instantiate(bullet, firepos.transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / oneShotNum), speed * Mathf.Sin(Mathf.PI * i * 2 / oneShotNum)));
        obj.transform.Rotate(new Vector3(0f, 0f, 360 * i / oneShotNum - 90)); // 360도 회전 공격

        i *= -1;
        if (++i >= oneShotNum) i = 0;

    }

    public void Fire2() // 전방 몇 도 정도만 쏘는 공격
    {
        
        GameObject obj;
        obj = (GameObject)Instantiate(bullet, firepos.transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / oneShotNum), speed * Mathf.Sin(Mathf.PI * i * 2 / oneShotNum)));
        obj.transform.Rotate(new Vector3(0f, 0f, -90 * i / oneShotNum + 47));

        i += ch;
        if (i >= oneShotNum) ch *= -1;
        else if (i <= 0) ch *= -1;

    }

    public void Destroy_Servant()
    {
        Destroy(this.gameObject);
    }
}
