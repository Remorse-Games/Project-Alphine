using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player and Player's Speed declaration.
    public GameObject mainPlayer;
    public Transform movePoint;
    public float moveSpeed;
    void Start()
    {
        //Sets first position of mainPlayer and movePoint
        mainPlayer.transform.position = Vector3.zero;

        //Sets parent of movePoint to null
        movePoint.SetParent(null,false);
    }

    void Update()
    {
        Debug.Log(Input.GetAxisRaw("Horizontal"));
        //Player's movement.
        mainPlayer.transform.position = Vector3.MoveTowards(mainPlayer.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(mainPlayer.transform.position, movePoint.transform.position) <= 0.5f)
        {
            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
        }
    }
}
