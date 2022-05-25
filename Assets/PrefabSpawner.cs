using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;

    public void Spawn(Transform transform)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
