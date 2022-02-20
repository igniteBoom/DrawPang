using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum When
{
    None,
    Awake,
    Enable,
    Start,
    Disable
}

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingString : MonoBehaviour
{
    public When _when;
    public TextMeshProUGUI _string;
    public string _typingString = string.Empty;
    public float _startDelay = 0f;
    public float _during = 0f;

    public void SetTypingString(string message, float startDelay = 0f, float during = 0f)
    {
        _typingString = message;
        _startDelay = startDelay;
        _during = during;

        StartCoroutine(Typing(_typingString, _startDelay, _during));
    }
    private void Awake()
    {
        _string = this.GetComponent<TextMeshProUGUI>();
        if (_when == When.Awake)
        {
            StartCoroutine(Typing(_typingString, _startDelay, _during));
        }
    }

    private void OnEnable()
    {
        _string = this.GetComponent<TextMeshProUGUI>();
        if (_when == When.Enable)
        {
            StartCoroutine(Typing(_typingString, _startDelay, _during));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _string = this.GetComponent<TextMeshProUGUI>();
        if (_when == When.Start)
        {
            StartCoroutine(Typing(_typingString, _startDelay, _during));
        }
    }
    private void OnDisable()
    {
        _string = this.GetComponent<TextMeshProUGUI>();
        if (_when == When.Disable)
        {
            StartCoroutine(Typing(_typingString, _startDelay, _during));
        }
    }
    IEnumerator Typing(string message, float startDelay = 0f, float during = 0f)
    {
        float oneLetterTime;
        if (during == 0) oneLetterTime = 1f;
        else oneLetterTime = during / (float)message.Length;

        yield return new WaitForSeconds(startDelay);
        for (int i = 0; i < message.Length; i++)
        {
            _string.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(oneLetterTime);
        }
    }
}
