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
        if (other.gameObject.GetComponent<EnemyAiTutorial>())
        {
            other.gameObject.GetComponent<EnemyAiTutorial>().TakeDamage(1, player.transform, player);
            Debug.Log("Попал");
        }
    }
}
