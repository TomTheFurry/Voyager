using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipModelInInterface : MonoBehaviour
{
    public GameObject ship;
    public float rotateSpeed = 6f;
    public float swingSpeed = 1f;
    public Transform cam;
    public List<CamPos> positions;

    private float timer = 0.5f;
    private Vector3 startLocalPos;

    [Serializable]
    public struct CamPos
    {
        public Tech tech;
        public Vector3 localPos;
    }

    private void Start()
    {
        startLocalPos = cam.localPosition;

        if (ship == null)
            ship = gameObject;
        rotateSpeed /= 100f;
        swingSpeed /= 100f;

        ShipModelChange shipModelChange = GetComponent<ShipModelChange>();
        if (shipModelChange != null)
        {
            shipModelChange.onModelUpdate.AddListener(updatePos);
        }
    }

    private void Update()
    {

        ship.transform.Rotate(Vector3.up * rotateSpeed * 360 * Time.deltaTime);
        ship.transform.Rotate(Vector3.back * swingSpeed * Mathf.Sin(timer * Mathf.PI) * Time.deltaTime);
        //ship.transform.Translate(Vector3.up * swingSpeed * 0.007f * Mathf.Sin(timer * Mathf.PI));

        timer += Time.deltaTime * 0.5f;
    }

    public void updatePos(Tech tech)
    {
        foreach (CamPos camPos in positions)
        {
            if (camPos.tech == tech)
            {
                cam.localPosition = camPos.localPos;
                return;
            }
        }
        cam.localPosition = startLocalPos;
    }
}
