using System.Collections;
using UnityEngine;
using TMPro;

public class BookTypewriterTMP : MonoBehaviour
{
    [Header("BOOK PAGES")]

    [SerializeField]
    private TMP_Text bookPageA;

    [SerializeField]
    int bookPageALines;

    [SerializeField]
    private TMP_Text bookPageB;

    [Header("BOOK TEXT")]
    
    [SerializeField, TextArea(5, 20)]
    string textToType;

    [Header("START SETTINGS")]

    [SerializeField]
    bool startWithDelay;

    [SerializeField]
    float startDelayTime = 4.0f;

    [Header("TYPEWRITER SETTINGS")]

    [SerializeField]
    float charactersPerSecond = 24;

    [SerializeField]
    float interruptionDelay= 0.4f;

    [Header("IMAGE SETTINGS")]

    [SerializeField]
    SpriteRenderer image;

    [SerializeField]
    Material imageMaterial;

    [SerializeField]
    float timeToCompleteImage = 0.0f;

    [SerializeField]
    float pauseDelay = 1.0f;

    bool canFadeInImage;
    float imageProgress;

    TMP_Text currentPage;
    int curCharacterIndex;
    bool pageAIsDone = false;
    bool _insideTag = false;
    bool _outsideTag = false;
    bool canWrite = false;

	//Wait-Instances for Coroutines
    WaitForSeconds _startDelay;
    WaitForSeconds _standardDelay;
    WaitForSeconds _interruptDelay;
    WaitForSeconds _pauseDelay;

    void OnEnable()
    {
        _startDelay = new WaitForSeconds(startDelayTime);
        _standardDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interruptDelay = new WaitForSeconds(interruptionDelay);
        _pauseDelay = new WaitForSeconds(pauseDelay);

        imageProgress = 0.0f;
        imageMaterial.SetFloat("_Progress", imageProgress);
        image.enabled = false;

        currentPage = bookPageA;
        currentPage.text = textToType;

        bookPageA.maxVisibleCharacters = 0;
        bookPageB.maxVisibleCharacters = 0;

        canWrite = true;

        if (startWithDelay)
            StartCoroutine(StartDelayedTypewriter());
        else 
            StartCoroutine(Typewriter());
    }
    
	IEnumerator StartDelayedTypewriter()
    {
        yield return _startDelay;
        StartCoroutine(Typewriter());
    }
	
	IEnumerator Typewriter()
	{
		int totalVisibleCharacters = textToType.Length;

		while (curCharacterIndex < totalVisibleCharacters)
		{
			if (curCharacterIndex >= totalVisibleCharacters - 1)
			{
				yield break;
			}

			char character = textToType[curCharacterIndex];

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

				curCharacterIndex++;
			}
			else
			{
				currentPage.maxVisibleCharacters++;
				curCharacterIndex++;

				if (IsInterruptionCharacter(character))
				{
					yield return _interruptDelay;
				}
				else
				{
					yield return _standardDelay;
				}
			}

			if (!pageAIsDone && VisibleLineCount() == bookPageALines)
			{
				currentPage.maxVisibleCharacters--;
				curCharacterIndex--;

				int visibleChar = currentPage.maxVisibleCharacters;

				currentPage = bookPageB;
				currentPage.maxVisibleCharacters = visibleChar;

				pageAIsDone = true;
                canFadeInImage = true;

				image.enabled = true;
                canWrite = false;

                while (!canWrite)
				{
					yield return null;
				}
			}
		}
	}
	
    bool IsInterruptionCharacter(char character)
    {
        return (character == '?' || character == '.' || character == ',' || character == '!');
    }

    int VisibleLineCount()
    {
        int line;
        int characterCount;

        for (line = 0, characterCount = 0; line < currentPage.textInfo.lineInfo.Length; line++)
        {
            characterCount += currentPage.textInfo.lineInfo[line].characterCount;

            if (currentPage.maxVisibleCharacters <= characterCount)
                break;
        }

        return line + 1;
    }

    private void Update()
    {
        if (!canFadeInImage) return;

        imageProgress += Time.deltaTime / timeToCompleteImage;
        imageMaterial.SetFloat("_Progress", imageProgress);

        if (imageProgress >= 1f)
        {
            imageProgress = 1f;
            imageMaterial.SetFloat("_Progress", imageProgress);

            canFadeInImage = false;
            StartCoroutine(PauseTime());
        }
    }

    IEnumerator PauseTime()
	{
		yield return _pauseDelay;
        canWrite = true;
	}
}