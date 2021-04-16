using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event Action OnTenSecondsPassed;
    public static event Action<int> OnScoreChanged;

    [Header("Slowmo")]
    [SerializeField] private float slowmoScale;
    [SerializeField] private float slowmoDelay;
    [SerializeField] private float slowmoMaxTime;
    [SerializeField] private Image slowlmoImage;
    private float slowmoCooldown;
    private float slowmoStartTime;
    private float slowmoStopTime;
    private bool isSlowmoActive;

    private WaitForSeconds tenSeconds = new WaitForSeconds(10f);
    
    private InputController input;

    private void Awake()
    {
        input = GetComponent<InputController>();
        StartCoroutine(TenSecondTimer());
    }

    private void Update()
    {
        HandleSlowmo();
    }

    private void HandleSlowmo()
    {
        if (input.keyInput.slowmoPress && Time.time >= slowmoCooldown)
            StartSlowmo();

        if (!isSlowmoActive)
        {
            slowlmoImage.fillAmount = (Time.time - slowmoStopTime) / (slowmoCooldown - slowmoStopTime);
            return;
        }
        
        if (input.keyInput.slowmoRelease || Time.time >= slowmoStartTime + slowmoMaxTime)
            StopSlowmo();
    }

    private void StartSlowmo()
    {
        Time.timeScale = slowmoScale;
        slowmoStartTime = Time.time;
        isSlowmoActive = true;
        Debug.Log("Started slowmo at: " + Time.time);
    }

    private void StopSlowmo()
    {
        Time.timeScale = 1f;
        slowmoCooldown = Time.time + slowmoDelay;
        isSlowmoActive = false;
        slowmoStopTime = Time.time;
        Debug.Log("Stopped slowmo at: " + Time.time);
    }

    IEnumerator TenSecondTimer()
    {
        yield return tenSeconds;

        OnTenSecondsPassed?.Invoke();

        StartCoroutine(TenSecondTimer());
    }
}
