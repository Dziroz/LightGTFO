using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lamp : MonoBehaviour
{
    private Collider col;
    [SerializeField] FireManager FireManagerScript;
    void Start()
    {
        col = GetComponent<Collider>();
        FireManagerScript = GameObject.Find("FireManager").GetComponent<FireManager>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "End")
        {
            SceneManager.LoadScene(1);
            
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "End")
        {
            SceneManager.LoadScene(1);
            FireManagerScript.RemovePower();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //FireManagerScript.AttackLight();
        }
    }
}
