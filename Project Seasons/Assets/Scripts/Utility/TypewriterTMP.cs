using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterTMP : MonoBehaviour
{
    [SerializeField]
    bool startWithDelay;

    [SerializeField]
    float startDelayTime = 4.0f;

    [Header("Typewriter Settings")]

    [SerializeField]
    float charactersPerSecond = 8;

    [SerializeField]
    float interpunctuationDelay = 0.4f;

    TMP_Text _textBox;

    // Basic Typewriter Functionality
    int _currentTextIndex = 0;
    bool _insideTag = false;
    bool _outsideTag = false;

    WaitForSeconds _startDelay;
    WaitForSeconds _simpleDelay;
    WaitForSeconds _interpunctuationDelay;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();

        _startDelay = new WaitForSeconds(startDelayTime);
        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
    }

    private void Start()
    {
        string textToType = _textBox.text;
        _textBox.maxVisibleCharacters = 0;

        if (startWithDelay)
            StartCoroutine(StartDelay(textToType));
        else StartTypewriter(textToType);
    }

    IEnumerator StartDelay(string textToType)
    {
        yield return _startDelay;
        StartCoroutine(Typewriter(textToType));
    }

    private void StartTypewriter(string textToType)
    {
        StartCoroutine(Typewriter(textToType));
    }

    private IEnumerator Typewriter(string textToType)
    {
        int totalVisibleCharacters = textToType.Length;

        while (_currentTextIndex < totalVisibleCharacters)
        {
            if (_currentTextIndex >= totalVisibleCharacters - 1)
            {
                yield break;
            }

            char character = textToType[_currentTextIndex];

            if (character == '<')
                _insideTag = true;
            else if (_insideTag && character == '>')
                _outsideTag = true;

            if (_insideTag)
            {
                if (_outsideTag)
                {
                    _outsideTag = false;
                    _insideTag = false;
                }

                _currentTextIndex++;
            }
            else
            {
                _textBox.maxVisibleCharacters++;
                _currentTextIndex++;

                if (IsInterruptionCharacter(character))
                {
                    yield return _interpunctuationDelay;
                }
                else
                {
                    yield return _simpleDelay;
                }

            }
        }
    }

    private bool IsInterruptionCharacter(char character)
    {
        return (character == '?' || character == '.' || character == ',' || character == '!');
    }

}