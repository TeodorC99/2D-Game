using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicButton2 : MonoBehaviour
{
    public GameObject Player;

    public void pressed() 
    {
        Player.GetComponent<PlayerMove2>().Fire();
    }
}
