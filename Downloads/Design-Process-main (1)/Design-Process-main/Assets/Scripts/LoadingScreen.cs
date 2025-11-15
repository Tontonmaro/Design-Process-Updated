using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Image loadingScreen;
    public Image loadingBarFill;

    public TextMeshProUGUI loadingText;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        StartCoroutine(FadeIn(0.5f, loadingScreen));
        StartCoroutine(FadeIn(0.5f, loadingText));

        yield return new WaitForSeconds(1.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        while (!operation.isDone)
        {
            float progessValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarFill.fillAmount = progessValue;

            yield return null;
        }
    }

    IEnumerator FadeIn(float duration, Image targetImage)
    {
        float timer = 0f;
        Color startColor = targetImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            targetImage.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null; // Wait for the next frame
        }
        targetImage.color = targetColor; // Ensure it's fully opaque at the end
    }

    IEnumerator FadeIn(float duration, TextMeshProUGUI targetText)
    {
        float timer = 0f;
        Color startColor = targetText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            targetText.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null; // Wait for the next frame
        }
        targetText.color = targetColor; // Ensure it's fully opaque at the end
    }

}
