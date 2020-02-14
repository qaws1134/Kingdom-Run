using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuffer : MonoBehaviour
{
    public List<ItemProperty> items = new List<ItemProperty>();

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

        // 1001 ~ 2000 즉시 소모성 아이템
        items.Add(new ItemProperty(1001, "HP 포션", "최대 체력의 30%만큼 회복", 150, 0, 0, 0, 0, 0.3f, 1));
        items.Add(new ItemProperty(1002, "닭다리", "체력+5", 50, 0, 0, 0, 5, 0, 1));
        
        // 2001 ~ 3000 지속형 아이템
        items.Add(new ItemProperty(2001, "쿨타임 포션", "스킬 쿨타임 2배 빨라짐", 150, 0, 0, 0, 0, 0, 2.0f));
        items.Add(new ItemProperty(2002, "빨간 고추", "스킬 쿨타임 1.3배 빨라짐, 현재 체력 감소 최대 체력의 20%", 20, 0, 0, 0, 0, -0.2f, 1.3f));
    }
}


