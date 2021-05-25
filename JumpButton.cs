using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public GameObject Player;

    public void Pressed()
    {
        Player.GetComponent<PlayerMovement>().jump = true;
        Player.GetComponent<Animator>().SetTrigger("Jump");
        Player.GetComponent<Animator>().SetBool("IsJumping", true);
    }
}
