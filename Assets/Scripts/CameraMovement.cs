using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameManager gameManagerScript;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject[] Players;
    [SerializeField] private float[] Zposition;
    [SerializeField] private float cameraZposition;
    [SerializeField] private float distance;
    [SerializeField] FireManager fireManagerScript;
    [SerializeField] float maxZPosition;
    [SerializeField] float minZposition;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (gameManagerScript.game)
        {
            Players = GameObject.FindGameObjectsWithTag("Player");
            SetZ();
            if (Players.Length != 0)
            {
                FindZPosition();
            }
        }
    }
    void FindZPosition()
    {
        cameraZposition = ((Zposition.Max() - Zposition.Min())/2) + Zposition.Min();
        if(cameraZposition <= maxZPosition && cameraZposition >= minZposition)
        {
            cameraObject.transform.position = new Vector3(0, cameraObject.transform.position.y, cameraZposition - distance);
        }
        else
        {
            //cameraObject.transform.position = new Vector3(0, cameraObject.transform.position.y, cameraZposition - distance);
        }
        //cameraObject.transform.position = new Vector3(0, cameraObject.transform.position.y, cameraZposition-distance);
    }

    void SetZ()
    {
        Zposition = new float[Players.Length];

        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i].GetComponent<PlayerController>().alive)
            {
                Zposition[i] = Players[i].transform.position.z;
            }
            else
            {
                Zposition[i] = fireManagerScript.lamp.transform.position.z;
            }
        }
    }
}
