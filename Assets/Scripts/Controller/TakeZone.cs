using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeZone : MonoBehaviour
{
    private SphereCollider col;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerScript;

    void Start()
    {
        col = GetComponent<SphereCollider>();
        playerScript = player.GetComponent<PlayerController>();
       
    }

    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Lamp" && PlayerController.lampInPlayer == false)
        {
            playerScript.canTake = true;
            playerScript.lampInGame = other.gameObject;
        }
        if(other.gameObject.tag == "Fire")
        {

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Lamp" && PlayerController.lampInPlayer == false)
        {
            playerScript.canTake = false;
            playerScript.lampInGame = null;
        }
    }

}
