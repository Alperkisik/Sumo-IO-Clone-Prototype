using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;
    Rigidbody rb;
    Vector3 movement;

    bool able_To_Move;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        able_To_Move = false;
        Subscribe();
    }

    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        if (!able_To_Move) return;

        Move_Player(movement);
    }

    void Move_Player(Vector3 direction)
    {
        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }
    void Subscribe()
    {
        Game_Event_Manager.Instance.OnLevelStartTap += Event_OnLevelStartTap;
        Game_Event_Manager.Instance.OnLevelFinish += Event_OnLevelFinish;
    }

    private void Event_OnLevelFinish(object sender, System.EventArgs e)
    {
        able_To_Move = false;
    }

    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        able_To_Move = true;
    }
}
