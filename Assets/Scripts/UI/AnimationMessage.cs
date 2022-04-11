
using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationMessage : ICloneable
{
    public GameObject caller;
    public int priority;
    [Tooltip("Should this not replace same-priority message?")]
    public bool lowPriority;
    public float autoClose;
    [Tooltip("Can this be skipped by pressing button?")]
    public bool canSkip;
    public UnityEvent<AnimationMessage> onAnimationComplete;
    public UnityEvent<AnimationMessage> onBoxClose;
    public UnityEvent<AnimationMessage, AnimationMessage> onReplaced;
    [TextArea(15, 20)]
    public string text;
    public object Clone()
    {
        return MemberwiseClone();
    }
}
