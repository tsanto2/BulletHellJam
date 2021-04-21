using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private GameManager gm;
    private InputController ic;

    private SceneTransitionFader fader;

    private string currentSceneName;

    private int maxCombo;
    private int score;

    private bool newHiScore;
    private bool newMaxCombo;

    private bool isStatsScene;

    private int nextScene = 1;
    private void OnEnable()
    {
        BossController.OnBossDeath += BossBattleEnded;

        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        BossController.OnBossDeath -= BossBattleEnded;

        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        gm = FindObjectOfType<GameManager>();
        ic = FindObjectOfType<InputController>();
        fader = FindObjectOfType<SceneTransitionFader>();

        if (scene.name == "PostLevelStatsScene")
        {
            StartCoroutine(StatSceneTransition(1f, false));

            isStatsScene = true;

            nextScene++;

            FindObjectOfType<LevelStatsDisplay>().PopulateData(currentSceneName, score, maxCombo, newHiScore, newMaxCombo);
        }
        else
        {
            isStatsScene = false;

            currentSceneName = scene.name;
        }
    }

    private void Update()
    {
        if (isStatsScene)
            CheckForNextSceneButtonPressed();
    }

    private void CheckForNextSceneButtonPressed()
    {
        if (ic == null)
            return;

        bool nextSceneButtonPressed = ic.keyInput.topFaceButtonPress;

        if (nextSceneButtonPressed && isStatsScene)
        {
            TransitionToNextGameplayLevel();
        }
    }

    private void TransitionToNextGameplayLevel()
    {
        SceneManager.LoadScene("Level" + nextScene.ToString());
    }

    private void BossBattleEnded()
    {
        SetLevelStats();
        StartCoroutine(StatSceneTransition(4f, true));
    }

    IEnumerator StatSceneTransition(float delay, bool isGameplayScene)
    {
        Image faderImage = fader.FaderImage;

        float elapsedTime = 0;
        float startValue = faderImage.color.a;
        float endValue = 1;

        if (!isGameplayScene)
            endValue = 0;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / delay);
            faderImage.color = new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, newAlpha);
            yield return null;
        }
        if (isGameplayScene)
            SceneManager.LoadScene("PostLevelStatsScene");
    }

    private void SetLevelStats()
    {
        score = gm.Score;
        maxCombo = gm.MaxCombo;

        if (!PlayerPrefs.HasKey(currentSceneName + "HiScore") || score > PlayerPrefs.GetInt(currentSceneName + "HiScore"))
        {
            newHiScore = true;
            PlayerPrefs.SetInt(currentSceneName + "HiScore", score);
        }

        if (!PlayerPrefs.HasKey(currentSceneName + "MaxCombo") || maxCombo > PlayerPrefs.GetInt(currentSceneName + "MaxCombo"))
        {
            newMaxCombo = true;
            PlayerPrefs.SetInt(currentSceneName + "MaxCombo", maxCombo);
        }
    }
}
