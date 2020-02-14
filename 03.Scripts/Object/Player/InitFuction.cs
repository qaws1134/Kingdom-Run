using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitFuction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;
    public PlayerCtrl pc;
    bool SettingWindowToggle=false;
    void Start()
    {
        
    }

    public void onClickDeveloperBtn()
    {
        if (SettingWindowToggle)
        {
            this.gameObject.SetActive(false);
            SettingWindowToggle = false;
        }
        else
        {
            this.gameObject.SetActive(true);
            SettingWindowToggle = true;
        }
    }

    public void InitHighScore()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        GameManager.score = 0;
        gm.HighScore = 0;
        gm.HighScoreText.text = "000000";
    }

    public void InitPlayerGold()
    {
        if(PlayerPrefs.GetInt("player_gold")>0)
            PlayerPrefs.SetInt("player_gold",0);
        else
        {
            PlayerPrefs.SetInt("player_gold", 100000);
        }
        try
        {
            GameObject.Find("Store").GetComponent<Store>().printGold();
        }
        catch { }
    }

    public void GoToMain() {
        SceneManager.LoadScene("Main");
    }

    public void InitPlayerLevel()
    {
        switch (CharacterSelect.selected_character)
        {
            case 0: // 기사
                pc.player_level = 1;
                pc.player_exp = 0;
                pc.next_exp = pc.next_exp * Mathf.Pow(1.1f, pc.player_level - 1);
                PlayerPrefs.SetInt("Player_Level_Knight",1);
                PlayerPrefs.SetFloat("Player_Exp_Knight", 0);
                break;
            case 1: // 궁수
                pc.player_level = 1;
                pc.player_exp = 0;
                pc.next_exp = pc.next_exp * Mathf.Pow(1.1f, pc.player_level - 1);
                PlayerPrefs.SetInt("Player_Level_Archer", 1);
                PlayerPrefs.SetFloat("Player_Exp_Archer",0);
                break;
            case 2: // 마법사
                pc.player_level = 1;
                pc.player_exp = 0;
                pc.next_exp = pc.next_exp * Mathf.Pow(1.1f, pc.player_level - 1);
                PlayerPrefs.SetInt("Player_Level_Wizard", 1);
                PlayerPrefs.SetFloat("Player_Exp_Wizard", 0);
                break;
        }
        pc.update_status();
    }
}
