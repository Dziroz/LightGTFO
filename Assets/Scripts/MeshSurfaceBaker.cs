using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public class MeshSurfaceBaker : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    void Start()
    {
        
    }


    void Update()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
            if (i > surfaces.Length)
            {
                i = 0;
            }
        }
    }
}
