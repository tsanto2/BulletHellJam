using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crawlSpeed;

    private InputController input;

    private void Awake()
    {
        input = GetComponent<InputController>();
    }

    private void FixedUpdate()
    {
        if (input.keyInput.crawlPress) Move(crawlSpeed);
        else Move(moveSpeed);
    }

    private void Move(float speed)
    {
        transform.position += input.keyInput.moveVec.normalized * speed * Time.fixedDeltaTime;
    }
}