using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checking_Tap : MonoBehaviour
{
    bool check_tap = false;
    void Start()
    {
        check_tap = true;
        Subscribe();
    }
    void Update()
    {
        if (check_tap) Check_Tap();
    }
    private void Check_Tap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            check_tap = false;
            Game_Event_Manager.Instance.Event_OnLevelTap();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                check_tap = false;
                Game_Event_Manager.Instance.Event_OnLevelTap();
            }
        }
    }

    private void Subscribe()
    {
        Game_Event_Manager.Instance.OnLevelLoaded += Event_OnLevelLoaded;
        Game_Event_Manager.Instance.OnLevelStartTap += Event_OnLevelStartTap;
    }

    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        check_tap = false;
    }

    private void Event_OnLevelLoaded(object sender, System.EventArgs e)
    {
        check_tap = true;
    }
}
