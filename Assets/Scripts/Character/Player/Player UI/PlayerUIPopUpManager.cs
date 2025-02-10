using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("DEATH POP UP")]
    [SerializeField] private GameObject deathPopUpGameObject;
    [SerializeField] private TextMeshProUGUI deathPopUpBackgroundText;
    [SerializeField] private TextMeshProUGUI deathPopUpText;
    [SerializeField] private CanvasGroup deathPopUpCanvasGroup;

    public void SendDeathPopUp()
    {
        deathPopUpGameObject.SetActive(true);
        deathPopUpBackgroundText.characterSpacing = 0f;

        StartCoroutine(StretchPopUpTextOverTime(deathPopUpBackgroundText, 8.32f, 9f));

        StartCoroutine(FadeInPopUpOverTime(deathPopUpCanvasGroup, 5f));

        StartCoroutine(WaitFadeOutPopUpOverTime(deathPopUpCanvasGroup, 2f, 5f));
    }

    // POP UP ANIMATIONS
    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0f;
            float timer = 0f;
            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20f));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvasGroup, float duration)
    {
        if (duration > 0f)
        {
            canvasGroup.alpha = 0f;
            float timer = 0f;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvasGroup.alpha = 1f;
        yield return null;
    }

    private IEnumerator WaitFadeOutPopUpOverTime(CanvasGroup canvasGroup, float duration, float delay)
    {
        if (duration > 0f)
        {
            while (delay > 0f)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
            float timer = 0f;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvasGroup.alpha = 0f;
        yield return null;
    }
}
