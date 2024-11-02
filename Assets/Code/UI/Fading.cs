using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{
    [SerializeField] private Texture2D fadeOutTexture;
    public float fadeDuration = 1f;

    private enum FadeDirection { In, Out, None };
    private FadeDirection fadeDirection = FadeDirection.None;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private float fadeStartTime;

    private void Start()
    {
        StartFadeIn();
    }

    private void OnGUI()
    {
        if (fadeDirection == FadeDirection.None)
        {
            return;
        }

        UpdateFade();
        DrawFadeEffect();
    }

    private float GetFadeStartValue() => fadeDirection == FadeDirection.Out ? 0.0f : 1.0f;
    private float GetFadeEndValue() => fadeDirection == FadeDirection.Out ? 1.0f : 0.0f;

    private void UpdateFade()
    {
        float fadeElapsed = Time.unscaledTime - fadeStartTime;
        alpha = Mathf.Lerp(GetFadeStartValue(), GetFadeEndValue(), fadeElapsed / fadeDuration);
        alpha = Mathf.Clamp01(alpha);

        if (fadeElapsed >= fadeDuration + 0.1f)
            fadeDirection = FadeDirection.None;
    }

    private void DrawFadeEffect()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public void StartFadeOut()
    {
        fadeDirection = FadeDirection.Out;
        fadeStartTime = Time.unscaledTime;
        StartCoroutine(FadeSound(1f, 0f));
    }

    public void StartFadeIn()
    {
        fadeDirection = FadeDirection.In;
        fadeStartTime = Time.unscaledTime;
        StartCoroutine(FadeSound(0f, 1f));
    }

    IEnumerator FadeSound(float audioStartValue, float audioEndValue)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            AudioListener.volume = Mathf.Lerp(audioStartValue, audioEndValue, elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
