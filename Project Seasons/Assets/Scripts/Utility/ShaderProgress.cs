using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderProgress : MonoBehaviour
{
    [SerializeField]
    Material imageMaterial;

    [Header ("DELAY SETTINGS")]

    [SerializeField]
    bool startWithDelay;
    bool canStartFade;

    [SerializeField]
    float startDelayTime;
    WaitForSeconds _startDelayTime;

    [Header("PROGRESS SETTINGS")]

    [SerializeField, Range(0, 1.1f)]
    float progressValueStart;

    [SerializeField, Range(0, 1.1f)]
    float progressValueEnd;
    float _progressValue;
    int _progressState;

    [SerializeField]
    float timeToComplete;

    private void OnEnable()
    {
        _startDelayTime = new WaitForSeconds(startDelayTime);

        if (progressValueStart < progressValueEnd)
            _progressState = 1;
        else
            _progressState = 0;

        if (startWithDelay)
            StartCoroutine(FadeProgress());
        else
        {
            canStartFade = true;
            _progressValue = progressValueStart;
        }
    }

    private void OnDisable()
    {
        imageMaterial.SetFloat("_Progress", progressValueStart);

    }

    private IEnumerator FadeProgress()
    {
        canStartFade = false;

        yield return _startDelayTime;
        canStartFade = true;
    }

    private void Update()
    {
        if (!canStartFade) return;

        if (_progressState == 1)
            _progressValue += Time.deltaTime / timeToComplete;
        else
            _progressValue -= Time.deltaTime / timeToComplete;

        imageMaterial.SetFloat("_Progress", _progressValue);
        IsCompleteCheck(_progressState);
    }
    bool IsCompleteCheck(int state)
    {
        if (state == 1 && _progressValue >= 1f)
            return true;
        else if (_progressValue <= 1f)
            return true;

        return false;
    }
}
