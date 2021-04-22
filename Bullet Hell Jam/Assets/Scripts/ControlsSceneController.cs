using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsSceneController : MonoBehaviour
{

    private InputController ic;

    [SerializeField]
    private bool isMainMenuControlsScene = false;

    [SerializeField]
    private float countdownDuration = 3.0f;

    [SerializeField]
    private GameObject continuePrompt;

    private bool hasCountdownFinished = false;

    private void OnEnable()
    {
        ic = FindObjectOfType<InputController>();
    }

    private void Start()
    {
        StartCoroutine(ControlsSceneCountdown());
    }

    private void Update()
    {
        if (ic.keyInput.topFaceButtonPress && hasCountdownFinished)
        {
            if (isMainMenuControlsScene)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("MatTest");
            }
        }
    }

    IEnumerator ControlsSceneCountdown()
    {
        yield return new WaitForSeconds(countdownDuration);

        continuePrompt.SetActive(true);

        hasCountdownFinished = true;
    }

}
