using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBuffer : MonoBehaviour
{
    public List<ItemProperty> items = new List<ItemProperty>();

    // Start is called before the first frame update
    public void Start()
    {
        /*
        ItemProperty(
            int _itemID, string _itemName = "", string _itemDes = "",
            int _price = 0, float _atk = 0, float _maxHpUp = 0, float _speed = 1.0f,
            float _recover_hp = 0, float _recover_hp_percent = 1.0f, float _reduce_cooltime = 1.0f)
        */

        // 괄호 안은 기본값. 기본값일시 더하더라도 변화를 안 주므로 해당 아이템은 기능을 가지지 않음을 의미. 
        // 아이템 ID(필수), 이름(""), 설명(""), 가격(0), 공격력증가(0), 최대체력 증가(0), 스피드 증가(0), 
        // 체력 회복(0), 체력회복 퍼센트(0), 쿨타임 감소(1) //

        // items.Add(new ItemProperty(아이디, "이름", "설명", 가격, 0, 0, 0, 0, 0, 1.0f)); // 기본값

        // 3001 ~ 4000  무기
        items.Add(new ItemProperty(3001, "강철검", "공격력+1", 1000, 1, 0, 0, 0, 0, 1.0f));
        items.Add(new ItemProperty(3002, "황금검", "공격력+3", 3000, 3, 0, 0, 0, 0, 1.0f));
        items.Add(new ItemProperty(3003, "암흑검", "공격력+5, 스피드 -20%", 4000, 5, 0, -0.2f, 0, 0, 1.0f));

        // 4001 ~5000 방어구
        items.Add(new ItemProperty(4001, "강철 방패", "최대체력 +5", 1000, 0, 5, 0, 0, 0, 1.0f));
        items.Add(new ItemProperty(4002, "강철 갑옷", "최대체력 +15", 3000, 0, 5, 0, 0, 0, 1.0f));
        // 아직 미구현 items.Add(new ItemProperty(4003, "쾌속 신발", "이동속도 30% 증가", 1500, 0, 0, 0.3f, 0, 0, 1.0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
