using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public Image backgroundImage1;
    public Image backgroundImage2;
    public float changeInterval = 5f;
    public float fadeDuration = 1.5f;
    public Image fadePanel; 
    public float transitionTime = 1f;
    public string gameSceneName = "GameScene"; 
    private Image currentVisibleImage;
    private Image nextImageToFadeIn;
    public AudioSource menuMusic;
    public float musicFadeOutDuration = 1f;

    private void Start()
    {
        SetImageAlpha(fadePanel, 0f);
        
        SetImageAlpha(backgroundImage1, 1f);
        SetImageAlpha(backgroundImage2, 0f);
        currentVisibleImage = backgroundImage1;

        StartCoroutine(FadeInScreen());
        
        StartCoroutine(BackgroundCycleCoroutine());
    }

    public void StartGame()
    {
        StopAllCoroutines();

        if (menuMusic != null && menuMusic.isPlaying)
        {
            StartCoroutine(FadeOutMusic(menuMusic, musicFadeOutDuration));
        }

        StartCoroutine(FadeOutAndLoadScene(gameSceneName));
    }

    IEnumerator FadeOutMusic(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        source.Stop();
    }

    IEnumerator BackgroundCycleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);

            nextImageToFadeIn = (currentVisibleImage == backgroundImage1) ? backgroundImage2 : backgroundImage1;
            
            yield return StartCoroutine(ImageAlphaFadeTransition(currentVisibleImage, nextImageToFadeIn, fadeDuration));

            currentVisibleImage = nextImageToFadeIn;
        }
    }
    
    IEnumerator ImageAlphaFadeTransition(Image fadeOutImage, Image fadeInImage, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float ratio = timer / duration;

            SetImageAlpha(fadeOutImage, 1f - ratio);

            SetImageAlpha(fadeInImage, ratio);

            yield return null; 
        }

        SetImageAlpha(fadeOutImage, 0f);
        SetImageAlpha(fadeInImage, 1f);
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(FadePanelAlphaTransition(1f, transitionTime));

        SceneManager.LoadScene(sceneName);
    }
    
    IEnumerator FadeInScreen()
    {
        yield return StartCoroutine(FadePanelAlphaTransition(0f, transitionTime)); 
    }

    IEnumerator FadePanelAlphaTransition(float targetAlpha, float duration)
    {
        float startAlpha = fadePanel.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            SetImageAlpha(fadePanel, newAlpha);
            yield return null;
        }
        
        SetImageAlpha(fadePanel, targetAlpha);
    }
    
    void SetImageAlpha(Image img, float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}