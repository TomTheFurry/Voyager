using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipsAuto : MonoBehaviour
{
    public GameObject highlighterPrefab;
    private List<GameObject> highlighters;
    public GameObject temp;

    private void Start()
    {
        highlighters = new List<GameObject>();
        addTooltips("space garbage", temp);
    }

    public void addTooltips(string keyword, GameObject customTooltip)
    {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        text.ForceMeshUpdate();
        
        int startIndex = -1,
            endIndex = -1,
            textLength = 0;
        string description = "";
        foreach (var temp in text.textInfo.characterInfo)
            description += temp.character;
        textLength = description.Length;

        do
        {
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
                        highlighter = instantiateHighlighter(customTooltip);
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

                highlighter = instantiateHighlighter(customTooltip);
                highlighterTransform = highlighter.GetComponent<RectTransform>();
                
                Vector3 newPos = new Vector3((cStart.topLeft.x + cEnd.bottomRight.x) / 2, (top + bottom) / 2, 0);
                highlighterTransform.localPosition = newPos;
                highlighterTransform.sizeDelta = new Vector2((cEnd.bottomRight.x - cStart.topLeft.x), (top - bottom));
            } // end if
        } while (startIndex != -1 && endIndex <= textLength);
    } //end start()

    public GameObject instantiateHighlighter(GameObject customTooltip)
    {
        GameObject highlighter = Instantiate(highlighterPrefab);
        highlighters.Add(highlighter);
        highlighter.transform.parent = gameObject.transform;
        highlighter.transform.localScale = Vector3.one;
        highlighter.GetComponent<AdvancedTooltip>().customTooltip = customTooltip;

        return highlighter;
    }

    public void clearTooltips()
    {
        foreach (GameObject highlighter in highlighters)
        {
            Destroy(highlighter);
        }
        highlighters.Clear();
    }
}
