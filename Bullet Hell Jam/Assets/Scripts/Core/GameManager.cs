using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action OnTenSecondsPassed;
    public static event Action OnStartedTenSecondTimer;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnComboChanged;
    public static event Action OnSlowMoStarted;
    public static event Action OnSlowMoEnded;

    private WaitForSeconds tenSeconds = new WaitForSeconds(10f);

    [Header("UI")]
    [SerializeField] private float comboResetDelay;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float comboResetTime;
    private int score;
    private int combo;
    
    [SerializeField] private float pauseTime = 0.03f;
    [SerializeField] private float minPitch = 0.3f;
    private Coroutine pauseCoroutine;
    
    [Header("Boss")]
    [SerializeField] private float bossSpawnDelay;
    [SerializeField] private GameObject bossGameObject;
    private int enemyCount;

    private InputController input;
    private Camera cam;

    private void Awake()
    {
        input = GetComponent<InputController>();
        OnStartedTenSecondTimer?.Invoke();
        StartCoroutine(TenSecondTimer());
    }

    private void Start()
    {
        foreach (var enemy in FindObjectsOfType<EnemyController>())
            enemyCount++;
    
        cam = Camera.main;
    }

    private void OnEnable()
    {
        EnemyController.OnEnemyDeathScore += AddPoints;
        EnemyController.OnEnemyDeath += AddCombo;
        EnemyController.OnEnemyDeath += DecreaseEnemyCount;

        combo = 0;
        score = 0;

        // Because I'm lazy, that's why
        scoreText.text = score.ToString("0000000");
        comboText.text = "";
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyDeathScore -= AddPoints;
        EnemyController.OnEnemyDeath -= AddCombo;
        EnemyController.OnEnemyDeath -= DecreaseEnemyCount;
    }

    private void Update()
    {
        HandlePause();

        if (Time.time >= comboResetTime)
            ResetCombo();

        if (input.keyInput.restartPress)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #region Enemy Methods

    private void DecreaseEnemyCount()
    {
        enemyCount--;
        
        if (enemyCount == 0 && bossGameObject != null)
            Invoke("SpawnBoss", bossSpawnDelay);
    }

    private void SpawnBoss()
    {
        Debug.Log("OH LAWD HE COMIN'");
        Instantiate(bossGameObject, new Vector3(cam.ViewportToWorldPoint(Vector3.right).x + 2f, 1f, 0f), Quaternion.identity);
    }

    #endregion

    private void AddPoints(int points)
    {
        score += points * combo;
        OnScoreChanged?.Invoke(points);
        scoreText.text = score.ToString("0000000");
    }

    #region Combo Methods

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

    #endregion

    #region Slowmo Methods

    private void HandlePause()
    {
        if (pauseCoroutine == null)
        {
            if (input.keyInput.slowmo && Time.timeScale == 1f)
            {
                OnSlowMoStarted?.Invoke();
                pauseCoroutine = StartCoroutine(Pause(0f, pauseTime));
            }

            if (!input.keyInput.slowmo && Time.timeScale == 0f)
            {
                OnSlowMoEnded?.Invoke();
                pauseCoroutine = StartCoroutine(Pause(1f, pauseTime));
            }
        }   

    }

    IEnumerator Pause(float targetValue, float time)
    {
        while (Mathf.Abs(Time.timeScale - targetValue) > 0.1f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetValue, time);
            yield return null;
        }

        Time.timeScale = targetValue;
        pauseCoroutine = null;
    }
    
    #endregion

    IEnumerator TenSecondTimer()
    {
        yield return tenSeconds;
        OnTenSecondsPassed?.Invoke();
        StartCoroutine(TenSecondTimer());
    }
}
