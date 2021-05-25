using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpButton2 : MonoBehaviour
{
    public GameObject Player;

    public void Pressed()
    {
        Player.GetComponent<PlayerMove2>().jump = true;
        Player.GetComponent<Animator>().SetTrigger("Jump");
        Player.GetComponent<Animator>().SetBool("IsJumping", true);
    }
}
