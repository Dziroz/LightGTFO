using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool game;
    static public bool gameHelth;
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
        gameHelth = game;
        if (Input.GetKey(KeyCode.G))
        {
            game = true;
            TeleportPlayer();
        }
        //End();
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
                if (fireManagerScript.firePower <= 0)
                {
                    End();
                    //SceneManager.LoadScene(2);
                }
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
    }
    void TeleportPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = new Vector3(fireManagerScript.lamp.transform.position.x, fireManagerScript.lamp.transform.position.y, fireManagerScript.lamp.transform.position.z)
                ;
        }
    }
    void Starting()
    {
        TeleportPlayer();
        Managers.SetActive(true);
    }
}
