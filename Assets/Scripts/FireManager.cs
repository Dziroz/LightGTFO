using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FireManager : MonoBehaviour
{
    [SerializeField] GameObject[] aliveLamp = new GameObject[10];
    [SerializeField] GameObject[] aLiveLampInWorld = new GameObject[10];
    [SerializeField] int lampsALiveCount;
    [SerializeField] int maxLamplsALiveCount;
    [SerializeField] GameManager gameManagerScript;
    [SerializeField] private float ImmortalTime;
    [SerializeField] private float immortalTImer;
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
    [SerializeField] private GameObject lampGameObjectEmmission;
    void Start()
    {
        
    }
    void SpawnFire()
    {
        timerSpawn += Time.deltaTime;
        if (timerSpawn >= timerToSpawnFire)
        {
            Vector3 startPosition = new Vector3(Camera.main.transform.position.x, 1, Camera.main.transform.position.z);
            startPosition = new Vector3(lamp.transform.position.x, 1, lamp.transform.position.z);
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
        DestroyLamp();
        if (GameManager.game)
        {
            SpawnFire();
            Find();
            LightPower();
            Timer();
            RemovePower();
            immortalTImer += Time.deltaTime;
            
        }
    }
    private void Find()
    {
        lamp = GameObject.FindGameObjectWithTag("Lamp");
        lampGameObjectEmmission = GameObject.Find("SVET");
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

            
            if(firePower <=0)
            {
                lampGameObjectEmmission.SetActive(false);
                Debug.Log("Фонарь потушен");
                //SceneManager.LoadScene(2);
                
            }
        }
    }
    public void AttackLight()
    {
        if (firePower >= 0 && immortalTImer >= ImmortalTime) 
        {
            immortalTImer = 0;
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
    public void DestroyLamp()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("Lamp");

        for (int i = 0; i < lamps.Length; i++)
        {
            if (lamps[i].activeSelf == true)
            {
                lampsALiveCount++;               
            }
            if (lamps[i].activeSelf == true && lamps[i].transform.parent == false)
            {
                aLiveLampInWorld[i] = lamps[i];
            }

        }
        for (int i = 0; i < aLiveLampInWorld.Length; i++)
        {

        }
        maxLamplsALiveCount = lampsALiveCount;
        lampsALiveCount = 0;
       // Debug.Log(length();
        if (maxLamplsALiveCount>=2)
        {
            for (int i = 1; i < aLiveLampInWorld.Length; i++)
            {
                Destroy(aLiveLampInWorld[i]);
            }
            /*
            for (int i = 1; i < lamps.Length; i++)
            {
                if (lamps[i].transform.parent == null)
                {

                }
                else
                {
                    Destroy(aLiveLampInWorld[i]);
                }
            }
            */
        }
    }


}
