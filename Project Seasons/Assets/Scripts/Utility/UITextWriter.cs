using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextWriter : MonoBehaviour
{
    [Header("Text Writer")]
    [SerializeField]
    TextMeshPro textElement;
    
    [SerializeField]
    string TextToWrite;
    
    [SerializeField]
    float TimeToWrite;

    [SerializeField]
    bool hasStartDelay;
    bool canStartDelay;

    [SerializeField]
    float startDelayTime;

    float timer;
    int Index;

    private void OnEnable()
    {
        textElement = GetComponent<TextMeshPro>();

        if (hasStartDelay)
        {
            canStartDelay = false;
            StartCoroutine(DelayBeforeStart());
        }
        else canStartDelay = true;
    }

    private void Update()
    {
        if (!canStartDelay) return;

        if (textElement != null)
        {
            timer -= Time.deltaTime;
            while (timer < 0f)
            {
                timer += TimeToWrite;
                Index++;
                textElement.text = TextToWrite.Substring(0, Index);

                if (Index >= TextToWrite.Length)
                {
                    enabled = false;
#if UNITY_EDITOR
                    Debug.Log($"Text write from {gameObject.name} is done. Script disabled ");
#endif
                    return;
                }
            }
        }
    }

    IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(startDelayTime);
        canStartDelay = true;
    }
}
