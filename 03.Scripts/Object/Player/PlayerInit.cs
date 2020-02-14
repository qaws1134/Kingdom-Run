using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//선택한 캐릭터를 나타내는 스크립트
public class PlayerInit : MonoBehaviour
{
    private Animator ani;
    public RuntimeAnimatorController[] controller;
    public Transform Character;
    public Transform shadow;

    
    void Start()
    {
        ani = GetComponent<Animator>();

        if (CharacterSelect.selected_character == 1)      //궁수일때
        {
            Character.localScale = new Vector3(0.5f, 0.45f);
            shadow.Translate(-0.02f, -0.33f, 0);
        }
        else if (CharacterSelect.selected_character == 2)//마법사일때
        {
            Character.localScale = new Vector3(0.4f, 0.45f);
            shadow.Translate(0.28f, -0.05f, 0);
        }

        ani.runtimeAnimatorController = controller[CharacterSelect.selected_character]; // 플레이어의 모습을 담당하는 애니메이션을 직업별로 바꿈.

        JoyStick2.Player = transform;   // 조이스틱과 플레이어 오브젝트의 transform을 연결.
    }
}
