using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent_AI : MonoBehaviour
{
    [SerializeField] float movement_Speed;
    [SerializeField] float stunned_Duration;
    [SerializeField] float waiting_Duration_After_Hit;
    [SerializeField] private LayerMask player_Layer;
    [SerializeField] private float scan_Range = 10f;
    public enum State
    {
        idling, moving, stunned, dead, hit
    }

    State state;
    Rigidbody rb;
    GameObject target_Player;
    bool enable_AI;
    float stunned_Cooldown;
    float waiting_Cooldown;
    string last_hitter_tag;

    void Start()
    {
        Subscribe();
        stunned_Cooldown = stunned_Duration;
        waiting_Cooldown = waiting_Duration_After_Hit;
        rb = this.GetComponent<Rigidbody>();
        enable_AI = false;
        state = State.idling;
    }

    private void FixedUpdate()
    {
        if (!enable_AI) return;

        Choose_Behaviour();
    }

    void Choose_Behaviour()
    {
        switch (state)
        {
            case State.idling:
                Find_Closest_Player();
                break;
            case State.moving:
                if (target_Player.tag != "Player" && !Is_Target_Alive_Check()) state = State.idling;
                else Go_To_Closest_Player();
                break;
            case State.stunned:
                stunned_Cooldown -= Time.fixedDeltaTime;
                if (stunned_Cooldown <= 0f)
                {
                    state = State.idling;
                    stunned_Cooldown = stunned_Duration;
                    rb.velocity = Vector3.zero;
                }
                break;
            case State.hit:
                waiting_Cooldown -= Time.fixedDeltaTime;
                if (waiting_Cooldown <= 0f)
                {
                    state = State.idling;
                    waiting_Cooldown = waiting_Duration_After_Hit;
                }
                break;
            default:
                break;
        }
    }
    void Find_Closest_Player()
    {
        Collider[] player_Colliders = Physics.OverlapSphere(transform.position, scan_Range, player_Layer);

        if (player_Colliders.Length > 0)
        {
            List<GameObject> players = new List<GameObject>();
            foreach (Collider player_Collider in player_Colliders)
            {
                if (player_Collider.transform.name != this.gameObject.transform.name) players.Add(player_Collider.gameObject);
            }

            target_Player = Find_Nearest_Player(players);
            state = State.moving;
        }
        else
        {
            state = State.idling;
            rb.velocity = Vector3.zero;
        }
    }

    GameObject Find_Nearest_Player(List<GameObject> targets)
    {
        GameObject target;

        if (targets.Count == 1)
        {
            target = targets[0];
        }
        else
        {
            float distance;
            float tempDistance;

            distance = Vector3.Distance(transform.position, targets[0].transform.position);
            target = targets[0];

            foreach (GameObject cube in targets)
            {
                tempDistance = Vector3.Distance(transform.position, cube.transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    target = cube;
                }
            }
        }

        return target;
    }

    void Go_To_Closest_Player()
    {
        transform.LookAt(target_Player.transform);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        rb.velocity = transform.forward * movement_Speed;

        float targetDistance = Vector3.Distance(transform.position, target_Player.transform.position);

        if (targetDistance <= 0.2f)
        {
            rb.velocity = Vector3.zero;
        }
    }
    bool Is_Target_Alive_Check()
    {
        if (target_Player.GetComponent<Opponent_AI>().state == State.dead) return false;
        else return true;
    }
    public void Get_Hit(float force_value, Vector3 direction, string hitter_Tag)
    {
        if (state == State.idling) waiting_Cooldown = waiting_Duration_After_Hit;
        rb.velocity = Vector3.zero;
        last_hitter_tag = hitter_Tag;
        Vector3 force_vector = direction * force_value;
        rb.AddForce(force_vector, ForceMode.Impulse);
        state = State.stunned;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Death Zone")
        {
            state = State.dead;
            if (last_hitter_tag == "Player") PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1);
            transform.root.GetChild(0).GetComponent<Level_Manager>().Player_Died();
            gameObject.SetActive(false);
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
        enable_AI = true;
        rb.velocity = Vector3.zero;
    }

    private void Event_OnGamePaused(object sender, System.EventArgs e)
    {
        enable_AI = false;
        rb.velocity = Vector3.zero;
    }

    private void Event_OnLevelStartTap(object sender, System.EventArgs e)
    {
        enable_AI = true;
    }

    private void Event_OnLevelFinish(object sender, System.EventArgs e)
    {
        enable_AI = false;
        rb.velocity = Vector3.zero;
    }
}
