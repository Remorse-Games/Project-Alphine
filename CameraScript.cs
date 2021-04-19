using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float distance;
    public Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x - offset.x, distance, player.position.z - offset.y);
    }
}
