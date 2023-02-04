using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spawmer : MonoBehaviour
{
    [SerializeField] GameObject player_Prefab;
    [SerializeField] GameObject parent_Object;
    void Start()
    {
        GameObject player = Instantiate(player_Prefab, transform.position + Vector3.up, Quaternion.identity);
        player.transform.SetParent(parent_Object.transform);
        transform.root.GetChild(0).GetComponent<Level_Manager>().Increase_Player_Count();
        gameObject.SetActive(false);
    }
}
