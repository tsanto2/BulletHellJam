using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IFireable))]
public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private BulletPattern pattern;
    [SerializeField] private Sound shootSound;

    public BulletPattern Pattern
    {
        get
        {
            return pattern;
        }
        set
        {
            pattern = value;
            UpdatePatternDetails();
        }
    }
    private IFireable shooter;
    private ObjectPool pool;

    private float bulletSpread;
    private float spawnRotation;
    private float currentBaseAngle;

    private Vector3[] positions;
    private GameObject[] bullets;

    private void Awake()
    {
        shooter = GetComponent<IFireable>();
        UpdatePatternDetails();
    }

    private void Start()
    {
        pool = ObjectPool.Instance;
    }

    public float GetBaseSpawnAngle()
    {
        if (pattern.baseSpread == 0f)
        {
            currentBaseAngle = pattern.baseDirection;
            return pattern.baseDirection;
        }

        float oscillationStep = Time.time * pattern.baseOscillationSpeed * (360f / pattern.baseSpread);
        float oscillationPoint = 0f;
        float oscillationAngle = 0f;

        switch (pattern.baseOscillationType)
        {
            case BulletPattern.SpawnOscillation.PingPong:
                {
                    oscillationPoint = Mathf.PingPong(oscillationStep, pattern.baseSpread);
                    oscillationAngle = oscillationPoint - pattern.spawnRadius;
                    break;
                }

            case BulletPattern.SpawnOscillation.Sine:
                {
                    oscillationPoint = Mathf.Sin(oscillationStep * Mathf.Deg2Rad);
                    oscillationAngle = oscillationPoint * pattern.spawnRadius;
                    break;
                }

            case BulletPattern.SpawnOscillation.Linear:
                {
                    oscillationPoint = oscillationStep % pattern.baseSpread;
                    oscillationAngle = oscillationPoint - pattern.spawnRadius;
                    break;
                }

            case BulletPattern.SpawnOscillation.Tan:
                {
                    oscillationPoint = Mathf.Tan(oscillationStep * Mathf.Deg2Rad);
                    oscillationAngle = oscillationPoint * pattern.spawnRadius;
                    break;
                }

            case BulletPattern.SpawnOscillation.Perlin:
                {
                    oscillationPoint = (Mathf.PerlinNoise(oscillationStep * Mathf.Deg2Rad, 0.7f) - 0.5f) * 2f;
                    oscillationAngle = oscillationPoint * pattern.spawnRadius;
                    break;
                }

            case BulletPattern.SpawnOscillation.Random:
                {
                    oscillationPoint = Random.Range(-pattern.spawnRadius, pattern.spawnRadius) * Mathf.Deg2Rad;
                    oscillationAngle = oscillationPoint * pattern.spawnRadius;
                    break;
                }
        }

        currentBaseAngle = pattern.baseDirection + oscillationAngle;
        return currentBaseAngle;
    }

    public Vector3[] GetSpawnPositions()
    {
        float baseAngle = GetBaseSpawnAngle();
        float minAngle;

        if (pattern.wrapAngles)
            minAngle = baseAngle - (pattern.baseSpread / 2f);
        else
            minAngle = baseAngle - (bulletSpread / 2f);

        for (int i = 0; i < pattern.bulletTotal; i++)
        {
            float deltaIncrement = pattern.bulletOffset * i;

            if (pattern.wrapAngles)
            {
                if (pattern.baseSpread > 0f)
                {
                    deltaIncrement += baseAngle;
                    deltaIncrement %= pattern.baseSpread;
                }
                else deltaIncrement = 0f;
            }

            float spawnerAngle = deltaIncrement + ((pattern.spawnerOscillationSpeed * Time.time)) % (Mathf.Max(1f, bulletSpread) + 1f);
            float angle = (minAngle + spawnerAngle) * Mathf.Deg2Rad;

            Vector3 bulletPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

            positions[i] = bulletPos;
        }

        return positions;
    }

    public void SpawnBullets(Vector3 startPosition)
    {
        positions = GetSpawnPositions();
        bullets = pool.GetObject(pattern.bulletPrefab, pattern.bulletTotal);
        shooter.ShootCooldown = pattern.shootDelay;
        
        if (shootSound != null)
            AudioManager.PlaySFX(shootSound, true);

        for (int i = 0; i < pattern.bulletTotal; i++)
        {
            float spawnDistance = Random.Range(pattern.bulletMinSpawnDistance, pattern.bulletMaxSpawnDistance);
            Vector3 spawnPos = startPosition + (positions[i] * spawnDistance);

            GameObject bullet = bullets[i];
            bullet.transform.position = spawnPos;

            float speedMultiplier = 1f + (pattern.incrementalSpeed * i);

            var comp = pool.componentCache[bullet];

            if (!pattern.mirrorShootAngles)
                comp.moveVec = positions[i] * speedMultiplier;
            else
                comp.moveVec = Quaternion.AngleAxis(currentBaseAngle + 180f, Vector3.forward) * Vector3.left * speedMultiplier;

            comp.rotationSpeed = pattern.bulletRotationSpeed;
            comp.moveSpeed = Random.Range(pattern.bulletMinBaseSpeed, pattern.bulletMaxBaseSpeed);
            comp.lifetime = Random.Range(pattern.bulletMinLifeTime, pattern.bulletMaxLifeTime);
        }
    }

    private void UpdatePatternDetails()
    {
        if (pattern == null)
            return;

        bulletSpread = pattern.bulletOffset * (pattern.bulletTotal - 1);
        positions = new Vector3[pattern.bulletTotal];
        bullets = new GameObject[pattern.bulletTotal];
    }
}
