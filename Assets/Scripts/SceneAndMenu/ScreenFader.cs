using AxGrid;
using AxGrid.Base;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Image))]

public class ScreenFader : MonoBehaviourExt
{
    public Image fadeImage; // assign fullscreen Image (black)
    public float defaultDuration = 1f;
    public float duration = 1f;


    [OnStart]
    void TheAwake()
    {
        fadeImage = gameObject.GetComponent<Image>();

        Model.EventManager.AddAction<string>("FadeIn", FadeIn);
        Model.EventManager.AddAction<string>("FadeOut", FadeOut);

        if (fadeImage == null)
            Debug.LogError("Assign fadeImage (fullscreen UI Image).");
    }

    public void FadeIn(string onComplete) // Make sure the image is enabled BEFORE COROUTINE START and raycast blocks if you want to block input during fade
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(1f, 0f, duration, onComplete));
    }

    public void FadeOut(string onComplete)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0f, 1f, duration, onComplete));
    }

    IEnumerator FadeRoutine(float fromAlpha, float toAlpha, float duration, string onComplete)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = fromAlpha;
        fadeImage.color = c;        

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            c.a = Mathf.Lerp(fromAlpha, toAlpha, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = toAlpha;
        fadeImage.color = c;

        if (!String.IsNullOrEmpty(onComplete))
        {
            //Debug.Log(onComplete);
            Model.EventManager.Invoke(onComplete);
        }

        yield return new WaitForFixedUpdate();
        if (toAlpha <= 0f) fadeImage.gameObject.SetActive(false);
    }

    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction<string>("FadeIn", FadeIn);
        Model.EventManager.RemoveAction<string>("FadeOut", FadeOut);
    }

}