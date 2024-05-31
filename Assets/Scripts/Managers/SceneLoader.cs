using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] float backgroundFadeTime = 1.0f;

    public delegate void OnSceneLoad();
    public OnSceneLoad onSceneLoad;

    public int currentSceneIndex { get; private set; }
    public int previousSceneIndex { get; private set; }

    private void Awake()
    {
        if (SceneManager.sceneCount == 1 && SceneManager.GetSceneAt(0).buildIndex == 0)
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }

    public void SetScene(int sceneIndex)
    {
        onSceneLoad?.Invoke();
        if (SceneManager.sceneCount < 1)
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            previousSceneIndex = currentSceneIndex;
            currentSceneIndex = sceneIndex;
            return;
        }

        StartCoroutine(LoadScene(sceneIndex));
    }

    public void LoadPreviousScene()
    {
        SetScene(previousSceneIndex);
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        // Show background
        yield return StartCoroutine(FadeBackground(true));

        // Unload other scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            int scene = SceneManager.GetSceneAt(i).buildIndex;
            if (scene != 0)
                SceneManager.UnloadSceneAsync(scene);
        }

        // Load scene
        yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        if (sceneIndex == 2) GameManager.Instance.BattleStart();

        // Hide background
        yield return StartCoroutine(FadeBackground(false));
    }

    IEnumerator FadeBackground(bool fadeIn = false)
    {
        background.gameObject.SetActive(true);
        float startTime = Time.unscaledTime;
        background.color = new Color(background.color.r, background.color.g, background.color.b, fadeIn ? 0f : 1f);
        while (Time.unscaledTime < startTime + backgroundFadeTime)
        {
            float t = (Time.unscaledTime - startTime) / backgroundFadeTime;
            Color color = background.color;
            color.a = fadeIn ? t : 1f - t;
            background.color = color;
            yield return null;
        }
        background.color = new Color(background.color.r, background.color.g, background.color.b, fadeIn ? 1f : 0f);
        if (!fadeIn) background.gameObject.SetActive(false);
    }
}
