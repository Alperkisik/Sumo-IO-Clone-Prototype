using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public enum Input_Type
    {
        Touch,keyboard
    }
    [SerializeField] private float stun_duration;
    [SerializeField] private float movement_Speed;
    [SerializeField] private float rotation_Speed;
    [SerializeField] Input_Type input_type;
    Rigidbody rb;
    Touch touch;
    bool able_To_Move;
    bool get_Hit;
    bool game_unavailable;
    float stun_cooldown;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        able_To_Move = false;
        get_Hit = false;
        game_unavailable = false;
        stun_cooldown = stun_duration;
        Subscribe();
    }

    void FixedUpdate()
    {
        if (game_unavailable) return;

        if (get_Hit)
        {
            stun_cooldown -= Time.fixedDeltaTime;
            if (stun_cooldown <= 0f)
            {
                able_To_Move = true;
                stun_cooldown = stun_duration;
                get_Hit = false;
            }
        }

        if (able_To_Move) 
        {
            if (input_type == Input_Type.Touch) Touch_Movement();
            else Keyboard_Movement();
        } 
    }
    void Subscribe()
    {
        Game_Event_Manager.Instance.OnLevelStartTap += Event_OnLevelStartTap;
        Game_Event_Manager.Instance.OnLevelFinish += Event_OnLevelFinish;
        Game_Event_Manager.Instance.OnGamePaused += Event_OnGamePaused;
        Game_Event_Manager.Instance.OnGameResumed += Event_OnGameResumed;
    }

    private void Event_OnGameResumed(object sender, System.EventArgs e)
    {
        game_unavailable = true;
        rb.velocity = Vector3.zero;
    }
    private void Event_OnGamePaused(object sender, System.EventArgs e)
    {
        game_unavailable = false;
        rb.velocity = Vector3.zero;
    }

    private void Event_OnLevelFinish(object sender, System.EventArgs e)
    {
        able_To_Move = false;
    }
    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        able_To_Move = true;
    }

    void Touch_Movement()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchDir = new Vector3(transform.position.x + touch.deltaPosition.x, transform.position.y, transform.position.z + touch.deltaPosition.y);
                transform.LookAt(touchDir);

                rb.velocity = touchDir.normalized * movement_Speed * Time.deltaTime;
            }
        }
        else rb.velocity = Vector3.zero;
    }

    void Keyboard_Movement()
    {
        if (Input.GetKey(KeyCode.W)) rb.velocity = transform.forward * movement_Speed;
        else if (Input.GetKey(KeyCode.S)) rb.velocity = -transform.forward * movement_Speed;
        else rb.velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) transform.Rotate(0f, -1f * rotation_Speed, 0f);
        else if (Input.GetKey(KeyCode.D)) transform.Rotate(0f, 1f * rotation_Speed, 0f);
    }

    public void Get_Hit(float force_value, Vector3 direction)
    {
        able_To_Move = false;
        get_Hit = true;
        Vector3 force_vector = direction * force_value;
        rb.AddForce(force_vector, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Death Zone")
        {
            Camera.main.transform.parent = transform.parent.parent;
            Game_Event_Manager.Instance.Event_OnPlayerLose();
            Game_Event_Manager.Instance.Event_OnLevelFinish();
            gameObject.SetActive(false);
        }
    }
}
