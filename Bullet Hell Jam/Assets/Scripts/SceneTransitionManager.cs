using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private GameManager gm;

    private Scene currentScene;

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
            FindObjectOfType<LevelStatsDisplay>().PopulateData(currentScene, score, maxCombo, newHiScore, newMaxCombo);
        }
        else
        {
            currentScene = scene;
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

        if (!PlayerPrefs.HasKey(currentScene.name + "HiScore") || score > PlayerPrefs.GetInt(currentScene.name + "HiScore"))
        {
            newHiScore = true;
            PlayerPrefs.SetInt(currentScene.name + "HiScore", score);
        }

        if (!PlayerPrefs.HasKey(currentScene.name + "MaxCombo") || maxCombo > PlayerPrefs.GetInt(currentScene.name + "MaxCombo"))
        {
            newMaxCombo = true;
            PlayerPrefs.SetInt(currentScene.name + "MaxCombo", maxCombo);
        }
    }
}
