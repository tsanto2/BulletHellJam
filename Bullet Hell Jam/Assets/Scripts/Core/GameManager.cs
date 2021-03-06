using System;
using System.Collections;
using UnityEngine;
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
    private WaitForSeconds oneSecond = new WaitForSeconds(1f);

    [Header("UI")]
    [SerializeField] private float comboResetDelay;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float comboResetTime;

    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
            scoreText.text = score.ToString("0000000");
        }
    }
    private int combo;

    private int maxCombo;
    public int MaxCombo
    {
        get
        {
            return maxCombo;
        }
        set
        {
            maxCombo = value;
        }
    }

    [SerializeField] private float pauseTime = 0.03f;
    [SerializeField] private float minPitch = 0.3f;
    [SerializeField] private Sound tickSound;
    private Coroutine pauseCoroutine;
    
    [Header("Boss")]
    [SerializeField] private float bossSpawnDelay;
    [SerializeField] private GameObject bossGameObject;
    private int enemyCount;

    private int countdown = 10;
    public int Countdown { get { return countdown;  } }
    private bool stopCountdownSound;

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
        cam = Camera.main;

        foreach (var enemy in FindObjectsOfType<EnemyController>())
            enemyCount++;

        if (enemyCount == 0)
            SpawnBoss();
    }

    private void OnEnable()
    {
        EnemyController.OnEnemyDeathScore += AddPoints;
        EnemyController.OnEnemyDeath += AddCombo;
        EnemyController.OnEnemyDeath += DecreaseEnemyCount;
        EnemyMovement.OnEnemyDespawnOffscreen += DecreaseEnemyCount;
        PlayerController.OnPlayerTakeDamage += ResetCombo;
        BossController.OnBossDeath += DisableCountdownSound;

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
        EnemyMovement.OnEnemyDespawnOffscreen -= DecreaseEnemyCount;
        PlayerController.OnPlayerTakeDamage -= ResetCombo;
        BossController.OnBossDeath -= DisableCountdownSound;
    }

    private void Update()
    {
        HandlePause();

        if (Time.time >= comboResetTime && enemyCount > 0)
            ResetCombo();

        if (input.keyInput.restartPress)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #region Enemy Methods

    private void DecreaseEnemyCount(int dimmadome)
    {
        enemyCount--;
        
        if (enemyCount == 0 && bossGameObject != null)
            Invoke("SpawnBoss", bossSpawnDelay);
    }

    private void SpawnBoss()
    {
        Debug.Log("OH LAWD HE COMIN'");
        Instantiate(bossGameObject, new Vector3(cam.ViewportToWorldPoint(Vector3.right).x + 2f, 0f, 0f), Quaternion.identity);
    }

    #endregion

    public void AddPoints(int points)
    {
        Score += points * combo;
    }

    #region Combo Methods

    public void AddCombo(int increase)
    {
        combo += increase;
        OnComboChanged?.Invoke(combo);
        comboText.text = $"x{combo}";

        if (combo > maxCombo)
        {
            maxCombo = combo;
        }

        if (enemyCount > 0)
            comboResetTime = Time.time + comboResetDelay;
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
            Time.timeScale = Mathf.SmoothStep(Time.timeScale, targetValue, time);
            AudioManager.UpdateBGMLowPassFilter();
            yield return null;
        }

        Time.timeScale = targetValue;
        AudioManager.UpdateBGMLowPassFilter();
        pauseCoroutine = null;
    }
    
    #endregion

    private void DisableCountdownSound() => stopCountdownSound = true;

    IEnumerator TenSecondTimer()
    {
        yield return oneSecond;

        countdown--;

        if (countdown <= 3 && !stopCountdownSound)
        {
            if (countdown == 0)
            {
                countdown = 10;
                OnTenSecondsPassed?.Invoke();
            }
            else 
                AudioManager.PlaySFX(tickSound);
        }

        StartCoroutine(TenSecondTimer());
    }
}
