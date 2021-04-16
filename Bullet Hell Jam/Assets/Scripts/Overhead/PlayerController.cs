using UnityEngine;
using System;

[RequireComponent(typeof(InputController))]
public class PlayerController : MonoBehaviour, IDamageable
{
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerEnergyChange;
    public static event Action<bool> OnPlayerActivateSlowmo;

    [Header("Stats")]
    [SerializeField] private int healthMax;
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

    [SerializeField] private int energyMax;
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
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [Space]
    [SerializeField, Range(0f, 1f)] private float slowmoScale = 0.5f;

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
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health == 0)
            Die();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}