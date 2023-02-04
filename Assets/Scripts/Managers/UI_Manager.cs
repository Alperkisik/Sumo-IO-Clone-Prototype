using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject game_UI_Panel;
    [SerializeField] GameObject level_Starting_Panel;
    [SerializeField] TextMeshProUGUI level_Textmesh;
    [SerializeField] TextMeshProUGUI score_Textmesh;
    [SerializeField] GameObject level_Finishing_Panel;
    [SerializeField] TextMeshProUGUI pause_Button_TextMesh;

    bool level_finish = false;
    bool pause = false;
    void Start()
    {
        pause = false;
        level_Starting_Panel.SetActive(true);
        level_Finishing_Panel.SetActive(false);
        game_UI_Panel.SetActive(false);
        Subscribe();
    }

    private void Subscribe()
    {
        Game_Event_Manager.Instance.OnLevelLoaded += Event_OnLevelLoaded;
        Game_Event_Manager.Instance.OnLevelStartTap += Event_OnLevelStartTap;
        Game_Event_Manager.Instance.OnLevelFinish += Event_OnLevelFinish;
        Game_Event_Manager.Instance.OnPlayerLose += Event_OnPlayerLose;
        Game_Event_Manager.Instance.OnPlayerWon += Event_OnPlayerWon;
        Game_Event_Manager.Instance.OnTimeIsUp += Event_OnTimeIsUp;
    }

    private void Event_OnTimeIsUp(object sender, EventArgs e)
    {
        level_Finishing_Panel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Time is Up !!!";
    }

    private void Event_OnPlayerWon(object sender, EventArgs e)
    {
        level_Finishing_Panel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Level WON By Player !!!";
    }

    private void Event_OnPlayerLose(object sender, EventArgs e)
    {
        level_Finishing_Panel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Level WON By AI !!!";
    }

    private void Event_OnScoreChanged(object sender, System.EventArgs e)
    {
        score_Textmesh.text = "Score " + PlayerPrefs.GetInt("Score").ToString();
    }

    private void Event_OnLevelFinish(object sender, System.EventArgs e)
    {
        level_finish = true;
        level_Starting_Panel.SetActive(false);
        game_UI_Panel.SetActive(false);
        level_Finishing_Panel.SetActive(true);
    }

    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        game_UI_Panel.SetActive(true);
        level_Starting_Panel.SetActive(false);
        level_Finishing_Panel.SetActive(false);
    }

    private void Event_OnLevelLoaded(object sender, System.EventArgs e)
    {
        game_UI_Panel.SetActive(false);
        level_Finishing_Panel.SetActive(false);
        level_Starting_Panel.SetActive(true);
        level_Textmesh.text = "Level " + (Game_Manager.Instance.Get_Level_Index() + 1).ToString();
    }

    public void Next_Level()
    {
        if (!level_finish) return;
        Game_Manager.Instance.Next_Level();
    }
    public void Retry_Level()
    {
        if (!level_finish) return;

        Game_Manager.Instance.Retry_Level();
    }

    public void Button_Pause()
    {
        if (!pause)
        {
            pause_Button_TextMesh.text = "Resume"; pause = true;
            Game_Event_Manager.Instance.Event_OnGamePaused();
        }
        else
        {
            pause_Button_TextMesh.text = "Pause"; pause = false;
            Game_Event_Manager.Instance.Event_OnGameResumed();
        }
    }
}
