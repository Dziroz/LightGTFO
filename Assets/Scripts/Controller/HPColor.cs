using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPColor : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer rend;
    [SerializeField] private PlayerController player;
    [SerializeField] private Material[] mat;
    [SerializeField] private int Hepe;
    void Start()
    {
        rend = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
        HP();
    }
    void HP()
    {
        Hepe = player.getHP();
        switch (Hepe)
        {
            case 3:
                rend.material = mat[0];
                break;
            case 2:
                rend.material = mat[1];
                break;
            case 1:
                rend.material = mat[2];
                break;
            default:
                break;
        }
;    }
}
