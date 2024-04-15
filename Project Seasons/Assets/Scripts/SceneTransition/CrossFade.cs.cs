using System.Collections;
using UnityEngine;

[System.Serializable]
public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;

    public override IEnumerator AnimateTransitionIn()
    {
        // Starte die Einblend-Animation
        float duration = 1f;
        float targetAlpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            crossFade.alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        crossFade.alpha = targetAlpha; // Stelle sicher, dass der Alpha-Wert korrekt gesetzt ist
    }

    public override IEnumerator AnimateTransitionOut()
    {
        // Starte die Ausblend-Animation
        float duration = 1f;
        float targetAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            crossFade.alpha = Mathf.Lerp(1f, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        crossFade.alpha = targetAlpha; // Stelle sicher, dass der Alpha-Wert korrekt gesetzt ist
    }
}
