using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public Transform rootSlot;
    public EquipmentStore store;
    private List<Slot> slots;
    public EquipmentBuffer equipmentBuffer;

    private bool backpackToggle = false;
    // Start is called before the first frame update
    void Start()
    {
        
        slots = new List<Slot>();   //리스트 슬롯 초기화

        int slotCnt = rootSlot.childCount;  //자식 오브젝트의개수를 새줌 -> 인벤토리의 공간 

        for (int i = 0; i < slotCnt; i++)
        {
            var slot = rootSlot.GetChild(i).GetComponent<Slot>();   //자식 오브젝트에 GetChild를 사용하여 접근

            slots.Add(slot);        //인벤토리 slots 리스트에 각 슬롯을 추가 
        }

        store.onSlotClick += BuyItem;   //산 아이템을 가져와줌 델리게이트로 아이템을 저장
        load_equipment();
    }

    void BuyItem(ItemProperty item) //아이템 구매
    {
        var emptySlot = slots.Find(t  => {return t.item == null || t.item.itemName == string.Empty;} );  //Find를 사용하여 비어잇는 슬롯을 가져옴 
        if (emptySlot != null)
        {
            int player_gold = PlayerPrefs.GetInt("player_gold");
            if (item.price < player_gold)
            {
                emptySlot.SetItem(item);    //인벤토리에 클릭한 아이템을 채워줌
                emptySlot.text.enabled = false;             //가격을 text를 없애줌
                PlayerPrefs.SetInt("player_gold", player_gold - item.price);
                store.printGold(); //구매 후 사용자의 골드 다시 출력
            }
        }
    }

    void load_equipment()
    {
        int slotCnt = rootSlot.childCount;  //슬롯 개수를 샘
        for (int i = 0; i < slotCnt; i++)
        {
            var slot = rootSlot.GetChild(i).GetComponent<Slot>();   // Slot이라는 컴포넌트를 가져옴

            if (i < equipmentBuffer.items.Count && (equipmentBuffer.items[i].itemID == PlayerPrefs.GetInt("WeaponID") || equipmentBuffer.items[i].itemID == PlayerPrefs.GetInt("ArmorID")))
            {
                slot.SetItem(equipmentBuffer.items[i]);
                slot.text.enabled = false;
                GameObject.Find("Inventory_Equipment").GetComponent<EquipmentOnOff>().PutOnWeapon(PlayerPrefs.GetInt("WeaponID"));
                GameObject.Find("Inventory_Equipment").GetComponent<EquipmentOnOff>().PutOnArmor(PlayerPrefs.GetInt("ArmorID"));
                slot.onEquipEffect(slot);
            }
            slots.Add(slot);
            
        }
    }

    public void BtnBackpack()
    {
        if (backpackToggle)
        {
            this.gameObject.SetActive(false);
            backpackToggle = false;
        }
        else
        {
            this.gameObject.SetActive(true);
            backpackToggle = true;
        }
    }
}
