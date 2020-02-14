using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 적 몬스터의 리스폰을 관리하는 메서드
public class RespawnManager2 : MonoBehaviour
{

    public GameObject[] obj;        // 0번이 보스 , 1,2는 일반 몬스터, 3,4는 특수 몬스터


    public List<GameObject> enemy_obj = new List<GameObject>();    // 적 객체가 생성되면 들어갈 곳.
    public Transform[] respawnTr; // 리스폰 위치
    public float respawn_delay = 3.0f;   // 리스폰 되는 시간
    public int[] spm_chk = { 0, 0 };

    public int respawn_count = 0;

    int Enemy_MaxCount = 4;
    int Enemy_MinCount = 2;

    bool merchant_respawn = false;  // 상인이 등장했었는지 체크 (중복 등장 방지)
    bool equipmerchant_respawn = false;  // 상인이 등장했었는지 체크 (중복 등장 방지)
    bool saint_respawn = false;  // 성자가 등장했었는지 체크 (중복 등장 방지)

    void Start()
    {
        respawnTr = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        if (respawnTr.Length > 0)
        {
            StartCoroutine(RespawnEnemy()); // 몬스터 생성 코루틴 함수 호출
        }
    }
    public void Update()
    {
        if (enemy_obj.Count == 0)       // 적이 0마리면 앞으로 진행
        {
            mapScrolling_toggle(true);
        }
    }
    public IEnumerator RespawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawn_delay);        // 적 리스폰 시간

            for (int i = 0; i < enemy_obj.Count; i++)   // 적이 모종의 이유로 missing 돼서 없어질 경우 오브젝트 리스트에서 알아서 제거해줌.
                if (enemy_obj[i] == null)
                    enemy_obj.Remove(enemy_obj[i]);

            if (enemy_obj.Count > 0)
                continue;

            int spm_num = 0;
            int enemy_num = Random.Range(Enemy_MinCount, Enemy_MaxCount + 1); // 한번에 나올 적 숫자

            int[] idx = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < Enemy_MaxCount; i++)
            {
                int temp = Random.Range(1, respawnTr.Length);

                if (idx[temp] == 0)
                {
                    idx[temp] = 1;
                }
                else if (i != 0 && idx[temp] == 1)      // 이미 있는 자리일 경우 랜덤을 다시 돌려주기 위함
                    i--;
            }

           
            if (respawn_count == 1)
            {
                obj[8].SetActive(true);
                equipmerchant_respawn = true;
            }
            if (respawn_count == 2)  
            {
                obj[7].SetActive(true);
                saint_respawn = true;
            }
            if (respawn_count == 3)  
            {
                obj[6].SetActive(true);
                merchant_respawn = true;
            }


            if (respawn_count < 3) //일반몬스터만
            {
      
                for (int i = 0; i < enemy_num; i++)
                    enemy_obj.Add(Instantiate(obj[Random.Range(1, 3)], respawnTr[res_pos(idx)].transform.position, Quaternion.identity)); // Quaternion.identity는 원래 각도대로 리스폰 하라는 뜻
            }
            else if (respawn_count >= 3 && respawn_count < 15) //특수몬스터 출현
            {
                for (int i = 0; i < enemy_num; i++)
                {
                    int temp = Random.Range(1, 5);  // 1<= temp <=4 // 0번은 보스라 제외
                    if (spm_chk[1] == 0)   // 2열이 비었을 경우에만 특수몬스터 등장 가능
                    {
                        enemy_obj.Add(Instantiate(obj[temp], respawnTr[res_pos(idx)].transform.position, Quaternion.identity));
                        if (temp >= 3) // 랜덤으로 나온게 특수 몬스터라는 뜻
                            spm_num++;
                    }
                    else
                        enemy_obj.Add(Instantiate(obj[Random.Range(1, 3)], respawnTr[res_pos(idx)].transform.position, Quaternion.identity)); // 특수 몬스터가 2열이 꽉 찼을 시 그냥 일반 몬스터 출력
                }
            }
            else if (respawn_count == 15)                                                               // 보스 몬스터 소환
                enemy_obj.Add(Instantiate(obj[0], respawnTr[0].transform.position, Quaternion.identity));

            // respawnTr.position + new Vector3(Random.Range(-range, range), 0, 0) 은 리스폰 포지션 중앙점에서 랜덤으로 range 범위내에서+- x축을 변형. y와 z축은 0으로서 변경 안 한다는 뜻. 

            switch (respawn_count)
            {
                case 1:
                    Enemy_MaxCount++;
                    break;
                case 3:
                    Enemy_MaxCount++;
                    Enemy_MinCount++;
                    break;
                case 5:
                    Enemy_MaxCount = 4;
                    break;
                case 7:
                    Enemy_MaxCount++;
                    Enemy_MinCount++;
                    break;
                case 9:
                    Enemy_MaxCount++;
                    break;
            }
            respawn_count++;
            //Debug.Log("respawn_count : " + respawn_count);

            if (spm_num != 0)
            {
                if (spm_chk[0] == 0)
                    spm_chk[0] = spm_num;
                else if (spm_chk[0] != 0 && spm_chk[1] == 0)
                {
                    spm_chk[1] = spm_num;
                }
                else Debug.Log("spm_count error");
            }
            //Debug.Log("spm_chk [0] : " + spm_chk[0] + " spm_chk [1] : " + spm_chk[1]);

            if (enemy_obj.Count > 0)
            {        // 살아있는 적이 있다면 화면 멈춤
                mapScrolling_toggle(false);
                respawn_delay = 6.5f;   // 코루틴 호출 시간 늘림.
            }
        }
    }

    int res_pos(int[] idx)
    {
        for (int i = 0; i < idx.Length; i++)
            if (idx[i] != 0)
            {
                idx[i] = 0; // 해당 위치는 사용했으니 초기화
                return i;
            }
        return 0;
    }

    public void summon_monster(int enemy_num, int offset)
    {
        int[] idx = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < enemy_num; i++)
        {
            int temp = Random.Range(1, respawnTr.Length);

            if (idx[temp] == 0)
            {
                idx[temp] = 1;
            }
            else if (i != 0 && idx[temp] == 1)      // 이미 있는 자리일 경우 랜덤을 다시 돌려주기 위함
                i--;
        }
        for (int i = 0; i < enemy_num; i++)
        {
            Instantiate(obj[Random.Range(1, 3)], respawnTr[res_pos(idx)].transform.position + new Vector3(0, Random.Range(0, 2) + offset, 0), Quaternion.identity); // Quaternion.identity는 원래 각도대로 리스폰.}
            if (i % 2 == 0)
                Instantiate(obj[5], respawnTr[Random.Range(1, 7)].transform.position + new Vector3(0, Random.Range(0, 2) + offset, 0), Quaternion.identity); // 빨간 고블린 몇 마리 소환해보고 싶었어
        }
    }

    public void mapScrolling_toggle(bool tf)    // true면 맵스크롤링 시작, false면 멈춤
    {
        var map1 = GameObject.Find("Map1").GetComponent<BackgroundScroll>();
        var map2 = GameObject.Find("Map2").GetComponent<BackgroundScroll>();
        map1.scrolling_toggle(tf);
        map2.scrolling_toggle(tf);
    }
}
