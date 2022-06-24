using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipModify : MonoBehaviour
{
    public bool showSuggestIcon = false;
    public GameObject suggestion;
    public GameObject icon;
    public List<Tech> suggestTechs = new List<Tech>();
    private List<GameObject> icons = new List<GameObject>();

    private void OnEnable()
    {
        suggestion.SetActive(showSuggestIcon);
        if (!showSuggestIcon)
            return;

        foreach (GameObject obj in icons)
            Destroy(obj);
        icons.Clear();

        int index = 0;
        foreach (Tech tech in suggestTechs)
        {
            GameObject obj = Instantiate(icon);
            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector3 pos = rect.localScale;
            pos.x = pos.x + index * (rect.rect.width + 20);
            obj.GetComponent<RectTransform>().localScale = pos;

            //obj.GetComponent<Image>().sprite = 
        }
    }
}
