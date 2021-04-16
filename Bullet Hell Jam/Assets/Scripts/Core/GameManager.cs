using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action OnTenSecondsPassed;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnComboChanged;

    private WaitForSeconds tenSeconds = new WaitForSeconds(10f);

    [Header("UI")]
    [SerializeField] private float comboResetDelay;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float comboResetTime;
    private int score;
    private int combo;
    
    [Header("Slowmo")]
    [SerializeField] private float slowmoScale;
    [SerializeField] private float slowmoDelay;
    [SerializeField] private float slowmoMaxTime;
    [SerializeField] private Image slowlmoCooldownImage;
    [SerializeField] private Image slowmoMaxTimeImage;

    private float slowmoCooldown;
    private float slowmoStartTime;
    private float slowmoStopTime;
    private bool isSlowmoActive;
    
    private InputController input;

    private void Awake()
    {
        input = GetComponent<InputController>();
        StartCoroutine(TenSecondTimer());

        slowmoMaxTimeImage.enabled = false;
    }

    private void OnEnable()
    {
        EnemyController.OnEnemyDeathScore += AddPoints;
        EnemyController.OnEnemyDeath += AddCombo;

        combo = 0;
        score = 0;

        scoreText.text = score.ToString("000000");
        comboText.text = "";
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyDeathScore -= AddPoints;
        EnemyController.OnEnemyDeath -= AddCombo;
    }

    private void Update()
    {
        HandleSlowmo();

        if (Time.time >= comboResetTime)
            ResetCombo();
    }

    private void AddPoints(int points)
    {
        score += points * combo;
        OnScoreChanged?.Invoke(points);
        scoreText.text = score.ToString("000000");
    }

    private void AddCombo()
    {
        combo++;
        OnComboChanged?.Invoke(combo);
        comboResetTime = Time.time + comboResetDelay;
        comboText.text = $"x{combo}";
    }

    private void ResetCombo()
    {
        combo = 0;
        OnComboChanged?.Invoke(combo);
        comboText.text = "";
    }

    private void HandleSlowmo()
    {
        if (!isSlowmoActive)
        {
            slowlmoCooldownImage.fillAmount = (Time.time - slowmoStopTime) / (slowmoCooldown - slowmoStopTime);
            
            if (input.keyInput.slowmoPress && Time.time >= slowmoCooldown)
                StartSlowmo();
                
        }
        else
        {
            slowmoMaxTimeImage.fillAmount = 1 - ((Time.time - slowmoStartTime) / slowmoMaxTime);

            if (input.keyInput.slowmoRelease || Time.time >= slowmoStartTime + slowmoMaxTime)
                StopSlowmo();
        }
        

    }

    private void StartSlowmo()
    {
        Time.timeScale = slowmoScale;
        slowmoStartTime = Time.time;
        isSlowmoActive = true;
        slowmoMaxTimeImage.enabled = true;
        slowlmoCooldownImage.enabled = false;
    }

    private void StopSlowmo()
    {
        Time.timeScale = 1f;
        slowmoCooldown = Time.time + slowmoDelay;
        isSlowmoActive = false;
        slowmoStopTime = Time.time;
        slowmoMaxTimeImage.enabled = false;
        slowlmoCooldownImage.enabled = true;
    }

    IEnumerator TenSecondTimer()
    {
        yield return tenSeconds;

        OnTenSecondsPassed?.Invoke();

        StartCoroutine(TenSecondTimer());
    }
}
