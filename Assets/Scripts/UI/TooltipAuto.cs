using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipAuto : MonoBehaviour
{
    public GameObject highlighterPrefab;
    private List<GameObject> highlighters;
    private List<GameObject> customTooltips;

    private void Start()
    {
        highlighters = new List<GameObject>();
        customTooltips = new List<GameObject>();
    }

    private void addTooltip(GameObject customTooltip)
    {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        text.ForceMeshUpdate();

        string keyword = customTooltip.GetComponent<TooltipKeyword>().keyword.ToLower();

        int startIndex = -1,
            endIndex = -1;
        string description = "";
        foreach (var temp in text.textInfo.characterInfo)
            description += temp.character;
            
        GameObject highlighter;
        RectTransform highlighterTransform;
            
        startIndex = description.ToLower().IndexOf(keyword, startIndex + 1);
        endIndex = startIndex + keyword.Length;
            
        if (startIndex != -1)
        {
            TMP_CharacterInfo cStart = text.textInfo.characterInfo[startIndex],
                            cEnd = text.textInfo.characterInfo[endIndex - 1];
            float line = cStart.baseLine;
            float top = cStart.topLeft.y,
                bottom = cStart.bottomRight.y;
            for (int i = startIndex + 1; i < endIndex; i++)
            {
                TMP_CharacterInfo cInfo = text.textInfo.characterInfo[i];

                if (!line.Equals(cInfo.baseLine))
                {
                    highlighter = instantiateTooltip(customTooltip);
                    highlighterTransform = highlighter.GetComponent<RectTransform>();
                        
                    cEnd = text.textInfo.characterInfo[i - 1];
                    highlighterTransform.localPosition = new Vector3((cStart.topLeft.x + cEnd.bottomRight.x) / 2, (top + bottom) / 2, 0);
                    highlighterTransform.sizeDelta = new Vector2((cEnd.bottomRight.x - cStart.topLeft.x), (top - bottom));

                    cStart = cInfo;
                    cEnd = text.textInfo.characterInfo[endIndex - 1];
                    line = cInfo.baseLine;
                    top = cStart.topLeft.y;
                    bottom = cStart.bottomRight.y;
                }

                if (top < cInfo.topLeft.y)
                    top = cInfo.topLeft.y;

                if (bottom > cInfo.bottomRight.y)
                    bottom = cInfo.bottomRight.y;


            } // end for

            highlighter = instantiateTooltip(customTooltip);
            highlighterTransform = highlighter.GetComponent<RectTransform>();
                
            Vector3 newPos = new Vector3((cStart.topLeft.x + cEnd.bottomRight.x) / 2, (top + bottom) / 2, 0);
            highlighterTransform.localPosition = newPos;
            highlighterTransform.sizeDelta = new Vector2((cEnd.bottomRight.x - cStart.topLeft.x), (top - bottom));
        } // end if
    } //end start()

    public void addTooltips(GameObject customTooltip)
    {
        clearTooltips();
        addTooltip(customTooltip);
    }
    public void addTooltips(GameObject[] customTooltips)
    {
        clearTooltips();
        foreach (GameObject customTooltip in customTooltips)
        {
            addTooltip(customTooltip);
        }
    }


    private GameObject instantiateTooltip(GameObject customTooltip)
    {
        GameObject highlighter = Instantiate(highlighterPrefab, gameObject.transform);
        highlighters.Add(highlighter);

        //GameObject customTooltipTemp = Instantiate(temp, gameObject.transform);
        //customTooltips.Add(customTooltipTemp);
        
        string name = customTooltip.GetComponent<TooltipKeyword>().keyword.Replace(" ", "");
        highlighter.name = highlighter.name.Replace("(Clone)", "_" + name);
        highlighter.transform.localScale = Vector3.one;
        highlighter.GetComponent<AdvancedTooltip>().customTooltip = customTooltip;

        return highlighter;
    }

    public void clearTooltips()
    {
        foreach (GameObject customTooltip in customTooltips)
        {
            Destroy(customTooltip);
        }
        foreach (GameObject highlighter in highlighters)
        {
            Destroy(highlighter);
        }
        highlighters.Clear();
        customTooltips.Clear();
    }
}
