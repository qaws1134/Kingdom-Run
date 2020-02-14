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
        slots = new List<Slot>();

        int slotCnt = rootSlot.childCount;

        for (int i = 0; i < slotCnt; i++)
        {
            var slot = rootSlot.GetChild(i).GetComponent<Slot>();

            slots.Add(slot);
        }

        store.onSlotClick += BuyItem;
        load_equipment();
    }

    void BuyItem(ItemProperty item)
    {
        var emptySlot = slots.Find(t =>
        {
            return t.item == null || t.item.itemName == string.Empty;
        });
        if (emptySlot != null)
        {
            int player_gold = PlayerPrefs.GetInt("player_gold");
            if (item.price < player_gold)
            {
                emptySlot.SetItem(item);
                emptySlot.text.enabled = false;
                PlayerPrefs.SetInt("player_gold", player_gold - item.price);
                store.printGold();
            }
        }
    }

    void load_equipment()
    {
        int slotCnt = rootSlot.childCount;
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
