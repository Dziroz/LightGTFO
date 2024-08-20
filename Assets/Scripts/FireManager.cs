using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] float timerToSpawnFire;
    [SerializeField] int spawningDistance;
    private float timerSpawn;
    [SerializeField] public int firePower;
    [SerializeField] public int maxFirePower;
    [SerializeField] public int[] firePowerArray;

    [SerializeField] public GameObject lamp;
    [SerializeField] public float TimeToLoseFire;
    [SerializeField] public GameObject lampLightPoint;
    [SerializeField] private float timer;
    void Start()
    {
        
    }
    void SpawnFire()
    {
        timerSpawn += Time.deltaTime;
        if (timerSpawn >= timerToSpawnFire)
        {
            Vector3 startPosition = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
            float x = Random.Range(-1.0f, 1.0f);
            float z = Random.Range(-1.0f, 1.0f);
            Vector3 v = new Vector3(x, 0.0f, z).normalized;
            Vector3 pos = startPosition + v * spawningDistance;
            Instantiate(firePrefab, pos, Quaternion.identity);
            timerSpawn = 0;
        }
    }
    private void Update()
    {
        SpawnFire();
        Find();
        LightPower();
        Timer();
        RemovePower();
    }
    private void Find()
    {
        lamp = GameObject.FindGameObjectWithTag("Lamp");
        lampLightPoint = lamp.transform.GetChild(0).gameObject;
    }
    private void Timer()
    {
        timer += Time.deltaTime;
    }
    public void ResetTime()
    {
        timer = 0;
        //Debug.Log(firePower);
    }
    public void AddPower()
    {
        if (firePower < 4)
        {
            firePower++;
            ResetTime();
        }
        if(firePower == 4)
        {
            ResetTime();
        }

    }
    public void RemovePower()
    {
        if(timer >= TimeToLoseFire)
        {
            if(firePower >=0)
            {
                firePower--;
                ResetTime();
            }

            
            if(firePower == -1)
            {
                Debug.Log("Фонарь потушен");
            }
        }
    }
    public void AttackLight()
    {
        if (firePower >= 0)
        {
            firePower--;
            ResetTime();
        }
    }
    public void LightPower()
    {
        if(firePower <=-1)
        {
            lampLightPoint.GetComponent<Light>().range = 0;
        }
        else
        {
            lampLightPoint.GetComponent<Light>().range = firePowerArray[firePower];
        }
        
    }


}
