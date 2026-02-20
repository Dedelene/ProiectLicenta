using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    public Image fadeImage;
    public float fadeSpeed = 2f;
    private bool isFading = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }
    public void FadeToBlackOnly()
    {
        if (!isFading)
            StartCoroutine(FadeToBlackRoutine());
    }

    IEnumerator FadeToBlackRoutine()
    {
        isFading = true;

        float alpha = fadeImage.color.a;

        while(alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
        isFading = false;
    }

    public void FadeIn()
    {
        if (!isFading)
            StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        isFading = true;
        float alpha = fadeImage.color.a;
        while(alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        isFading = false;
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        isFading = true;
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        isFading = false;
        FadeIn();
    }
}
