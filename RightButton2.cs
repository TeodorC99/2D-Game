using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButton2 : MonoBehaviour
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
            Player.GetComponent<PlayerMove2>().flip = true;
            Player.GetComponent<PhotonView>().RPC("ButtonMoveRight", PhotonTargets.AllBuffered);
            Player.GetComponent<Animator>().SetFloat("Speed", 1);
        }
    }
}
