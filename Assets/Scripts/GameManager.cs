using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool game;
    public GameObject[] players;
    public bool[] PlayerStatus;
    public int liveCounter;
    void Start()
    {
        
    }

    void Update()
    {
        FindPlayer();
        FindPlayerLiveStatus();
    }
    void FindPlayerLiveStatus()
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerStatus[i] = players[i].GetComponent<PlayerController>().alive;
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
        Debug.Log("End");
    }
}
