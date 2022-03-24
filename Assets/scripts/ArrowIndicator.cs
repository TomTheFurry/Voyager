using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public float unitScale = 1f;
    public Vector3 valueScale = Vector3.one;
    public float arrowScale = 1f;
    private GameObject arrowLeft;
    private GameObject arrowRight;
    private GameObject arrowUp;
    private GameObject arrowDown;
    private GameObject arrowForward;
    private GameObject arrowBack;

    // Start is called before the first frame update
    void Start()
    {
        arrowLeft = transform.Find("ArrowLeft").gameObject;
        arrowRight = transform.Find("ArrowRight").gameObject;
        arrowUp = transform.Find("ArrowUp").gameObject;
        arrowDown = transform.Find("ArrowDown").gameObject;
        arrowForward = transform.Find("ArrowForward").gameObject;
        arrowBack = transform.Find("ArrowBack").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
