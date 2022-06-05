using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeReoriantator : MonoBehaviour
{
    void Start()
    {
        Randomize();
    }

    void Randomize() {
        int x = Random.Range(0, 4);
        int y = Random.Range(0, 4);
        int z = Random.Range(0, 4);
        transform.localRotation = Quaternion.Euler(new Vector3(x * 90, y * 90, z * 90));
    }
}
