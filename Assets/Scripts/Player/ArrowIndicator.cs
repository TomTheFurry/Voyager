using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public float unitScale = 1f;
    public Vector3 valueScale = Vector3.one;
    public float arrowScale = 1f;
    public float arrowOffset = 1f;
    public char arrowChar = '>';
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
        
        arrowLeft.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        arrowRight.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        arrowUp.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        arrowDown.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        arrowForward.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        arrowBack.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        
        arrowLeft.transform.localPosition = arrowLeft.transform.localPosition * arrowOffset;
        arrowRight.transform.localPosition = arrowRight.transform.localPosition * arrowOffset;
        arrowUp.transform.localPosition = arrowUp.transform.localPosition * arrowOffset;
        arrowDown.transform.localPosition = arrowDown.transform.localPosition * arrowOffset;
        arrowForward.transform.localPosition = arrowForward.transform.localPosition * arrowOffset;
        arrowBack.transform.localPosition = arrowBack.transform.localPosition * arrowOffset;
    }

    private string repeatChar(char c, int count)
    {
        string s = "";
        for (int i = 0; i < count; i++)
        {
            s += c;
        }
        return s;
    }

    public void setValue(Vector3 value)
    {
        value.Scale(valueScale);
        arrowLeft.SetActive(value.x < 0);
        arrowRight.SetActive(value.x > 0);
        arrowUp.SetActive(value.y > 0);
        arrowDown.SetActive(value.y < 0);
        arrowForward.SetActive(value.z > 0);
        arrowBack.SetActive(value.z < 0);
        
        if (arrowLeft.activeSelf)
        {
            int numOfArrow = value.x < 0 ? (int)(value.x * -1) : (int)value.x;
            arrowLeft.GetComponent<TextMesh>().text = repeatChar(arrowChar, numOfArrow);
        }
        if (arrowRight.activeSelf)
        {
            int numOfArrow = value.x > 0 ? (int)value.x : (int)(value.x * -1);
            arrowRight.GetComponent<TextMesh>().text = repeatChar(arrowChar, numOfArrow);
        }
        if (arrowUp.activeSelf)
        {
            int numOfArrow = value.y > 0 ? (int)value.y : (int)(value.y * -1);
            arrowUp.GetComponent<TextMesh>().text = repeatChar(arrowChar, numOfArrow);
        }
        if (arrowDown.activeSelf)
        {
            int numOfArrow = value.y < 0 ? (int)(value.y * -1) : (int)value.y;
            arrowDown.GetComponent<TextMesh>().text = repeatChar(arrowChar, numOfArrow);
        }
        if (arrowForward.activeSelf)
        {
            int numOfArrow = value.z > 0 ? (int)value.z : (int)(value.z * -1);
            arrowForward.GetComponent<TextMesh>().text = repeatChar(arrowChar, numOfArrow);
        }
        if (arrowBack.activeSelf)
        {
            int numOfArrow = value.z < 0 ? (int)(value.z * -1) : (int)value.z;
            arrowBack.GetComponent<TextMesh>().text = repeatChar(arrowChar, numOfArrow);
        }
    }

}
