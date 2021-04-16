using UnityEngine;
using System;

[RequireComponent(typeof(InputController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    #region Events
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerEnergyChange;
    public static event Action<bool> OnPlayerActivateSlowmo;
    #endregion
    
    [Header("Stats")]
    [SerializeField, Min(1)] private int healthMax;
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, healthMax);
            OnPlayerHealthChange?.Invoke(health);
        }
    }

    [SerializeField, Min(1)] private int energyMax;
    private int energy;
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = Mathf.Clamp(value, 0, energyMax);
            OnPlayerEnergyChange?.Invoke(energy);
        }
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crawlSpeed;

    [Header("Misc")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private InputController input;

    private void Awake()
    {
        input = GetComponent<InputController>();

        Health = healthMax;
        Energy = energyMax;
    }

    private void FixedUpdate()
    {
        if (input.keyInput.crawlPress)
            Move(crawlSpeed);
        else
            Move(moveSpeed);
    }

    private void Move(float speed)
    {
        transform.position += input.keyInput.moveVec.normalized * speed * Time.fixedDeltaTime;

        float clampX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampX, clampY);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health == 0)
            Die();
    }

    public void Die()
    {
        Debug.Log("Doug Dimmadome declares you dimma-dun-dead, son.");
    }
}