using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool game;
    public GameObject[] players;
    public bool[] PlayerStatus;
    public int liveCounter;
    public FireManager fireManagerScript;

    public GameObject Managers;

    void Start()
    {
        FindPlayer();
        //Starting();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            game = true;
            TeleportPlayer();
        }
        FindPlayer();
        FindPlayerLiveStatus();
    }
    void FindPlayerLiveStatus()
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerStatus[i] = players[i].GetComponent<PlayerController>().alive;
            players[i].GetComponent<PlayerController>().enabled = true;
            if(PlayerStatus[i] == false)
            {
                liveCounter++;
            }
            if(liveCounter == players.Length)
            {
                End();
            }


        }
        liveCounter = 0;
    }
    void FindPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        PlayerStatus = new bool[players.Length];
    }
    void End()
    {
        if(fireManagerScript.firePower <= 0)
        {
            Debug.Log("End");
        }
        Debug.Log("End");
    }
    void TeleportPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = new Vector3(3, 1.58f, -9);
        }
    }
    void Starting()
    {
        TeleportPlayer();
        Managers.SetActive(true);
    }
}
