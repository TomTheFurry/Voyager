using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarNumber : MonoBehaviour
{
    
    void Start()
    {
        TechStorage.instance.onTechStatusChanged.AddListener(OnEnable);
        OnEnable();
    }

    private void OnEnable()
    {
        int stars = PlayerData.GetData().stars;
        Text text = GetComponent<Text>();
        text.text = stars.ToString();
    }
}
