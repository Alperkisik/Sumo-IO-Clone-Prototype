using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer_TextMesh;
    [SerializeField] float duration;
    float cooldown;

    bool timer_Available;
    void Start()
    {
        timer_Available = true;
        cooldown = duration;
        timer_TextMesh.text = cooldown.ToString();
        Subscribe();
    }

    private void FixedUpdate()
    {
        if (!timer_Available) return;

        cooldown -= Time.fixedDeltaTime;
        timer_TextMesh.text = ((int)cooldown).ToString();

        if (cooldown <= 0f)
        {
            timer_Available = false;
            transform.root.GetChild(0).GetComponent<Level_Manager>().Time_Is_Up();
        }
    }

    void Subscribe()
    {
        Game_Event_Manager.Instance.OnGamePaused += Event_OnGamePaused;
        Game_Event_Manager.Instance.OnGameResumed += Event_OnGameResumed;
    }

    private void Event_OnGameResumed(object sender, System.EventArgs e)
    {
        timer_Available = true;
    }

    private void Event_OnGamePaused(object sender, System.EventArgs e)
    {
        timer_Available = false;
    }

    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        timer_Available = true;
    }
}
