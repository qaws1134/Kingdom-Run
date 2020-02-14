using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int score = 0;
    public static int extraScore = 1500;
    public int HighScore = 0;
    public static int BossComplete = 0;   // 1 : 보스 잡고 스테이지 클리어, 0 : 보스 아직 안 잡음
    public Text scoreText;
    public Text HighScoreText;

    void Awake()
    {
        score = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(900, 1600, true);
    }
    void Start()
    {
        StartCoroutine(Score());
    }

    public void Update()
    {
        if (Application.platform == RuntimePlatform.Android)    // 안드로이드에서 뒤로가기 키 눌렀을 때 동작 설정.
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main");
            }
        }
    }

    IEnumerator Score()
    {
        HighScore = PlayerPrefs.GetInt("HighScore");
        while (true)
        {

            scoreText.text = "" + score.ToString("000000");       // score로 연결해준 텍스트 UI에 값을 업데이트 시켜줌

            if (extraScore > 0)
                extraScore -= 10;
            else
                extraScore = 0;
            if (BossComplete == 1)
            {
                BossComplete = 0;
                score += extraScore;
            }
            ////////////////////////////////////////
            if (score > HighScore)
            {
                HighScore = score;
                PlayerPrefs.SetInt("HighScore", HighScore);
            }
            HighScoreText.text = "" + HighScore.ToString("000000");
            ///////////////////////////////////////
            //Debug.Log(Time.realtimeSinceStartup); // 게임 시작후 1초씩 타이머
            yield return new WaitForSeconds(0.5f);
        }
    }
}
