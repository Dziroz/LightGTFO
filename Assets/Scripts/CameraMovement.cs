using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject[] Players;
    [SerializeField] private float[] Zposition;
    [SerializeField] private float cameraZposition;
    [SerializeField] FireManager fireManagerScript;
    
    void Start()
    {
        
    }

    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        SetZ();
        FindZPosition();
    }
    void FindZPosition()
    {
        cameraZposition = ((Zposition.Max() - Zposition.Min())/2) + Zposition.Min();
        cameraObject.transform.position = new Vector3(0, cameraObject.transform.position.y, cameraZposition);
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
