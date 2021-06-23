using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    static bool _isFirstLogin = false;

    [SerializeField]
    Slider ProgressSlider;

    [SerializeField]
    TextMeshProUGUI ProgressPercent;

    [SerializeField]
    TextMeshProUGUI ProgressText;

    public static void LoadScene(string sceneName, bool isFirstLogin = false)
    {
        _isFirstLogin = isFirstLogin;
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess(_isFirstLogin));
    }

    IEnumerator LoadSceneProcess(bool isFirstLogin)
    {
        if (isFirstLogin)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            float timer = 0f;
            while (!op.isDone)
            {
                yield return null;

                if (op.progress < 0.9f)
                {
                    ProgressSlider.value = op.progress;
                    ProgressPercent.text = String.Format("{0:P0}", ProgressSlider.value);
                }
                else
                {
                    StringManager.Instance.Init();
                    TableManager.Instance.Init();
                    if (!StringManager.Instance.isSuccessInit) yield return null;
                    else if(!TableManager.Instance.isSuccessInit) yield return null;
                    else
                    {
                        timer += Time.unscaledDeltaTime;
                        ProgressSlider.value = Mathf.Lerp(0.9f, 1f, timer);
                        ProgressPercent.text = String.Format("{0:P0}", ProgressSlider.value);
                        if (ProgressSlider.value >= 1f)
                        {
                            op.allowSceneActivation = true;
                            yield break;
                        }
                    }
                }
            }
        }
        else
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            float timer = 0f;
            while (!op.isDone)
            {
                yield return null;

                if (op.progress < 0.9f)
                {
                    ProgressSlider.value = op.progress;
                    ProgressPercent.text = String.Format("{0:P0}", ProgressSlider.value);
                }
                else
                {
                    timer += Time.unscaledDeltaTime;
                    ProgressSlider.value = Mathf.Lerp(0.9f, 1f, timer);
                    ProgressPercent.text = String.Format("{0:P0}", ProgressSlider.value);
                    if (ProgressSlider.value >= 1f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }

                }
            }
        }
    }
}
