using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkField : MonoBehaviour
{
    public int junkCount = 0;
    public Transform container;
    public GameObject sampleObj;
    public GameObject[] junkAssets;

    // the area of 'life' area
    public BoxCollider lifeArea;
    // the bounds of speed
    public Bounds speedBound;

    private GameObject[] computedSample;

    private void SpawnObj()
    {
        int index;
        do { index = Random.Range(0, computedSample.Length); }
        while (computedSample[index] == null);
        Vector3 pos = new Vector3(
            Random.Range(lifeArea.bounds.min.x, lifeArea.bounds.max.x),
            Random.Range(lifeArea.bounds.min.y, lifeArea.bounds.max.y),
            Random.Range(lifeArea.bounds.min.z, lifeArea.bounds.max.z));
        
        
        Quaternion quat = Quaternion.Euler(
            Random.Range(0, 360),
            Random.Range(0, 360),
            Random.Range(0, 360));
        Vector3 speed = new Vector3(
            Random.Range(speedBound.min.x, speedBound.max.x),
            Random.Range(speedBound.min.y, speedBound.max.y),
            Random.Range(speedBound.min.z, speedBound.max.z));

        GameObject obj = Instantiate(computedSample[index], container);
        obj.transform.position = pos;
        obj.transform.rotation = quat;
        obj.GetComponent<Rigidbody>().velocity = speed;
        // TODO: Random angler velocity?
        obj.SetActive(true);
    }
    
    void Start()
    {
        computedSample = new GameObject[junkAssets.Length];
        for (int i = 0; i < junkAssets.Length; i++)
        {
            GameObject obj = Instantiate(sampleObj);
            obj.SetActive(false);
            obj.GetComponent<MeshCollider>().convex = true;
            try
            {
                obj.GetComponent<MeshFilter>().mesh = junkAssets[i].GetComponent<MeshFilter>().sharedMesh;
                obj.GetComponent<MeshRenderer>().materials = junkAssets[i].GetComponent<MeshRenderer>().sharedMaterials;
                obj.GetComponent<MeshCollider>().sharedMesh = junkAssets[i].GetComponent<MeshFilter>().sharedMesh;
                computedSample[i] = obj;
            } catch {
               
            }
        }

        for (int i = 0; i < junkCount; i++) SpawnObj();
    }

    private void LateUpdate()
    {
        while (container.childCount < junkCount) SpawnObj();
    }
}