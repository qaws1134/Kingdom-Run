using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [HideInInspector]   // 이 애트리뷰트를 쓰면 public으로 했지만 에디터에서 표시 안 하도록 해줌
    public ItemProperty item;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Text text;
    public PlayerCtrl player;

    private void Awake()
    {
        SetSellBtnInteractable(false);
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    void SetSellBtnInteractable(bool b)
    {
        GetComponent<UnityEngine.UI.Button>().interactable = b;
    }

    public void SetItem(ItemProperty item)
    {
        this.item = item;
        if (item == null)
        {
            image.enabled = false;
            text.enabled = false;
            SetSellBtnInteractable(false);
            gameObject.name = "Empty";  // 아이템이 존재하지 않으면 Empty라고 이름 적어줌.
        }

        else
        {
            image.enabled = true;
            text.enabled = true;
            gameObject.name = item.itemName;
            // image.sprite = item.sprite;
            image.sprite = Resources.Load("Item/" + item.itemID.ToString(), typeof(Sprite)) as Sprite;
            if (item.price > PlayerPrefs.GetInt("player_gold"))
                text.color = Color.red;
            text.text = item.price.ToString() + "G";
            SetSellBtnInteractable(true);
        }
    }

    public void UseItem()
    {
        Debug.Log("this.item.itemID : " + this.item.itemID);

        if (this.item.itemID >= 1001 & this.item.itemID <= 2000)    // 소모성 아이템 : 체력 회복, 스킬 쿨타임 초기화 등 
        {
            Debug.Log("소모성 아이템");
            player.hp += this.item.recover_hp;   // 아이템에 설정된 회복량 만큼 현재 체력 회복.
            player.hp += player.maxHp * this.item.recover_hp_percent;  // 배율 회복. 0배면 그대로. -0.3배면 최대 체력의 30%만큼 감소. 죽을 수도 있음.
            if (player.hp > player.maxHp)          // 회복량이 최대 체력을 넘어가지 않도록 해줌.
                player.hp = player.maxHp;
        }

        else if (this.item.itemID >= 2001 & this.item.itemID <= 3000)   // 지속시간 아이템(일정 시간 속도 Up, 일정 시간 공격력 Up)
        {
            Debug.Log("지속성 아이템");
            // 회복 공통    // 어차피 0이면 회복 안 들어감.
            player.hp += this.item.recover_hp;   // 아이템에 설정된 회복량 만큼 현재 체력 회복.
            player.hp += player.maxHp * this.item.recover_hp_percent;  // 배율 회복. 0배면 그대로. -0.3배면 최대 체력의 30%만큼 감소. 죽을 수도 있음.
            if (player.hp > player.maxHp)          // 회복량이 최대 체력을 넘어가지 않도록 해줌.
                player.hp = player.maxHp;

            // 효과 적용 코드
            onItemEffect();
            // 일정 시간 후 효과 해제 코드 // 효과 적용 코드에 Invoke로 몇 초 뒤 해제 코드 호출 하도록 구현함.
        }
        else if (this.item.itemID >= 3001 & this.item.itemID <= 4000)   // 무기
        {

            EquipmentOnOff eoo = GameObject.Find("Inventory_Equipment").GetComponent<EquipmentOnOff>();
            // 장착 함수 구현
            if (eoo.WeaponID == 0)    // 무기를 안 들고 있다는 뜻
            {
                Debug.Log("무기 장착 : " + eoo.WeaponID);
                if(eoo.PutOnWeapon(this.item.itemID)!=-1)
                    onEquipEffect(this);
            }

            // 장착 해제 함수 구현 
            else
            {
                Debug.Log("무기 해제 : " + eoo.WeaponID);
                if(eoo.TakeOffWeapon(this.item.itemID)!=-1)   // 아이템 아이디가 리턴됨.
                    offEquipEffect(this);
            }

        }

        else if (this.item.itemID >= 4001 & this.item.itemID <= 5000)   // 방어구
        {
            EquipmentOnOff eoo = GameObject.Find("Inventory_Equipment").GetComponent<EquipmentOnOff>();

            // 장착 함수 구현
            if (eoo.ArmorID == 0)    // 무기를 안 들고 있다는 뜻
            {
                Debug.Log("아머 장착 : " + eoo.ArmorID);
                if(eoo.PutOnArmor(this.item.itemID)!=-1)
                    onEquipEffect(this);
            }
            // 장착 해제 함수 구현 
            else
            {
                Debug.Log("아머 해제 : " + eoo.ArmorID);
                if(eoo.TakeOffArmor(this.item.itemID)!=-1)    // 아이템 아이디가 리턴됨.
                    offEquipEffect(this);
            }
        }
        else
        {
            Debug.Log("미지정 아이템. 아이템의 itemID를 확인해주세요. itemID : " + this.item.itemID);
        }
    }
    void onItemEffect()
    {
        player.GetComponent<Skill>().reduce_cooltime = this.item.reduce_cooltime;
        Invoke("offItemEffect", 25.0f);     // 지정 시간 뒤 아이템 효과 해제.
    }

    void offItemEffect()
    {
        player.GetComponent<Skill>().reduce_cooltime = 1.0f;
    }

    public void onEquipEffect(Slot slot)     // 장비 능력 적용
    {
        player.EquipmentInitHp += slot.item.maxHpUp; // 이렇게 하면 안 되고 별도로 장비 변수 만들어서 플러스 해줘야할듯. 중첩할 수도 있으니.
        player.EquipmentDmg += slot.item.atk;
        player.EquipmentSpeed += slot.item.speed;
        player.update_status();
    }

    public void offEquipEffect(Slot slot)   // 장비 능력 해제
    {
        player.EquipmentInitHp -= slot.item.maxHpUp; // 이렇게 하면 안 되고 별도로 장비 변수 만들어서 플러스 해줘야할듯. 중첩할 수도 있으니.
        player.EquipmentDmg -= slot.item.atk;
        player.EquipmentSpeed -= slot.item.speed;
        player.update_status();
    }
    public void OnClickSellBtn()
    {
        UseItem();
        if (this.item.itemID > 3000)    // 3001부터는 장비 아이템
        {
            //            GetComponent<UnityEngine.UI.Button>().interactable = false;
            return;
        }
        else
            SetItem(null);
    }
}
