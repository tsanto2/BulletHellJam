using UnityEngine;

public class InputController : MonoBehaviour
{
    public KeyInput keyInput;

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        keyInput.moveVec = new Vector3(moveX, moveY, 0f);
        keyInput.crawlPress = Input.GetKey(KeyCode.LeftShift);

        keyInput.shootPress = Input.GetKey(KeyCode.Space);

        keyInput.slowmoPress = Input.GetKeyDown(KeyCode.LeftControl);
        keyInput.slowmoRelease = Input.GetKeyUp(KeyCode.LeftControl);
    }
}

public struct KeyInput
{
    public Vector3 moveVec;
    public bool shootPress;
    public bool crawlPress;

    public bool slowmoPress;
    public bool slowmoRelease;
}