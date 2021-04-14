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
    }
}