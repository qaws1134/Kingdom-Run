using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//시작 씬의 ui를 관리하는 스크립트
public class UIManager : MonoBehaviour
{
    //신발을 관리하는 변수
    public Text shoes_count_text;
    public int shoes_count;

    public void Start()
    {
        shoes_count = PlayerPrefs.GetInt("shoes_count");
        print_shoes_count();
    }

    // 게임 시작 버튼 클릭시 해당 씬으로 이동
    public void OnStartClick()         
    {
        if (shoes_count > 0)
        {               // 신발이 없으면 게임을 못 합니다.
            SceneManager.LoadScene("Play");
            shoes_control(-1);  // 게임 플레이시 신발 1개 사용.
        }
    }

    //RealRun 화면으로 이동
    public void OnClickRealRun()         
    {
        SceneManager.LoadScene("RealRun");
    }

    //어플리케이션 종료
    public void OnExitClick()
    {
        Application.Quit();
    }

    public void print_shoes_count()
    {
        shoes_count_text.text = "x " + shoes_count;
    }

    public void shoes_control(int num)
    {
        shoes_count += num;
        PlayerPrefs.SetInt("shoes_count", shoes_count);
        print_shoes_count();
    }

    public void shoes_btn()
    {
        shoes_control(1);
    }


    
}
