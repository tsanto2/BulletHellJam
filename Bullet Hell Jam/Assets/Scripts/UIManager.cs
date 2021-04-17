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

    [SerializeField]
    private GameManager gm;

    private void OnEnable()
    {
        PlayerController.OnPlayerHealthChange += UpdateHpText;
        PlayerController.OnPlayerEnergyChange += UpdateEnergyText;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHealthChange -= UpdateHpText;
        PlayerController.OnPlayerEnergyChange -= UpdateEnergyText;
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
        countdownValue.text = gm.Countdown.ToString();
    }

}
