using UnityEngine;

public class InputController : MonoBehaviour
{
    public KeyInput keyInput;
    private bool controlsEnabled = true;

    private void OnEnable()
    {
        controlsEnabled = true;
        BossController.OnBossDeath += DisableControls;
        PlayerController.OnPlayerDied += DisableControls;
    }

    private void OnDisable()
    {
        BossController.OnBossDeath -= DisableControls;
        PlayerController.OnPlayerDied -= DisableControls;
    }

    private void Update()
    {
        if (!controlsEnabled)
            return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        keyInput.moveVec = new Vector3(moveX, moveY, 0f);

        keyInput.crawlPress = (Input.GetButton("Crawl") || Input.GetAxisRaw("Crawl") > 0f);
        keyInput.shootPress = (Input.GetButton("Shoot") || Input.GetAxisRaw("Shoot") > 0f);
        keyInput.slowmo = Input.GetButton("SlowMo");

        keyInput.leftFaceButtonPress = Input.GetButtonDown("LeftFaceButton");
        keyInput.rightFaceButtonPress = Input.GetButtonDown("RightFaceButton");
        keyInput.topFaceButtonPress = Input.GetButtonDown("TopFaceButton");
        keyInput.bottomFaceButtonPress = Input.GetButtonDown("BottomFaceButton");

        keyInput.restartPress = Input.GetKeyDown(KeyCode.F12); 
    }

    private void DisableControls()
    {
        controlsEnabled = false;

        keyInput.moveVec = Vector3.zero;

        keyInput.crawlPress = false;
        keyInput.shootPress = false;
        keyInput.slowmo = false;

        keyInput.leftFaceButtonPress = false;
        keyInput.rightFaceButtonPress = false;
        keyInput.topFaceButtonPress = false;
        keyInput.bottomFaceButtonPress = false;

        keyInput.restartPress = false;
    }
}

public struct KeyInput
{
    public Vector3 moveVec;
    public bool shootPress;
    public bool crawlPress;

    public bool slowmo;
    public bool slowmoPress;
    public bool slowmoRelease;

    public bool leftFaceButtonPress;
    public bool rightFaceButtonPress;
    public bool topFaceButtonPress;
    public bool bottomFaceButtonPress;

    // Just a temporary button for debugging
    public bool restartPress;
}