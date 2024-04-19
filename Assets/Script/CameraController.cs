using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Vector3 target = Vector3.zero;

    void Update()
    {
        target = new Vector3(player.position.x, player.position.y + 2.25f, player.position.z - 4);
        transform.position = Vector3.Lerp(transform.position, target, 1f);
    }
}
