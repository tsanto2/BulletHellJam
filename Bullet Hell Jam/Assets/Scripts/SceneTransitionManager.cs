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

    private int nextScene = 1;

    [SerializeField]
    private string statsSceneName = "PostLevelStatsScene";
    [SerializeField]
    private string gameplaySceneBaseName = "Level";
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

    private void Update()
    {
        if (isStatsScene)
            CheckForNextSceneButtonPressed();
    }

    void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Scene loaded: " + scene.name);

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

        nextScene++;

        FindObjectOfType<LevelStatsDisplay>().PopulateData(currentSceneName, score, maxCombo, newHiScore, newMaxCombo);
    }

    private void CheckForNextSceneButtonPressed()
    {
        if (ic == null)
            return;

        bool nextSceneButtonPressed = ic.keyInput.topFaceButtonPress;

        if (nextSceneButtonPressed && isStatsScene)
        {
            string nextGameplaySceneName = gameplaySceneBaseName + nextScene.ToString();

            StartCoroutine(TimedSceneTransition(1f, true, nextGameplaySceneName));
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

    IEnumerator TimedSceneTransition(float delay, bool increaseAlpha, string sceneName=null)
    {
        Image faderImage = fader.FaderImage;

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
