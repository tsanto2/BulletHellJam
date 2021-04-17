using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action OnTenSecondsPassed;
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

    [SerializeField] private float pauseTime = 0.03f;
    [SerializeField] private float minPitch = 0.3f;
    
    [Header("Boss")]
    [SerializeField] private float bossSpawnDelay;
    [SerializeField] private GameObject bossGameObject;
    private int enemyCount;

    private InputController input;
    private Camera cam;
    private Coroutine pauseCoroutine;

    private void Awake()
    {
        input = GetComponent<InputController>();
        StartCoroutine(TenSecondTimer());

        if (slowmoMaxTimeImage != null)
            slowmoMaxTimeImage.enabled = false;
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
        HandleSlowmo();

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

    private void HandleSlowmo()
    {
        if (!isSlowmoActive)
        {
            //slowlmoCooldownImage.fillAmount = (Time.time - slowmoStopTime) / (slowmoCooldown - slowmoStopTime);
            
            if (input.keyInput.slowmoPress /*&& Time.time >= slowmoCooldown*/)
            {
                if (pauseCoroutine != null)
                    StopCoroutine(pauseCoroutine);

                pauseCoroutine = StartCoroutine(Pause(0f, pauseTime));
                isSlowmoActive = true;
            }
                //StartSlowmo();
                
        }
        else
        {
            //slowmoMaxTimeImage.fillAmount = 1 - ((Time.time - slowmoStartTime) / slowmoMaxTime);

            if (input.keyInput.slowmoRelease /*|| Time.time >= slowmoStartTime + slowmoMaxTime*/)
            {
                if (pauseCoroutine != null)
                    StopCoroutine(pauseCoroutine);

                pauseCoroutine = StartCoroutine(Pause(1f, pauseTime));
                isSlowmoActive = false;
            }
                //StopSlowmo();
        }
    }

    private void StartSlowmo()
    {
        Time.timeScale = 0 /*slowmoScale*/;
        //slowmoStartTime = Time.time;
        isSlowmoActive = true;
        //slowmoMaxTimeImage.enabled = true;
        //slowlmoCooldownImage.enabled = false;
        OnSlowMoStarted?.Invoke();
    }

    IEnumerator Pause(float targetValue, float time)
    {
        while (Mathf.Abs(Time.timeScale - targetValue) > 0.1f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetValue, time);
            GetComponent<AudioSource>().pitch = minPitch + (Time.timeScale * (1f - minPitch));
            yield return null;
        }

        Time.timeScale = targetValue;
        GetComponent<AudioSource>().pitch = minPitch + (Time.timeScale * (1f - minPitch));
        pauseCoroutine = null;
    }

    private void StopSlowmo()
    {
        Time.timeScale = 1f;
        //slowmoCooldown = Time.time + slowmoDelay;
        isSlowmoActive = false;
        //slowmoStopTime = Time.time;
        //slowmoMaxTimeImage.enabled = false;
        //slowlmoCooldownImage.enabled = true;
        OnSlowMoEnded?.Invoke();
    }
    
    #endregion

    IEnumerator TenSecondTimer()
    {
        yield return tenSeconds;

        OnTenSecondsPassed?.Invoke();

        StartCoroutine(TenSecondTimer());
    }
}
