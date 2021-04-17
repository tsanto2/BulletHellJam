using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI hpValue;
    [SerializeField]
    private TextMeshProUGUI energyValue;
    [SerializeField]
    private TextMeshProUGUI countdownValue;

    private int countdown = 10;

    private void OnEnable()
    {
        PlayerController.OnPlayerHealthChange += UpdateHpText;
        PlayerController.OnPlayerEnergyChange += UpdateEnergyText;
        GameManager.OnTenSecondsPassed += ResetCountdown;
        GameManager.OnStartedTenSecondTimer += StartCountdown;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHealthChange -= UpdateHpText;
        PlayerController.OnPlayerEnergyChange -= UpdateEnergyText;
        GameManager.OnTenSecondsPassed -= ResetCountdown;
        GameManager.OnStartedTenSecondTimer -= StartCountdown;
    }

    private void Update()
    {
        UpdateCountdownText();
    }

    private void UpdateHpText(int hp)
    {
        hpValue.text = hp.ToString();
    }

    private void UpdateEnergyText(int energy)
    {
        energyValue.text = energy.ToString();
    }

    private void UpdateCountdownText()
    {
        countdownValue.text = countdown.ToString();
    }

    private void ResetCountdown()
    {
        countdown = 10;
    }

    private void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);

        countdown--;

        // HAAAAAAAAAAAAAAAAAAAAAAAAAAAACK
        if (countdown == 0)
            countdown = 10;

        StartCoroutine(Countdown());
    }

}
