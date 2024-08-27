using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField] private int maxEnemyOnScene;
    [SerializeField]private int EnemyOnScene;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField]  int EnemyCount;

    [SerializeField] float SpawnColdown;
    [SerializeField] float spawningDistance;

    [SerializeField] int Snake;
    [SerializeField] int Tank;
    
    float timer;
    void Start()
    {       
    }

    void Update()
    {
        EnemyOnScene = GameObject.FindGameObjectsWithTag("Enemy").Length;
        timer += Time.deltaTime;
        if(timer >= SpawnColdown)
        {
            if(EnemyOnScene < maxEnemyOnScene)
            {
                Spawn();
                timer = 0;
            }
        }
    }
    private void FixedUpdate()
    {
    }
    void Spawn()
    {
        if (EnemyCount == Snake)
        {
            var x = Instantiate(enemyPrefab[1], Position(), Quaternion.identity);
            x.GetComponent<EnemyAiTutorial>().enabled = true;
            EnemyCount++;          
        }
        if (EnemyCount == Tank)
        {
            var x = Instantiate(enemyPrefab[2], Position(), Quaternion.identity);
            x.GetComponent<EnemyAiTutorial>().enabled = true;
            EnemyCount++;
        }
        else
        {
            var x = Instantiate(enemyPrefab[0], Position(), Quaternion.identity);
            x.GetComponent<EnemyAiTutorial>().enabled = true;
            EnemyCount++;
        }
        if(EnemyCount == Tank)
        {
            EnemyCount = 0;
        }
    }
    Vector3 Position()
    {
        Vector3 startPosition = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        Vector3 v = new Vector3(x, 0.0f, z).normalized;
        Vector3 pos = startPosition + v * spawningDistance;
        return pos;

    }

}
