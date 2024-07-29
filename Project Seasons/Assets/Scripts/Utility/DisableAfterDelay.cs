using System.Collections;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
    [SerializeField]
    GameObject[] objToDisable;

    [SerializeField]
    float delayTime = 1f;

    WaitForSeconds m_DelayTime;

    private void Start()
    {
        m_DelayTime = new WaitForSeconds(delayTime);

        StartCoroutine(DelayTimer());
    }

    IEnumerator DelayTimer()
    {
        yield return m_DelayTime;

        foreach (var obj in objToDisable)
        {
            obj.SetActive(false);
        }
    }
}
