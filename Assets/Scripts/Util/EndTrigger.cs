using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTrigger : MonoBehaviour
{
    public UnityEvent onEndTrigger;
    public float minSpeed = float.MaxValue;
    MovementControl2 control;
    CanvasHandler canvas;

    private void Start()
    {
        control = FindObjectOfType<MovementControl2>();
        canvas = FindObjectOfType<CanvasHandler>();
    }


    private bool triggered = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.attachedRigidbody.velocity.magnitude < minSpeed&&
            !triggered) {
            onEndTrigger.Invoke();
            triggered = true;
            control.DisableSpaceshipInput();
            control.SlowPlayer();
            canvas.OnSuccess();
        }
    }

}
