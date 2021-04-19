using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private GameManager gm;

    private string currentSceneName;

    private int maxCombo;
    private int score;

    private bool newHiScore;
    private bool newMaxCombo;

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

        if (scene.name == "PostLevelStatsScene")
        {
            FindObjectOfType<LevelStatsDisplay>().PopulateData(currentSceneName, score, maxCombo, newHiScore, newMaxCombo);
        }
        else
        {
            currentSceneName = scene.name;
        }
    }


    private void BossBattleEnded()
    {
        SetLevelStats();
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
