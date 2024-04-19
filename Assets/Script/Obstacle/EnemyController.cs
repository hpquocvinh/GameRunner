using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private Transform _player;

    public int speed = 5;
    public int distance = 100;
    // Use this for initialization
    void Start()
    {
        _player = GameObject.Find("PlayerController").transform;
        int r = Random.Range(1, 3);
        if (r == 1)
        {
            transform.position = new Vector3(-4.5f, transform.position.y, transform.position.z);
        }
        else if (r == 2)
        {
            transform.position = new Vector3(-0.3f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(-3.7f, transform.position.y, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (!GameObject.Find("PlayerController").GetComponent<PlayerController>().death)
        {
            if (Vector3.Distance(transform.position, _player.position) <= distance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * UnityEngine.Time.deltaTime);
            }
        }
    }

}