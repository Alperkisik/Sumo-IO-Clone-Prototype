using System;
using UnityEngine;

public class Game_Event_Manager : MonoBehaviour
{
    public static Game_Event_Manager Instance { get; private set; }

    public event EventHandler OnLevelStart;
    public event EventHandler OnLevelFinish;
    public event EventHandler OnLevelStartTap;
    public event EventHandler OnLevelLoaded;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    public event EventHandler OnPlayerLose;
    public event EventHandler OnPlayerWon;
    public event EventHandler OnTimeIsUp;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void Event_OnLevelStart()
    {
        OnLevelStart?.Invoke(this, EventArgs.Empty);
    }
    public void Event_OnLevelFinish()
    {
        OnLevelFinish?.Invoke(this, EventArgs.Empty);
    }
    public void Event_OnLevelTap()
    {
        OnLevelStartTap?.Invoke(this, EventArgs.Empty);
    }
    public void Event_OnOnLevelLoaded()
    {
        OnLevelLoaded?.Invoke(this, EventArgs.Empty);
    }

    public void Event_OnGamePaused()
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void Event_OnGameResumed()
    {
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }
    public void Event_OnPlayerLose()
    {
        OnPlayerLose?.Invoke(this, EventArgs.Empty);
    }

    public void Event_OnPlayerWon()
    {
        OnPlayerWon?.Invoke(this, EventArgs.Empty);
    }
    public void Event_OnTimeIsUp()
    {
        OnTimeIsUp?.Invoke(this, EventArgs.Empty);
    }
}
