using System;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    [SerializeField] List<GameObject> levels;
    int current_level_index;
    GameObject current_level_prefab;
    bool level_finish = false;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);

        Subscribe();

        current_level_index = PlayerPrefs.GetInt("Last_Level");
        Load_Level();
    }

    private void Subscribe()
    {
        Game_Event_Manager.Instance.OnLevelFinish += Event_OnLevelFinish;
        Game_Event_Manager.Instance.OnLevelStartTap += Event_OnLevelStartTap;
    }

    private void Event_OnLevelStartTap(object sender, EventArgs e)
    {
        Game_Event_Manager.Instance.Event_OnLevelStart();
    }

    private void Event_OnLevelFinish(object sender, EventArgs e)
    {
        level_finish = true;
    }

    private void Load_Level()
    {
        current_level_prefab = Instantiate(levels[current_level_index], Vector3.zero, Quaternion.identity, transform);
        Game_Event_Manager.Instance.Event_OnOnLevelLoaded();
    }

    private void DestroyCurrentLevel()
    {
        Destroy(current_level_prefab);
    }

    public int Get_Level_Index()
    {
        return current_level_index;
    }
    public void Next_Level()
    {
        if (!level_finish) return;

        current_level_index++;
        if (current_level_index >= levels.Count) current_level_index = 0;

        DestroyCurrentLevel();
        Load_Level();
    }

    public void Retry_Level()
    {
        if (!level_finish) return;

        DestroyCurrentLevel();
        Load_Level();
    }
}
