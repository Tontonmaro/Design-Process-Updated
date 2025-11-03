using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image loadingPanel;
    public Image loadingBar;
    public TextMeshProUGUI loadingText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut(0.5f, loadingPanel));
        StartCoroutine(FadeOut(0.5f, loadingBar));
        StartCoroutine(FadeOut(0.5f, loadingText));
    }

    IEnumerator FadeOut(float duration, Image targetImage)
    {
        float timer = 0f;
        Color startColor = targetImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            targetImage.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null; // Wait for the next frame
        }
        targetImage.color = targetColor; // Ensure it's fully opaque at the end
    }

    IEnumerator FadeOut(float duration, TextMeshProUGUI targetText)
    {
        float timer = 0f;
        Color startColor = targetText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            targetText.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null; // Wait for the next frame
        }
        targetText.color = targetColor; // Ensure it's fully opaque at the end
    }
}
