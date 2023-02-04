using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] Target target;
    [SerializeField] float force_Multiplier;

    public enum Target
    {
        Opponent, Player
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Opponent")
        {
            other.gameObject.GetComponent<Opponent_AI>().Get_Hit(force_Multiplier, transform.forward + new Vector3(0f, 0.5f, 0f), transform.parent.tag);
            Stop_Move();
        }
        else if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player_Manager>().Get_Hit(force_Multiplier, transform.forward + new Vector3(0f, 0.5f, 0f));
            Stop_Move();
        }
    }
    void Stop_Move()
    {
        transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
