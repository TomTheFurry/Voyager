using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeBG : MonoBehaviour
{
    public float fadeRate = 0.07f;
    public float fadeTime = 0.5f;
    // Start is called before the first frame update
    [SerializeField] private RawImage background;
    //[SerializeField] private RawImage star;
    public Texture[] photo = new Texture[3];
    [SerializeField] private float _x, _y;
    // Update is called once per frame
    void Start()
    {
        background.texture = photo[0];
        StartCoroutine(change());
        
    }
    void Update()
    {
        background.uvRect = new Rect(background.uvRect.position + new Vector2(_x, _y) 
            * Time.deltaTime, background.uvRect.size);
    }

    void changephoto() {
        //int randomphoto = randomphoto(Random.Range(0, 2));        
        //background.texture = photo[index];
    }

    IEnumerator fadeIn()
    {
        Color bgColor = background.color;
        float fadeTimePerStep = fadeRate * fadeTime;
        do
        {
            bgColor.r += fadeRate;
            bgColor.g += fadeRate;
            bgColor.b += fadeRate;
            background.color = bgColor;
            yield return new WaitForSecondsRealtime(fadeTimePerStep);
        } while (bgColor.r < 1f);
        bgColor.r = 1f;
        bgColor.g = 1f;
        bgColor.b = 1f;
        background.color = bgColor;
    }

    IEnumerator fadeOut()
    {
        Color bgColor = background.color;
        float fadeTimePerStep = fadeRate * fadeTime;
        do
        {
            bgColor.r -= fadeRate;
            bgColor.g -= fadeRate;
            bgColor.b -= fadeRate;
            background.color = bgColor;
            yield return new WaitForSecondsRealtime(fadeTimePerStep);
        } while (bgColor.r > 0f);
        bgColor.r = 0f;
        bgColor.g = 0f;
        bgColor.b = 0f;
        background.color = bgColor;
    }

    IEnumerator change()
    {
        int x = 0;
        int index = 0;
        while (true)
        {
            x++;
            if (x == 20) x = 0;
            yield return new WaitForSeconds(Random.Range(26, 32));            
            index++;
            //changephoto();
            yield return fadeOut();
            background.texture = photo[index];
            if (index == 2) index = -1;
            yield return fadeIn();
            //Debug.Log("111");
        }
       
    }
}
