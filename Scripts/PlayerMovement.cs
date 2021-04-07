using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Customizable Values")]
    public float movementSpeed = .1f;

    private void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0.00f, Input.GetAxis("Vertical")) * movementSpeed;
    }
}