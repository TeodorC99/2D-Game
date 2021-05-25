using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public GameObject Player;

    public void Press()
    {
        Player.GetComponent<Animator>().SetTrigger("IsAttacking");
        Player.GetComponent<PhotonView>().RPC("Attack", PhotonTargets.AllBuffered);
    }
}
