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

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
                lineRenderer.SetPositions(pos);
                lineRenderer.enabled = true;
            } else
            {
                Vector3[] pos = new Vector3[2];
                pos[0] = origin.position;
                pos[1] = origin.position + origin.forward.normalized*maxDistance;
                lineRenderer.SetPositions(pos);
                lineRenderer.enabled = true;
            }
        } else
        {
            lineRenderer.enabled = false;
        }
    }
}
