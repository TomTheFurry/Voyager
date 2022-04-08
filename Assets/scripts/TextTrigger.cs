using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public TextAnimator animator;
    public AnimationMessage message;
    
    // Mesh Colloder trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider");
        if (other.gameObject.tag == "Player")
        {
            animator.message = (AnimationMessage)message.Clone();
        }
    }
}
