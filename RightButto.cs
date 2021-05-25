using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButto : MonoBehaviour
{
    public GameObject Player;
    bool pressed = false;


    public void Pressed() 
    {
        pressed = true;
    }

    public void NotPressed() 
    {
        pressed = false;
        Player.GetComponent<Animator>().SetFloat("Speed", 0);
    }

    void FixedUpdate() 
    {
        if (pressed) 
        {
            Player.GetComponent<PlayerMovement>().flip = true;
            Player.GetComponent<CharacterController2D>().Move(2,false, Player.GetComponent<PlayerMovement>().jump);
            Player.GetComponent<Animator>().SetFloat("Speed", 1);

        }
    }

}
