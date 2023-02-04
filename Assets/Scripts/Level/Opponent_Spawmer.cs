using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent_Spawmer : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] float spawn_Radius;
    [SerializeField] GameObject opponent_Prefab;
    [SerializeField] GameObject parent_Object;
    [SerializeField] List<Material> colors;
    void Start()
    {
        Spawn_Opponents();
        gameObject.SetActive(false);
    }

    void Spawn_Opponents()
    {
        for (int i = 0; i < amount; i++)
        {
            float xPos = Random.Range(-1f * spawn_Radius, spawn_Radius), zPos = Random.Range(-1f * spawn_Radius, spawn_Radius);

            GameObject player = Instantiate(opponent_Prefab, transform.position, Quaternion.identity, parent_Object.transform);
            player.GetComponent<MeshRenderer>().material = colors[Random.Range(0, colors.Count)];
            player.transform.name = "Opponent " + (i + 1).ToString();
            player.transform.position = new Vector3(xPos, 1f, zPos);

            transform.parent.GetComponent<Level_Manager>().Increase_Player_Count();
        }
    }
}
