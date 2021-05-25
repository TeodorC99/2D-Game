using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Photon.MonoBehaviour
{
    public GameObject PlayerPrefab1;
    public GameObject PlayerPrefab2;
    public GameObject GameCanvas;
    bool nextPlayer;
    public GameObject myCanvas;
    public Text PingText;

    private bool Off = false;
    public GameObject disconectUI;

    public GameObject PlayerFeed;
    public GameObject FeedGrid;

    public Vector2 Player1 = new Vector2(-12, 2);
    public Vector2 Player2 = new Vector2(10, 2);

    bool playerSpawn = false;

    void Start()
    {
        GameObject canvas = Instantiate(myCanvas) as GameObject;
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Awake()
    {
        GameCanvas.SetActive(true);

        //GameObject canvas = Instantiate(myCanvas) as GameObject;
        //canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        CheckInput();
        PingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    private void CheckInput()
    {
        if (Off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconectUI.SetActive(false);
            Off = false;
        }
        else if (!Off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconectUI.SetActive(true);
            Off = true;
        }
    }

    public void SpawnChar1()
    {
        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab1.name, new Vector2(-1, 6), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        obj.GetComponent<PlayerMovement>().disUI = disconectUI;
    }

    public void SpawnChar2()
    {
        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab2.name, new Vector2(-1, 6), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        obj.GetComponent<PlayerMove2>().disUI = disconectUI;
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(FeedGrid.transform, false);
        obj.GetComponent<Text>().text = player.name + " joined the game.";
        obj.GetComponent<Text>().color = Color.blue;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(FeedGrid.transform, false);
        obj.GetComponent<Text>().text = player.name + " left the game.";
        obj.GetComponent<Text>().color = Color.red;
    }
}
