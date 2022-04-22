
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
    [TextArea(4, 4)]
    public string text;
    [TextArea(2, 6)]
    public string[] texts;
    public object Clone()
    {
        return MemberwiseClone();
    }
}
