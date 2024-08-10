using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
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

    private void Update()
    {
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
        Debug.Log(firePower);
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
