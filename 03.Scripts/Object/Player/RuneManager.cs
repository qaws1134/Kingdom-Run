using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapArray
{
    public GameObject[] Rune;
}

//룬 아이템을 관리해주는 스크립트
public class RuneManager : MonoBehaviour
{
    //룬 아이템 효과의 on off 기능을 담당하는 변수들
    public bool Red = false;
    public bool Yellow = false;
    public bool Blue = false;
    public bool Purple = false;



    public GameObject FirePos1;
    public GameObject FirePos2;


    int slotCount = 0;//룬의 슬롯을 카운트를해줌
    public int NowSlot = 0;  //현재위치한 슬롯
    public int Ground_RuneNum = 0;// 땅에 있는 룬갯수 확인
    public bool on = false;

    public int[] slot;
  
    public MapArray[] UISlot;

    private void Start()
    {
        slot = new int[2];
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //룬과 충돌시
        if (coll.CompareTag("RUNE"))
        {
            //룬슬롯에 룬이 1개만 있을 경우
            if (slotCount < 2)
            {
                RundEffect();   //획득한 룬효과 적용
            }
            else //룬이 2개 일 경우
            {
                RuneOut();  //FIFO 먼저 들어온 룬을 삭제해준다
                RundEffect(); //획득한 룬효과를 적용
           
                //  doubleRuneEffect();
            }
        }
    }

    void RundEffect()
    {
        var PlayerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        var FireCtrl = GameObject.Find("Player").GetComponent<FireCtrl>();

        Ground_RuneNum--;   //룬을 먹었을 시 땅에 있는 룬 갯수를 하나 없앰
            
        if (slotCount >1)
            UISlot[1].Rune[slot[NowSlot]].SetActive(false); //룬이 2개일 때 2번째 ui를 비활성화함

        //룬을 먹었을 시 룬을 캐릭터 슬롯에 저장 후 룬 효과 발동
        if (Red == true)
        {
            PlayerCtrl.dmg += 1;
            slot[NowSlot] = 0;
            Red = false;
        }
        if (Blue == true)
        {
            PlayerCtrl.hp += 1;
            slot[NowSlot] = 1;
            Blue = false;
        }
        if (Yellow == true)
        {
            FireCtrl.Bullet_State = (int)FireCtrl.bullet_shot.double_shot_ready;
            slot[NowSlot] = 2;
            Yellow = false;
        }   
        if (Purple == true)
        {
            PlayerCtrl.speed += 100f;
            slot[NowSlot] = 3;
            Purple = false;
        }

        if (slotCount < 2)
            slotCount++;

        UISlot[NowSlot].Rune[slot[NowSlot]].SetActive(true);

        //룬 슬롯을 늘림
        if (NowSlot < 1)
            NowSlot++;
        else  //룬슬롯은 최대 2
            NowSlot = 1;

    }

    void RuneOut()  //2개 이상의 룬을 가졌을 때 룬을 먹게 되면 먼저들어온 룬의 효과를 뺌 
    {
        var PlayerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        var FireCtrl = GameObject.Find("Player").GetComponent<FireCtrl>();

        if (slot[0] == 0)
        {
            PlayerCtrl.dmg -= 1;
        }
        else if (slot[0] == 1)
        {
            PlayerCtrl.hp -= 1;
        }
        else if (slot[0] == 2)
        {
            FirePos1.transform.Translate(0.5f, 0, 0);
            FirePos2.transform.Translate(-0.5f, 0, 0);
            FireCtrl.Bullet_State = (int)FireCtrl.bullet_shot.one_shot;
        }
        else if (slot[0] == 3)
        {
            PlayerCtrl.speed -= 100f;
        }

        UISlot[0].Rune[slot[0]].SetActive(false);

        slot[0] = slot[1];  //2번째 슬롯의 룬을 첫번째 슬롯으로 옮김

        UISlot[0].Rune[slot[0]].SetActive(true);


    }
}
