using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] GameObject level_UI;

    public int player_count = 0;

    private void Start()
    {
        player_count = 0;
        Subscribe();
    }
    void Subscribe()
    {
        Game_Event_Manager.Instance.OnLevelStartTap += Event_OnLevelStartTap;
        Game_Event_Manager.Instance.OnLevelFinish += Event_OnLevelFinish;
    }
    void DeSubscribe()
    {
        Game_Event_Manager.Instance.OnLevelStartTap -= Event_OnLevelStartTap;
        Game_Event_Manager.Instance.OnLevelFinish -= Event_OnLevelFinish;
    }
    private void Event_OnLevelFinish(object sender, System.EventArgs e)
    {
        level_UI.SetActive(false);
        DeSubscribe();
    }

    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        level_UI.SetActive(true);
    }

    public void Time_Is_Up()
    {
        Game_Event_Manager.Instance.Event_OnTimeIsUp();
        Game_Event_Manager.Instance.Event_OnLevelFinish();
    }

    public void Increase_Player_Count()
    {
        player_count++;
    }

    public void Increase_Score()
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1);
    }
    public int Get_Score()
    {
        return PlayerPrefs.GetInt("Score");
    }
    public void Player_Died()
    {
        player_count--;

        if (player_count <= 1)
        {
            Game_Event_Manager.Instance.Event_OnPlayerWon();
            Game_Event_Manager.Instance.Event_OnLevelFinish();
        }
    }
}
