using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimator : MonoBehaviour
{
    // Unity comments
    [TextArea(7, 10)]
    public string Notes = "This script is used to animate text.\n" +
        "Add '$' to begin and end a tag to change animation settings.\n" +
        "'$s0.3$': set the speed of the animation to 0.3 times of default\n" +
        "'$p0.3$': Pause for 0.3 seconds.\n" +
        "'$v0.3$': Set volume to 30%\n" +
        "'$k$': Wait for keypress (NOT IMPL YET!)\n";

    // The per char delay in seconds
    public float defaultTextDelay = 0.1f;
    // The audio effect source (optional)
    public AudioSource audioSource = null;
    TextMeshProUGUI textMesh;

    public Image panel;

    private Coroutine _currentAnimation = null;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        message = new AnimationMessage()
        {
            text = textMesh.text,
            caller = gameObject,
            autoClose = 0,
            priority = int.MinValue,
        };
        textMesh.useMaxVisibleDescender = true;
    }

    // setter calls triggerAnimation()
    // getter returns target text
    private AnimationMessage _msg;
    public AnimationMessage message {
        get => _msg;
        set => triggerAnimation(value);
    }

    private static int[] computeIndexerToRawString(TMP_CharacterInfo[] charInfo) {
        int[] indexer = new int[charInfo.Length];
        int i = 0;
        foreach (TMP_CharacterInfo c in charInfo)
        {
            indexer[i] = c.index;
            i++;
        }
        return indexer;
    }

    struct AnimatorTag {
        public int rawStringIndex;
        public char type;
        public string arg;
    }

    internal static List<char> SPEC_CHAR = new List<char> {
        's', // Set Speed of char (multiplier)
        'p', // Pause (seconds)
        'v', // Set volume (multiplier, 0.0 - 1.0)
        'k', // Wait for keypress (TODO)
    };

    private static void parseAnimatorTag(string str, StringBuilder builder, List<AnimatorTag> tag)
    {
        if (str == "") {
            builder.Append('$');
            return;
        }
        char specialChar = str[0];
        // if specialchar not SPEC_CHAR, throw error string
        if (SPEC_CHAR.Contains(specialChar))
        {
            tag.Add(new AnimatorTag()
            {
                rawStringIndex = builder.Length,
                type = specialChar,
                arg = str.Substring(1)
            });
        } else
        {
            builder.Append("<color=red>$INVALID_ANIMATOR_TAG:" + specialChar + "$</color>");
            return;
        }
    }

    // Strip all animatorTag (begins and end with '$') from the string
    private static AnimatorTag[] stripAnimatorTag(string rawText, out string processedString)
    {
        List<AnimatorTag> tags = new List<AnimatorTag>();
        StringBuilder builder = new StringBuilder(rawText.Length);
        int i = 0;
        while (i < rawText.Length)
        {
            int start = rawText.IndexOf('$', i);
            if (start == -1)
            {
                builder.Append(rawText.Substring(i));
                break;
            }
            builder.Append(rawText.Substring(i, start - i));
            int end = rawText.IndexOf('$', start + 1);
            if (end == -1)
            {
                builder.Append("<color=red>$</color>");
                break;
            }
            parseAnimatorTag(rawText.Substring(start + 1, end - start - 1), builder, tags);
            i = end + 1;
        }
        processedString = builder.ToString();
        return tags.ToArray();
    }


    bool triggerAnimation(AnimationMessage msg)
    {
        Debug.Log("Triggering animation for text: " + msg);
        if (_currentAnimation != null)
        {
            if ((msg.lowPriority && msg.priority <= _msg.priority) ||
                (!msg.lowPriority && msg.priority < _msg.priority))
            {
                return false;
            }
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
            if (_msg.onReplaced != null) _msg.onReplaced.Invoke(_msg, msg);
        }
        if (_msg == null) msgPopup();
        _msg = msg;
        AnimatorTag[] tags = stripAnimatorTag(msg.text, out string rawText);
        textMesh.SetText(rawText);
        textMesh.ForceMeshUpdate();
        int[] indexer = computeIndexerToRawString(textMesh.textInfo.characterInfo);
        _currentAnimation = StartCoroutine(animate(msg, tags, indexer));
        return true;
    }

    // the animate coroutine
    IEnumerator animate(AnimationMessage msg, AnimatorTag[] tags, int[] indexer)
    {
        textMesh.maxVisibleCharacters = 0;
        int tagRead = 0;
        int charRead = 0;
        float textDelay = defaultTextDelay;
        while (charRead < textMesh.textInfo.characterCount)
        {
            bool doDelay = true;
            while (tagRead < tags.Length && tags[tagRead].rawStringIndex <= indexer[charRead])
            {
                AnimatorTag tag = tags[tagRead];
                switch (tag.type)
                {
                    case 's':
                        textDelay = defaultTextDelay * float.Parse(tag.arg);
                        break;
                    case 'p':
                        yield return new WaitForSeconds(float.Parse(tag.arg));
                        doDelay = false;
                        break;
                    case 'v':
                        if (audioSource != null)
                        {
                            audioSource.volume = float.Parse(tag.arg);
                        }
                        break;
                    case 'k':
                        Debug.Log("TODO: Wait for keypress");
                        break;
                    default:
                        Debug.LogError("Invalid animator tag: " + tag.type + " " + tag.arg);
                        break;
                }
                tagRead++;
            }
            if (doDelay)
            {
                yield return new WaitForSeconds(textDelay);
            }
            textMesh.maxVisibleCharacters = ++charRead;
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
        if (msg.onAnimationComplete != null) msg.onAnimationComplete.Invoke(msg);
        if (msg.autoClose > 0)
        {
            yield return new WaitForSeconds(msg.autoClose);
        }
        if (msg.autoClose < 0)
        {
            _currentAnimation = null;
        }
        else
        {
            if (msg.onBoxClose != null) msg.onBoxClose.Invoke(msg);
            _msg = null;
            msgPopdown();
        }
        _currentAnimation = null;
    }

    private void msgPopup() {
        panel.enabled = true;
        textMesh.enabled = true;
    }
    private void msgPopdown() {
        panel.enabled = false;
        textMesh.enabled = false;
    }

}
