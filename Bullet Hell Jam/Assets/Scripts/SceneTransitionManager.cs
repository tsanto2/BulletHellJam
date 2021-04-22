using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private GameManager gm;
    private InputController ic;
    private UIController uic;

    private SceneTransitionFader fader;

    private string currentSceneName;

    private int maxCombo;
    private int score;

    private bool newHiScore;
    private bool newMaxCombo;

    private bool isStatsScene;

    private bool transitioningToNextScene = false;

    private int nextScene = 1;

    [SerializeField]
    private string statsSceneName = "PostLevelStatsScene";
    [SerializeField]
    private string gameplaySceneBaseName = "Level";
    private void OnEnable()
    {
        BossController.OnBossDeath += BossBattleEnded;
        PlayerController.OnPlayerDied += PlayerDied;
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        BossController.OnBossDeath -= BossBattleEnded;
        PlayerController.OnPlayerDied -= PlayerDied;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isStatsScene)
            CheckForNextSceneButtonPressed();
    }

    void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (scene.name == "MainMenu")
        {
            Destroy(this);
        }

        gm = FindObjectOfType<GameManager>();
        ic = FindObjectOfType<InputController>();
        fader = FindObjectOfType<SceneTransitionFader>();
        uic = FindObjectOfType<UIController>();

        if (scene.name == "PostLevelStatsScene")
        {
            InitStatsScreen();
        }
        else
        {
            isStatsScene = false;

            currentSceneName = scene.name;

            StartCoroutine(TimedSceneTransition(0.5f, false));
        }
    }

    private void InitStatsScreen()
    {
        StartCoroutine(TimedSceneTransition(1f, false));

        isStatsScene = true;
        transitioningToNextScene = false;

        nextScene++;

        FindObjectOfType<LevelStatsDisplay>().PopulateData(currentSceneName, score, maxCombo, newHiScore, newMaxCombo);
    }

    private void PlayerDied()
    {
        StartCoroutine(TimedSceneTransition(1f, true, "DeathScreen", true));
    }

    private void CheckForNextSceneButtonPressed()
    {
        if (ic == null)
            return;

        bool nextSceneButtonPressed = Input.anyKeyDown;

        if (nextSceneButtonPressed && isStatsScene && !transitioningToNextScene)
        {
            string nextGameplaySceneName = gameplaySceneBaseName + nextScene.ToString();

            if (nextScene < 3)
                StartCoroutine(TimedSceneTransition(1f, true, nextGameplaySceneName));
            else
                StartCoroutine(TimedSceneTransition(1f, true, "MainMenu"));
        }
    }

    private void BossBattleEnded()
    {
        SetLevelStats();

        DisableUI();

        StartCoroutine(TimedSceneTransition(4f, true, statsSceneName));
    }

    private void TransitionToScene(string sceneName)
    {
        StopAllCoroutines();

        SceneManager.LoadScene(sceneName);
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

    private void DisableUI()
    {
        foreach(Transform child in uic.transform)
        {
            if (child.GetComponent<SceneTransitionFader>() == null)
                child.gameObject.SetActive(false);
        }
    }

    IEnumerator TimedSceneTransition(float delay, bool increaseAlpha, string sceneName=null, bool playerDied = false)
    {
        transitioningToNextScene = true;

        Image faderImage = fader.FaderImage;

        if (playerDied)
            faderImage.color = new Color(Color.black.r, Color.black.g, Color.black.b, faderImage.color.a);

        float elapsedTime = 0;
        float startValue = faderImage.color.a;
        float endValue = 1;

        if (!increaseAlpha)
            endValue = 0;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / delay);
            faderImage.color = new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, newAlpha);
            yield return null;
        }

        if (increaseAlpha)
            TransitionToScene(sceneName);
    }
}
