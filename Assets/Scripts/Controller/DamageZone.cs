using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private BoxCollider col;
    [SerializeField] private GameObject player;
    void Start()
    {
        
    }
    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyAiTutorial>().TakeDamage(0, player.transform);
            Debug.Log(other);
        }
    }
}
