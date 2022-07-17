using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextTrigger : MonoBehaviour
{
    public TextAnimator animator;
    public AnimationMessage message;
    public bool triggerOnce = false;
    public UnityEvent onTrigger;
    
    // Mesh Colloder trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider");
        if (other.gameObject.tag == "Player")
        {
            if (onTrigger != null) onTrigger.Invoke();
            animator.message = (AnimationMessage)message.Clone();
            if (triggerOnce)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
