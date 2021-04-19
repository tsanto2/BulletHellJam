using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelStatsDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI newBestComboText;
    [SerializeField]
    private TextMeshProUGUI newBestScoreText;
    [SerializeField]
    private TextMeshProUGUI currentComboText;
    [SerializeField]
    private TextMeshProUGUI currentScoreText;
    [SerializeField]
    private TextMeshProUGUI bestComboText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    public void PopulateData(string previousSceneName, int score, int combo, bool newHiScore, bool newMaxCombo)
    {
        if (newHiScore)
        {
            newBestScoreText.enabled = true;
        }
        if (newMaxCombo)
        {
            newBestComboText.enabled = true;
        }

        currentScoreText.text = score.ToString();
        currentComboText.text = combo.ToString();

        Debug.Log(previousSceneName);

        bestScoreText.text = PlayerPrefs.GetInt(previousSceneName + "HiScore").ToString();
        bestComboText.text = PlayerPrefs.GetInt(previousSceneName + "MaxCombo").ToString();
    }

}
