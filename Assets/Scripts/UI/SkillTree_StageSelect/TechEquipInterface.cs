using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechEquipInterface : MonoBehaviour
{
    public int verticalSpacing = 62;
    public int horizontalSpacing = 74;
    public GameObject techIconPrefab;

    private Button btn;
    private GameObject img;
    private string type;
    private List<Tech> teches;

    private float iconHeight;
    private int lineNum;
    void Start()
    {
        btn = transform.GetChild(0).GetComponent<Button>();
        img = transform.GetChild(1).gameObject;
        btn.onClick.AddListener(closeInterface);

        type = TechEquipInterfaceController.type;
        teches = TechStorage.instance.getTechesIsUnlocked(type);

        iconHeight = techIconPrefab.transform.GetComponent<RectTransform>().sizeDelta.y;
        lineNum = (int)Mathf.Ceil(((float)teches.Count) / 3);
        int line = lineNum < 2 ? 2 : lineNum;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (line + 1) * verticalSpacing + iconHeight * line);
    }

    public void createTechIcon()
    {
        List<Tech> teches = TechStorage.instance.getTechesIsUnlocked(type);
        if (teches.Count == 0)
            return;

        instantiateTechIcon(teches, lineNum);
    }
    public void instantiateTechIcon(List<Tech> teches, int line)
    {
        int count;
        if (line < 0) line = 0;

        if (line > 1)
        {
            instantiateTechIcon(teches, line - 1);
            count = 3;
        }
        else
        {
            count = teches.Count - 3 * (lineNum - 1);
        }

        float yPos = (verticalSpacing + iconHeight) * line - iconHeight / 2;

        while (count > 0)
        {
            int index = teches.Count - 1;
            if (index < 0)
                break;
            Tech tech = teches[index];
            teches.Remove(tech);

            GameObject icon = Instantiate(techIconPrefab, img.transform);
            // set positon
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2((horizontalSpacing + iconHeight) * (count - 2), yPos);
            // set icon
            icon.transform.GetChild(0).GetComponent<Image>().sprite = tech.icon;
            // add close method
            icon.GetComponent<Button>().onClick.AddListener(closeInterface);
            // set tech
            icon.GetComponent<TechEquipIcon>().tech = tech;
            
            count--;
        }
    }
    public void closeInterface()
    {
        TechEquipInterfaceController.instance.closeInterface(gameObject, img);
        btn.interactable = false;
    }

    public void destroyInterface()
    {
        Destroy(gameObject);
    }
}
