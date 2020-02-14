using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//시작 씬에서 캐릭터를 선택하는 스크립트
public class CharacterSelect : MonoBehaviour
{
    public static int selected_character; // 0 : 기사, 1 : 궁수, 2 : 마법사

    public Image[] img;
    // Start is called before the first frame update
    public void Start()
    {
        selected_character = 0;
        img[selected_character].enabled = true;
    }
    public void LbtnClick()
    {
        img[selected_character].enabled = false;
        if (--selected_character <= -1)
        {
            selected_character = 2;
        }
        img[selected_character].enabled = true;
    }

    public void RbtnClick()
    {
        img[selected_character].enabled = false;
        if (++selected_character >= 3)
        {
            selected_character = 0;
        }

        img[selected_character].enabled = true;
    }
}
