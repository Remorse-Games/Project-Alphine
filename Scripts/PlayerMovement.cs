using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Customized Value")]
    public float movementSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * movementSpeed, 0, 0));
        transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical") * movementSpeed));

        transform.GetChild(0).GetComponent<Animator>().SetFloat("horizontal", Input.GetAxis("Horizontal"));
        transform.GetChild(0).GetComponent<Animator>().SetFloat("vertical", Input.GetAxis("Vertical"));
    }
}