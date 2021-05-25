using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicButton : MonoBehaviour
{
    public GameObject Player;

    public void Pressed()
    {
        Block();
    }

    public void NotPressed()
    {
        StopBlock();
    }

    void Block()
    {
        Player.GetComponent<PhotonView>().RPC("Block", PhotonTargets.AllBuffered);
    }

    void StopBlock()
    {
        Player.GetComponent<PhotonView>().RPC("StopBlock", PhotonTargets.AllBuffered);
    }

    IEnumerator Time() 
    {

        yield return new WaitForSeconds(0.1f);
    }
}
