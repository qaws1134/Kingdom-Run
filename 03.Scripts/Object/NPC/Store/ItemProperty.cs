using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // 클래스를 직렬화하여 퍼블릭 필드들을 밖으로 빼줌
public class ItemProperty
{
    public Sprite sprite;
    public int itemID = 0;

    // itemID 1001 ~ 2000 회복성 아이템    (Hp 회복, 스킬 쿨타임 초기화)
    // itemID 2001 ~ 3000 지속시간 아이템 (일정 시간 속도 Up, 일정 시간 공격력 Up)
    // itemID 3001 ~ 4000 장비형 아이템    (장착시 공격력, 체력 증가. 탈착시 다시 감소)

    public string itemName  = "";
    public string itemDescription = "";
    public int price = 0;

    // 장착형
    public float atk = 0; // 공격력
    public float maxHpUp = 0;  // 최대 체력

    // 지속형 (일정시간 작동후 사라짐. 장착형과 비슷하나 시간이 지나면 자동으로 해제된다는게 차이점.)
    public float speed = 1.0f;  // 1배면 그대로

    // 소비형
    public float recover_hp = 0;  // 값 체력 회복
    public float recover_hp_percent = 1.0f; // 배율 회복. 1배면 그대로.
    public float reduce_cooltime = 1f;    // ex) 1.5 = 50% 빨리 쿨타임 참.
    
    public ItemProperty(int _itemID, string _itemName = "", string _itemDes = "",
        int _price = 0, float _atk = 0, float _maxHpUp = 0, float _speed = 1.0f,
        float _recover_hp = 0, float _recover_hp_percent = 1.0f, float _reduce_cooltime = 1.0f)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;

        price = _price;
        atk = _atk;
        maxHpUp = _maxHpUp;
        speed = _speed;

        recover_hp = _recover_hp;
        recover_hp_percent = _recover_hp_percent;
        reduce_cooltime = _reduce_cooltime;

        sprite = Resources.Load("Item/" + itemID.ToString(), typeof(Sprite)) as Sprite;
    }
}
