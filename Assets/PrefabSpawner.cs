using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public bool enableOnSpawn = true;

    public void Spawn(Transform transform)
    {
        GameObject obj = Instantiate(prefab, transform.position, transform.rotation);
        if (enableOnSpawn)
        {
            obj.SetActive(true);
        }
    }
}
