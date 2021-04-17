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

        keyInput.shootPress = Input.GetButton("Shoot");

        if (Input.GetAxisRaw("SlowMo") > 0)
            RTActive = true;

        keyInput.slowmoPress = Input.GetKeyDown(KeyCode.LeftControl) || RTActive;

        bool shouldReleaseRT = false;
        if (RTActive && Input.GetAxisRaw("SlowMo") < 1)
        {
            shouldReleaseRT = true;
            RTActive = false;
        }

        keyInput.slowmoRelease = Input.GetKeyUp(KeyCode.LeftControl) || shouldReleaseRT;

        keyInput.leftFaceButtonPress = Input.GetButtonDown("LeftFaceButton");
        keyInput.rightFaceButtonPress = Input.GetButtonDown("RightFaceButton");
        keyInput.topFaceButtonPress = Input.GetButtonDown("TopFaceButton");
        keyInput.bottomFaceButtonPress = Input.GetButtonDown("BottomFaceButton");

    }
}

public struct KeyInput
{
    public Vector3 moveVec;
    public bool shootPress;
    public bool crawlPress;

    public bool slowmoPress;
    public bool slowmoRelease;

    public bool leftFaceButtonPress;
    public bool rightFaceButtonPress;
    public bool topFaceButtonPress;
    public bool bottomFaceButtonPress;
}