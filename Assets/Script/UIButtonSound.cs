using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{

    public AudioClip ClickedSound;
    public AudioClip HoverSound;

    //get button component
    private Button button { get { return GetComponent<Button>(); } }
    // get audiosource
    private AudioSource source { get { return GetComponent<AudioSource>(); } }


    void Start()

    {
        //bind an AudioSource on its
        gameObject.AddComponent<AudioSource>();
        //set default sound
        source.clip = HoverSound;

        source.playOnAwake = false;


        button.onClick.AddListener(() => PlayClickSoud());


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData == null)
        {
            throw new System.ArgumentNullException(nameof(eventData));
        }

        source.clip = HoverSound;
        source.PlayOneShot(HoverSound);
    }

    void PlayClickSoud()

    {

        source.clip = ClickedSound;
        source.PlayOneShot(ClickedSound);

    }
}
