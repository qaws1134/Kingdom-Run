using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentOnOff : MonoBehaviour
{
    public int WeaponID = 0;    // 0이면 미장착이라는 뜻
    public int ArmorID = 0;
    public PlayerCtrl player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        
        // Weapon
        WeaponID = PlayerPrefs.GetInt("WeaponID");
        // Armor
        ArmorID = PlayerPrefs.GetInt("ArmorID");

    }

    // 장착
    public int PutOnWeapon(int _itemID)    // 웨폰
    {
        if (_itemID == 0)
            return -1;
        if (WeaponID == 0)   // 무기가 장착중이 아닐 경우에만 장착
        {
            WeaponID = _itemID;
            PlayerPrefs.SetInt("WeaponID", WeaponID);

            // 효과 적용. 공격력, 최대 체력, 스피드
            return WeaponID;
        }
        else
            return -1;
    }
    public int PutOnArmor(int _itemID) // 아머
    {
        if (_itemID == 0)
            return -1;

        if (ArmorID == 0)
        {
            ArmorID = _itemID;
            PlayerPrefs.SetInt("ArmorID", ArmorID);
            return ArmorID;
            // 효과 적용. 공격력, 최대 체력, 스피드
        }
        else
            return -1;
    }

    // 탈착
    public int TakeOffWeapon(int _itemID)   // 웨폰
    {
        if (_itemID == WeaponID)
        {
            WeaponID = 0;
            PlayerPrefs.SetInt("WeaponID", 0);
            return _itemID;
        }

        else // ID가 같지 않으므로 벗지 않음.
            return -1;
    }
    public int TakeOffArmor(int _itemID)    // 아머
    {
        if (_itemID == ArmorID) 
        {
            ArmorID = 0;
            PlayerPrefs.SetInt("ArmorID", 0);
            return _itemID;
        }

        else // ID가 같지 않으므로 벗지 않음.
            return -1;
    }
    
}
