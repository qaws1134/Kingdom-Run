using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public ItemBuffer itemBuffer;
    public Transform slotRoot;
    public Text player_gold_Text;

    private List<Slot> slots;
    private int player_gold;
    public System.Action<ItemProperty> onSlotClick; // 함수를 연결해주는 델리게이트. 아이템 프로퍼티를 매개값으로 줘야함.
    // Start is called before the first frame update
    void Start()
    {
        slots = new List<Slot>();
        int slotCnt = slotRoot.childCount;
        printGold();
        for (int i = 0; i < slotCnt; i++)
        {
            var slot = slotRoot.GetChild(i).GetComponent<Slot>();   // Slot이라는 컴포넌트를 가져옴

            if (i < itemBuffer.items.Count)
            {
                slot.SetItem(itemBuffer.items[i]);
            }
            else //아이템이 없는 경우
            {
                slot.GetComponent<UnityEngine.UI.Button>().interactable = false;    // 아이템이 없는 칸은 버튼 클릭이 안 되도록 설정
            }

            slots.Add(slot);
        }
    }

    public void OnClickSlot(Slot slot)  // 외부에서 다른 아무 함수와 연결 시킨 뒤 클릭시 호출만 해주는 기능. 
    {
        if (onSlotClick != null)
        {
            onSlotClick(slot.item);
        }
    }
    public void onClickCloseBtn() {
        this.gameObject.SetActive(false);
    }

    public void printGold() // 사용자 보유 골드 상점에 출력
    {
        player_gold = PlayerPrefs.GetInt("player_gold");
        player_gold_Text.text = player_gold.ToString() + "G";
    }
}
