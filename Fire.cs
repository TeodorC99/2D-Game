using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Photon.MonoBehaviour
{
    public bool MoveDirection = false; //false (right), true(left)
    public float MoveSpeed;
    public float DestroyTime;

    public PhotonView photonView;

    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void ChangeDir_left()
    {
        MoveDirection = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void Update() 
    {
        if (!MoveDirection)
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (!photonView.isMine)
            return;

        PhotonView target = collison.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.isMine || target.isSceneView))
        {
            if (target.tag == "Player")
            {
                if (target.GetComponent<PlayerMove2>() != null)
                {
                    target.RPC("TakeDamage", PhotonTargets.AllBuffered, 15);
                }
                if (target.GetComponent<PlayerMovement>() != null)
                {
                    target.RPC("TakeDamage", PhotonTargets.AllBuffered, 15);
                }
                else
                {
                    Debug.Log("NE");
                }
            }

            this.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.AllBuffered);
        }
    }
}
