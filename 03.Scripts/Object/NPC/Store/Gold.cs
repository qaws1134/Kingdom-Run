using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public int goldValue = 0;      // 드랍된 골드의 가치
    private int player_gold;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("PLAYER"))
        {
            player_gold = PlayerPrefs.GetInt("player_gold");
            player_gold += goldValue;  // 플레이어 원래 자산에 골드 더함
            PlayerPrefs.SetInt("player_gold", player_gold); // 더해진 골드 값을 저장.
            Debug.Log("플레이어의 재산 : " + PlayerPrefs.GetInt("player_gold"));
            Destroy(this.gameObject);
        }
        else if (coll.CompareTag("DESTROY"))
        {
            Destroy(this.gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
