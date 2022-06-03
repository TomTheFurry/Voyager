using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModelInInterface : MonoBehaviour
{
    public GameObject ship;
    public float rotateSpeed = 6f;
    public float swingSpeed = 1f;

    private float timer = 0.5f;
    private bool reset = true;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rotateSpeed /= 100f;
        swingSpeed /= 100f;
    }

    private void Update()
    {

        ship.transform.Rotate(Vector3.up * rotateSpeed * 360 * Time.deltaTime);
        ship.transform.Rotate(Vector3.back * swingSpeed * Mathf.Sin(timer * Mathf.PI));
        //ship.transform.Translate(Vector3.up * swingSpeed * 0.007f * Mathf.Sin(timer * Mathf.PI));

        timer += Time.deltaTime * 0.5f;
    }
}
