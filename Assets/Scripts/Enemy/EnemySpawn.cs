using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private int maxEnemyOnScene;
    [SerializeField] private int EnemyOnScene;
    [SerializeField] GameObject[] enemyPrefab;
    //[SerializeField] private int 
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Spawn()
    {
        Instantiate(enemyPrefab[0]);
        EnemyOnScene++;
    }
    
}
