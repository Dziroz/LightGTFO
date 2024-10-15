using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource audio;
    static public bool game;
    static public bool gameHelth;
    public GameObject[] players;
    public bool[] PlayerStatus;
    public int liveCounter;
    public FireManager fireManagerScript;

    public GameObject Managers;
    public float songTimer;

    void Start()
    {
        var slider = GameObject.Find("Slider");
        slider.transform.localScale = new Vector3(0, 0, 0);
        FindPlayer();
        //Starting();
    }

    void Update()
    {
        if (game)
        {
            songTimer -= Time.deltaTime;
            audio.volume = songTimer/10;
        }
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
                    SceneManager.LoadScene(2);
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
            game = false;
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
    public void Starting()
    {
        TeleportPlayer();
        game = true;
    }
}
