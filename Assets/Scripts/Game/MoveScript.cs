using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Animator anim;
    bool lastMvmtPositiveX, lastMvmtPositiveY;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x == 1) // Set True If Last Walk To X Positive Axis
            lastMvmtPositiveX = true;
        else
            lastMvmtPositiveX = false;

        if (y == 1) // Set True If Last Walk To Y Positive Axis
            lastMvmtPositiveY = true;
        else
            lastMvmtPositiveY = false;


        if (x <= 0.2 && x >= -0.2 && y <= 0.2 && y >= -0.2) // If Input In Idle Position Range
        {
            if (lastMvmtPositiveX)
                anim.SetFloat("Horizontal", 0.2f);
            else if (lastMvmtPositiveY)
                    anim.SetFloat("Vertical", 0.2f);
            
        }
        else // If Input Outside the Idle Position Range
        {
            anim.SetFloat("Horizontal", x);
            anim.SetFloat("Vertical", y);
        }

        
    }
}
