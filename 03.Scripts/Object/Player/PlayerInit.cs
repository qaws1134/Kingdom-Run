using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//선택한 캐릭터를 나타내는 스크립트
public class PlayerInit : MonoBehaviour
{
    private Animator ani;
    public RuntimeAnimatorController[] controller;
    public BoxCollider2D box;
    public Transform Character;
    public Transform firepos1;
    public Transform firepos2;
    public Transform shadow;

    
    void Start()
    {
        ani = GetComponent<Animator>();

        if (CharacterSelect.selected_character == 1)      //궁수일때
        {
            firepos1.position = new Vector3(0, -3.8f);
            firepos2.position = new Vector3(0, -3.8f);
            Character.localScale = new Vector3(0.4f, 0.45f);
            box.offset = new Vector2(0f, 0.35f);
            shadow.Translate(-0.02f, -0.33f, 0);
        }
        else if (CharacterSelect.selected_character == 2)//마법사일때
        {
            firepos1.position = new Vector3(0.4f, -3.6f);
            firepos2.position = new Vector3(0.4f, -3.6f);
            Character.localScale = new Vector3(0.4f, 0.45f);
            box.offset = new Vector2(0.7f, 0.9f);
            box.size = new Vector2(1.7f, 1.95f);
            shadow.Translate(0.28f, -0.05f, 0);
        }

        ani.runtimeAnimatorController = controller[CharacterSelect.selected_character]; // 플레이어의 모습을 담당하는 애니메이션을 직업별로 바꿈.

        JoyStick2.Player = transform;   // 조이스틱과 플레이어 오브젝트의 transform을 연결.
    }
}
