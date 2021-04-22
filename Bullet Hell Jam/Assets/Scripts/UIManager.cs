using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI hpValue;
    [SerializeField]
    private Image healthbarImage;
    [SerializeField]
    private List<GameObject> energyIcons;
    [SerializeField]
    private TextMeshProUGUI countdownValue;
    [SerializeField]
    private Image countdownImage;

    [SerializeField]
    private GameManager gm;

    private void OnEnable()
    {
        PlayerController.OnPlayerHealthChange += UpdateHpText;
        PlayerController.OnPlayerEnergyChange += UpdateEnergyIcons;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHealthChange -= UpdateHpText;
        PlayerController.OnPlayerEnergyChange -= UpdateEnergyIcons;
    }

    private void Update()
    {
        UpdateCountdownText();
    }

    private void UpdateHpText(int hp)
    {
        //hpValue.text = hp.ToString();
        healthbarImage.fillAmount = (float)hp / 9;
    }

    private void UpdateEnergyIcons(int energy)
    {
        for (int i=0; i<energyIcons.Count; i++)
        {
            bool shouldBeActive = false;

            if (i < energy)
                shouldBeActive = true;

            energyIcons[i].SetActive(shouldBeActive);
        }
    }

    private void UpdateCountdownText()
    {
        countdownValue.text = gm.Countdown.ToString();
        countdownImage.fillAmount = (float)gm.Countdown / (float)10;
    }

}
