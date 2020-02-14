using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RealRunManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Update()
    {
        if (Application.platform == RuntimePlatform.Android)    // 안드로이드에서 뒤로가기 키 눌렀을 때 동작 설정.
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                PlayerPrefs.SetInt("player_gold", PlayerPrefs.GetInt("player_gold") + GameObject.Find("Pedometer").GetComponent<RealRun>().get_gold_count);
                PlayerPrefs.SetInt("shoes_count", PlayerPrefs.GetInt("shoes_count") + GameObject.Find("Pedometer").GetComponent<RealRun>().get_shoes_count);   // 신발 수 가져와서 추가함.
                GameObject.Find("Pedometer").GetComponent<RealRun>().OnDisable();   // 만보기 종료인듯.
                SceneManager.LoadScene("Main"); // 메인으로 이동
            }
        }

        if (Input.GetKey(KeyCode.Backspace))    // 키보드 백스페이스
        {
            PlayerPrefs.SetInt("player_gold", PlayerPrefs.GetInt("player_gold") + GameObject.Find("Pedometer").GetComponent<RealRun>().get_gold_count);
            PlayerPrefs.SetInt("shoes_count", PlayerPrefs.GetInt("shoes_count") + GameObject.Find("Pedometer").GetComponent<RealRun>().get_shoes_count);   // 신발 수 가져와서 추가함.
            GameObject.Find("Pedometer").GetComponent<RealRun>().OnDisable();   // 만보기 종료인듯.
            SceneManager.LoadScene("Main"); // 메인으로 이동
        }
    }
}
