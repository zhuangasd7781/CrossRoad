using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public float offset_Y;
    public List<GameObject> terrainObjects;
    public GameObject spawnObject;
    private int lastIndex;
    private void Start()
    {
        //InvokeRepeating(nameof(Spawn), 0.2f, Random.Range(5f, 7f));
        //  CheckPosition();
    }

    public void CheckPosition()
    {
        if (transform.position.y - Camera.main.transform.position.y < offset_Y / 2)
        {
            transform.position = new Vector3(0, Camera.main.transform.position.y + offset_Y, 0);
            SpawnTerrain();
        }


    }

    private void SpawnTerrain()
    {
        var index = Random.Range(0, terrainObjects.Count);

        while (lastIndex == index)
        {
            index = Random.Range(0, terrainObjects.Count);
        }
        lastIndex = index;
        spawnObject = terrainObjects[lastIndex];

        Instantiate(spawnObject, transform.position, Quaternion.identity);

        //target.GetComponent<MoveForward>().dir = direction;
    }
}
