using UnityEngine;

public class InputController : MonoBehaviour
{
    public KeyInput keyInput;

    private bool RTActive = false;

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        keyInput.moveVec = new Vector3(moveX, moveY, 0f);

        keyInput.crawlPress = Input.GetKey(KeyCode.LeftShift) || (Input.GetAxisRaw("Crawl") > 0);

        keyInput.slowmo = Input.GetButton("SlowMo");
        keyInput.shootPress = Input.GetAxisRaw("Shoot") > 0 || Input.GetKey(KeyCode.Space);

        keyInput.leftFaceButtonPress = Input.GetButtonDown("LeftFaceButton");
        keyInput.rightFaceButtonPress = Input.GetButtonDown("RightFaceButton");
        keyInput.topFaceButtonPress = Input.GetButtonDown("TopFaceButton");
        keyInput.bottomFaceButtonPress = Input.GetButtonDown("BottomFaceButton");

        keyInput.restartPress = Input.GetKeyDown(KeyCode.F12); 
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