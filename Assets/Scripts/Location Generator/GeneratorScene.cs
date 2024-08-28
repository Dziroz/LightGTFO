using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class GeneratorScene : MonoBehaviour
{
    [SerializeField] private GameObject[] sceneSimplePrefabs;
    [SerializeField] private GameObject[] sceneMissionPrefabs;
    [SerializeField] private GameObject sceneStartPrefab;
    [SerializeField] private GameObject sceneEndPrefab;

    [SerializeField] private int count;
    [SerializeField] private int[] MissionPosition;

    [SerializeField] private float rangeBetwenPrefabs;
    
    [SerializeField] private NavMeshSurface surfaces;


    private void Start()
    {
        surfaces = GetComponent<NavMeshSurface>();
        Generate();
        BuildNavMeshScene();
    }
    private void Generate()
    {
        //int countInScene = 1;
        int missionInScene =0;
        int currentPosition = 1;
        GameObject x = Instantiate(sceneStartPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        x.transform.parent = this.transform;
        for (int i = 1; currentPosition < count; i++)
        {
            currentPosition = i;
            int random = Random.Range(0, sceneSimplePrefabs.Length);
           if (missionInScene < MissionPosition.Length)
            {
                if (currentPosition == MissionPosition[missionInScene])
                {
                    x = Instantiate(sceneMissionPrefabs[missionInScene], new Vector3(0, 0, currentPosition * rangeBetwenPrefabs), Quaternion.identity);
                    x.transform.parent = this.transform;
                    missionInScene++;
                    continue;
                }
           }
            x = Instantiate(sceneSimplePrefabs[random], new Vector3(0,0, currentPosition * rangeBetwenPrefabs), Quaternion.identity);
            x.transform.parent = this.transform;
            //Debug.Log(i);

        }
        x = Instantiate(sceneEndPrefab, new Vector3(0, 0, (currentPosition+1)*rangeBetwenPrefabs), Quaternion.identity);
        x.transform.parent = this.transform;

    }

    private void BuildNavMeshScene()
    {
        surfaces.BuildNavMesh();
    }
}
