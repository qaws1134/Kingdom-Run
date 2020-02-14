using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    public Text scoreText;
    public void Start()
    {
        scoreText.text = "SCORE\n\n" + GameManager.score.ToString("000000");       // score로 연결해준 텍스트 UI에 값을 업데이트 시켜줌
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void Restart() {
        SceneManager.LoadScene("Main");
    }
}
