using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer), typeof(PrefabSpawner))]
public class Lazer : MonoBehaviour
{ 
    public Transform origin;
    public InputActionReference action;
    public float maxDistance;
    public LayerMask layer;
    private LineRenderer lineRenderer;
    private PrefabSpawner prefab;
    private List<Light> lazerLights;
    public BeamEffect beam;
    public FuelControl fuel;
    public Light lightPrefab;
    public Transform lightContainer;
    public float fuelUsage;
    public int lineSectments = 64;
    public float perLightObjMaxDistance = 1;
    public float lightFlicker = 0.1f;
    public float lineFlicker = 0.01f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        prefab = GetComponent<PrefabSpawner>();
    }

    private Vector3[] makeSectments(Vector3 origin, Vector3 target) {
        Vector3[] pos = new Vector3[lineSectments];
        float step = 1f / (lineSectments - 1);
        for (int i = 0; i < lineSectments; i++)
        {
            float t = step * i;
            pos[i] = Vector3.Lerp(origin, target, t);
            if (i != 0) // Don't fuzz the origin point
                pos[i] += new Vector3(Random.value-0.5f, Random.value-0.5f, Random.value-0.5f) * lineFlicker * 2;
        }
        return pos;
    }

    private void spawnLights(int num) {
        if (lazerLights == null)
            lazerLights = new List<Light>();
        if (lazerLights.Count < num)
            for (int i = lazerLights.Count; i < num; i++)
            {
                Light light = Instantiate(lightPrefab, Vector3.zero, Quaternion.identity, lightContainer);
                light.gameObject.SetActive(false);
                lazerLights.Add(light);
            }
    }

    private void setLights(Vector3 origin, Vector3 target) {
        int lightCount = (int)Mathf.Ceil((target - origin).magnitude / perLightObjMaxDistance);
        spawnLights(lightCount);
        for (int i = 0; i < lazerLights.Count; i++)
        {
            if (i < lightCount)
            {
                lazerLights[i].gameObject.SetActive(true);
                lazerLights[i].transform.position = Vector3.Lerp(origin, target, (float)i / lightCount);
                lazerLights[i].intensity = lightPrefab.intensity * (1 + (Random.value-0.5f) * lightFlicker);
            }
            else
            {
                lazerLights[i].gameObject.SetActive(false);
            }
        }
    }
    

    private void SetBeam(Vector3 origin, Vector3 target)
    {
        fuel.RecordUseFuel(fuelUsage);
        Vector3[] vecs = makeSectments(origin, target);
        lineRenderer.positionCount = vecs.Length;
        lineRenderer.SetPositions(vecs);
        lineRenderer.enabled = true;
        Vector3 center = (origin + target) / 2;
        float length = (target - origin).magnitude;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, (target - origin).normalized);
        beam.SetLengthAndRotation(length, rot, center);
        setLights(origin, target);
    }

    private void FixedUpdate()
    {
        if (action.action.IsPressed() && fuel.HasFuel()) {
            RaycastHit hit;
            Physics.Raycast(origin.position, origin.forward, out hit, maxDistance,
                layer.value, QueryTriggerInteraction.Ignore);
            if (hit.collider != null) {
                prefab.Spawn(hit.transform);
                Destroy(hit.collider.gameObject);
                SetBeam(origin.position, hit.point);
            } else
            {
                SetBeam(origin.position, origin.position + origin.forward.normalized * maxDistance);
            }
        } else
        {
            OnDisable();
        }
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        beam.StopEffect();
        if (lazerLights != null)
            foreach (Light light in lazerLights)
                light.gameObject.SetActive(false);
    }
}
