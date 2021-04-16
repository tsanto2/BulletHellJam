using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float slowmoScale;
    
    private InputController input;

    void Update()
    {
        if (input.keyInput.slowmoPress)
            Time.timeScale = slowmoScale;
        else if (input.keyInput.slowmoRelease)
            Time.timeScale = 1f;
    }
}
