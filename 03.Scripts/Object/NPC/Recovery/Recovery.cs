using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recovery : MonoBehaviour
{
    private void Awake()
    {
        var pc = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        // Debug.Log("회복 전 : " + pc.hp);
        pc.hp = pc.initHp;  // 체력 전부 회복.
        if ((int)(pc.hp * 1.5f)<=pc.initHp) pc.hp = (int)(pc.hp*1.5f);  // 이런 식으로 회복량 조절 가능
        // Debug.Log("회복 후 : "+pc.hp);
    }
    public void onClickCloseBtn()
    {
        this.gameObject.SetActive(false);
    }
}
