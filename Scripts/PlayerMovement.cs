using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player and Player's Speed declaration.
    public GameObject mainPlayer;
    public float moveSpeed;
    void Start()
    {
        //Resets the player location to (0,0,0).
        mainPlayer.transform.position = Vector3.zero;
    }

    void Update()
    {
        //Player's movement.
        mainPlayer.transform.position += new Vector3(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,0 , 0);
        mainPlayer.transform.position += new Vector3(0, 0, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
    }
}
