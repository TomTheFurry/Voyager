using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipsKeyword : MonoBehaviour
{
    public bool switch1 = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name.Equals("Text (TMP)"))
            {
                TextMeshProUGUI text = t.gameObject.GetComponent<TextMeshProUGUI>();
                text.text = LangSystem.parseText((string)text.text);
            }
        }
    }
}
