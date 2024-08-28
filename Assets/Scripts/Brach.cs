using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brach : MonoBehaviour
{
    [SerializeField] Collider col;
    [SerializeField] private float branchTimerProgres;
    [SerializeField] public bool isUsing;
    [SerializeField] float timeToUse;
    [SerializeField] bool Finish;
    [SerializeField] Animator animator;
    [SerializeField] bool isPlayer;
    [SerializeField] GameObject player;

    void Start()
    {

    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().isPressB)
        {
            Use();
        }
        else if (Finish == false)
        {
            branchTimerProgres = 0;
        }
        isBranch();
    }
    void Use()
    {
        branchTimerProgres += Time.deltaTime;
    }
    void isBranch()
    {
        if (branchTimerProgres >= timeToUse)
        {
            Finish = true;
            animator.SetBool("Ready", true);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("cnj.");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("cnj.");
            Use();
            if (collision.gameObject.GetComponent<PlayerController>().isPressB)
            {
                Use();
                isBranch();

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            isPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayer = false;
        }
    }
}


