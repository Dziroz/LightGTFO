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
    }
    private void Generate()
    {
        int countInScene = 1;
        int missionInScene =0;
        int currentPosition = 1;
        Instantiate(sceneStartPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        for (int i = 1; i <= count; i++)
        {
            int random = Random.Range(0, sceneSimplePrefabs.Length);
            if(currentPosition == MissionPosition[missionInScene])
            {
                Instantiate(sceneMissionPrefabs[missionInScene], new Vector3(0, 0, i * rangeBetwenPrefabs), Quaternion.identity);
                missionInScene++;
                currentPosition = i;
                countInScene++;
                continue;
            }
            Instantiate(sceneSimplePrefabs[random], new Vector3(0,0, i * rangeBetwenPrefabs), Quaternion.identity);
            countInScene++;
            currentPosition = i;

        }
        Instantiate(sceneEndPrefab, new Vector3(0, 0, (currentPosition+1)*rangeBetwenPrefabs), Quaternion.identity);

    }

    private void BuildNavMeshScene()
    {
        surfaces.BuildNavMesh();
    }
}
