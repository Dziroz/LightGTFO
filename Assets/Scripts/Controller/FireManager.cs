using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] public int firePower;
    [SerializeField] public GameObject lamp;
    [SerializeField] public float TimeToLoseFire;
    [SerializeField] public GameObject lampLightPoint;
    private float timer;
    void Start()
    {
        
    }

    private void Update()
    {
        Find();
        LightPower();
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
    }
    public void AddPower()
    {
        if (firePower < 5)
        {
            firePower++;
            ResetTime();
        }
        if(firePower == 5)
        {
            ResetTime();
        }

    }
    public void LightPower()
    {
        lampLightPoint.GetComponent<Light>().intensity = firePower;
    }


}
