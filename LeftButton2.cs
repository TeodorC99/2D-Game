using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftButton2 : MonoBehaviour
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
            Player.GetComponent<PlayerMove2>().flip = false;
            Player.GetComponent<PhotonView>().RPC("ButtonMoveLeft", PhotonTargets.AllBuffered);
            Player.GetComponent<Animator>().SetFloat("Speed", 1);
        }
    }
}
