using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer))]
public class Lazer : MonoBehaviour
{ 
    public Transform origin;
    public InputActionReference action;
    public float maxDistance;
    public LayerMask layer;
    private bool hitted = false;
    private Vector3 targetPos = Vector3.zero;
    private LineRenderer lineRenderer;

    public BeamEffect beam;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void SetBeam(Vector3[] pos)
    {
        lineRenderer.SetPositions(pos);
        lineRenderer.enabled = true;
        Vector3 center = (pos[0] + pos[1]) / 2;
        float length = (pos[1] - pos[0]).magnitude;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, (pos[1]-pos[0]).normalized);
        beam.SetLengthAndRotation(length, rot, center);
    }

    private void Update()
    {
        if (action.action.IsPressed()) {
            RaycastHit hit;
            Physics.Raycast(origin.position, origin.forward, out hit, maxDistance,
                layer.value, QueryTriggerInteraction.Ignore);
            if (hit.collider != null) {
                Destroy(hit.collider.gameObject);
                Vector3[] pos = new Vector3[2];
                pos[0] = origin.position;
                pos[1] = hit.point;
                SetBeam(pos);
            } else
            {
                Vector3[] pos = new Vector3[2];
                pos[0] = origin.position;
                pos[1] = origin.position + origin.forward.normalized*maxDistance;
                SetBeam(pos);
            }
        } else
        {
            lineRenderer.enabled = false;
            beam.StopEffect();
        }
    }
}
