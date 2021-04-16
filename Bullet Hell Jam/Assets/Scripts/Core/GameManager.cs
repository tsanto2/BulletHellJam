using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnTenSecondsPassed;
    private WaitForSeconds tenSeconds = new WaitForSeconds(10f);

    [SerializeField] private float slowmoScale;
    
    private InputController input;

    private void Awake()
    {
        input = GetComponent<InputController>();
        StartCoroutine(TenSecondTimer());
    }

    private void Update()
    {
        if (input.keyInput.slowmoPress)
            Time.timeScale = slowmoScale;
        else if (input.keyInput.slowmoRelease)
            Time.timeScale = 1f;
    }

    IEnumerator TenSecondTimer()
    {
        yield return tenSeconds;

        OnTenSecondsPassed?.Invoke();

        StartCoroutine(TenSecondTimer());
    }
}
