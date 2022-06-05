using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public int gridCount = 5; //5 by 5 by 5
    public float gridSize;
    public int bonusCount = 5;
    public GameObject bonusPrefab;

    void Start()
    {
        // 5 means -2 to 2
        if (gridCount % 2 == 0) gridCount++;
        int halfCount = gridCount / 2;
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < bonusCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-halfCount, halfCount+1),
                Random.Range(-halfCount, halfCount+1),
                Random.Range(-halfCount, halfCount+1)
            );
            pos *= gridSize;
            
            if (!positions.TrueForAll((p) => !pos.Equals(p)))
            {
                i--;
                continue;
            }
            positions.Add(pos);
            GameObject obj = Instantiate(bonusPrefab, transform, false);
            obj.transform.localPosition = pos;
        }
    }
}
