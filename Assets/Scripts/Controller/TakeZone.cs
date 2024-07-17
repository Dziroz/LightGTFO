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

        if(other.gameObject.tag == "Lamp" && playerScript.isLamped == false && playerScript.isRightTrigger == true)
        {
            Debug.Log("2");
            Destroy(other.gameObject);
            playerScript.TakeLamp();
            
        }
    }

}
