using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using UnityEngine.UI;


public class bgcontroll : MonoBehaviour
{
    [SerializeField] string volumeParameter = "bgm";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    //[SerializeField] private Toggle toggle;
    //private bool disableToggleEvent;


    private void Awake()
    {
        slider.onValueChanged.AddListener(Sliderchange);
        //toggle.onValueChanged.AddListener(Togglechange);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    /*private void Togglechange(bool onoff)
    {
        if (disableToggleEvent)
            return;

        if (onoff)
            slider.value = slider.maxValue;
        else
            slider.value = slider.minValue;
    } */     

    private void Sliderchange(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value)*multiplier);
        /*disableToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;*/
    }
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
