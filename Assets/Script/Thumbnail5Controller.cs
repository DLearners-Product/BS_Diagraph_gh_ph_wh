using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbnail5Controller : MonoBehaviour
{
    public GameObject spawnParent;
    public GameObject[] puzzleParts;

    void Start()
    {
        SpawnPuzzleObjects();
    }

    void Update()
    {
        
    }

    void SpawnPuzzleObjects()
    {
        foreach (var item in puzzleParts)
        {
            var spawnedObj = Instantiate(item, spawnParent.transform);
        }
    }
}
