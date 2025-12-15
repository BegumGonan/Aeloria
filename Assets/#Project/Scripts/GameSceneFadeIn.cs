using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameSceneFadeIn : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1f;

    private void Start()
    {
        SetAlpha(1f);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
    }

    void SetAlpha(float a)
    {
        Color c = fadePanel.color;
        c.a = a;
        fadePanel.color = c;
    }
}