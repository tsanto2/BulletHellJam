using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Spawner", menuName = "ScriptableObjects/Spawner")]
public class BulletPattern : ScriptableObject
{
    public enum SpawnOscillation
    {
        Sine,
        Linear,
        PingPong,
        Tan,
        Perlin,
        Random
    }

    [Header("Base Stats")]
    [Range(0f, 360f), SerializeField] private float baseDirection = 90f;
    [Range(0f, 360f), SerializeField] private float baseSpread = 90f;
    [Space]
    [Range(0f, 2f), SerializeField] private float bulletMinSpawnDistance = 1f;
    [Range(0f, 2f), SerializeField] private float bulletMaxSpawnDistance = 1f;
    [Space]
    [SerializeField] private SpawnOscillation baseOscillationType;
    [Range(1, 720), SerializeField] private float baseOscillationSpeed = 45f;

    private float spawnRadius;

    [Header("Bullet Stats")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletBaseSpeed;
    [Range(-30f, 30f), SerializeField] private float bulletRotationSpeed;

    [Header("Spawner Stats")]
    [Range(0.02f, 3f)] public float shootDelay = 0.1f;
    [Range(1, 30), SerializeField] private int bulletTotal = 2;
    [Range(0f, 360), SerializeField] private float bulletOffset = 0f;
    [SerializeField] private bool wrapAngles;

    [SerializeField, Tooltip("If disabled, bullets will move directly away from spawn position")] 
    private bool mirrorShootAngles;

    [SerializeField] private float spawnerOscillationSpeed;
    
    [Header("Incremental Spawn Adjustments")]
    [Range(-0.1f, 0.1f), SerializeField] private float incrementalSpeed;
    [Range(-90f, 90f), SerializeField] private float incrementalRotation;    

    private float bulletSpread;
    private float spawnRotation;
    private float currentBaseAngle;

    private Vector3[] positions;
    private GameObject[] bullets;
    private ObjectPool pool;

    private void OnEnable()
    {
        positions = new Vector3[bulletTotal];
        bullets = new GameObject[bulletTotal];
    }

    public float GetBaseSpawnAngle()
    {
        if (baseSpread == 0f)
        {
            currentBaseAngle = baseDirection;
            return baseDirection;
        }

        float oscillationStep = Time.time * baseOscillationSpeed * (360f / baseSpread);
        float oscillationPoint = 0f;
        float oscillationAngle = 0f;

        switch (baseOscillationType)
        {
            case SpawnOscillation.PingPong:
            {
                oscillationPoint = Mathf.PingPong(oscillationStep, baseSpread);
                oscillationAngle = oscillationPoint - spawnRadius;
                break;
            }

            case SpawnOscillation.Sine:
            {
                oscillationPoint = Mathf.Sin(oscillationStep * Mathf.Deg2Rad);
                oscillationAngle = oscillationPoint * spawnRadius;
                break;
            }

            case SpawnOscillation.Linear:
            {
                oscillationPoint = oscillationStep % baseSpread;
                oscillationAngle = oscillationPoint - spawnRadius;
                break;
            }

            case SpawnOscillation.Tan:
            {
                oscillationPoint = Mathf.Tan(oscillationStep * Mathf.Deg2Rad);
                oscillationAngle = oscillationPoint * spawnRadius;
                break;
            }

            case SpawnOscillation.Perlin:
            {
                oscillationPoint = (Mathf.PerlinNoise(oscillationStep * Mathf.Deg2Rad, 0.7f) - 0.5f) * 2f;
                oscillationAngle = oscillationPoint * spawnRadius;
                break;
            }

            case SpawnOscillation.Random:
            {
                oscillationPoint = Random.Range(-spawnRadius, spawnRadius) * Mathf.Deg2Rad;
                oscillationAngle = oscillationPoint * spawnRadius;
                break;
            }
        }

        currentBaseAngle = baseDirection + oscillationAngle;
        return currentBaseAngle;
    }

    public Vector3[] GetSpawnPositions()
    {
        float baseAngle = GetBaseSpawnAngle();
        float minAngle;

        if (wrapAngles)
            minAngle = baseDirection - (baseSpread / 2f);
        else
            minAngle = baseAngle - (bulletSpread / 2f);

        for (int i = 0; i < bulletTotal; i++)
        {
            float deltaIncrement = bulletOffset * i;
            
            if (wrapAngles)
            {
                deltaIncrement += baseAngle;
                deltaIncrement %= baseSpread;
            }

            float spawnerAngle = (deltaIncrement + (spawnerOscillationSpeed * Time.time)) % Mathf.Max(1f, bulletSpread);
            float angle = (minAngle + spawnerAngle) * Mathf.Deg2Rad;
            Vector3 bulletPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

            positions[i] = bulletPos;
        }

        return positions;
    }

    public void SpawnBullets(Vector3 startPosition, IFireable fireable)
    {
        if (fireable == null)
            return;

        if (pool == null)
            pool = FindObjectOfType<ObjectPool>();

        fireable.ShootCooldown = shootDelay;

        positions = GetSpawnPositions();
        bullets = pool.GetObject(bulletPrefab, bulletTotal);

        for (int i = 0; i < bulletTotal; i++)
        {
            float spawnDistance = Random.Range(bulletMinSpawnDistance, bulletMaxSpawnDistance);
            Vector3 spawnPos = startPosition + (positions[i] * spawnDistance);

            GameObject bullet = bullets[i];
            bullet.transform.position = spawnPos;

            float speedMultiplier = 1f + (incrementalSpeed * i);

            var comp = pool.componentCache[bullet];

            if (!mirrorShootAngles)
                comp.moveVec = positions[i] * speedMultiplier;
            else
                comp.moveVec = Quaternion.AngleAxis(currentBaseAngle + 180f, Vector3.forward) * Vector3.left * speedMultiplier;

            comp.rotationSpeed = bulletRotationSpeed;
            comp.moveSpeed = bulletBaseSpeed;
            comp.lifetime = bulletLifeTime;
        }
    }

    public void DrawSpread(Vector3 startingPosition)
    {
        float startAngle = baseDirection + Time.time;

        var minAngle = Quaternion.AngleAxis(startAngle - spawnRadius, Vector3.forward) * Vector3.left;
        var maxAngle = Quaternion.AngleAxis(startAngle + spawnRadius, Vector3.forward) * Vector3.left;
        Debug.DrawLine(startingPosition, startingPosition + minAngle, Color.red);
        Debug.DrawLine(startingPosition, startingPosition + maxAngle, Color.red);

        var spawnAngle = GetBaseSpawnAngle();

        for (int i = 0; i < bulletTotal; i++)
        {
            var minBulletAngle = Quaternion.AngleAxis(spawnAngle - (bulletSpread / 2f) + (bulletOffset * i), Vector3.forward) * Vector3.left;
            Debug.DrawLine(startingPosition, startingPosition + minBulletAngle, Color.green);
        }
    }

    private void OnValidate()
    {
        baseOscillationSpeed = Mathf.Floor(baseOscillationSpeed / 1f) * 1f;
        baseSpread = Mathf.Max(0f, Mathf.Floor(baseSpread / 5f) * 5f);
        spawnRadius = baseSpread / 2f;

        baseDirection = Mathf.Floor(baseDirection / 15f) * 15f;

        bulletRotationSpeed = Mathf.Floor(bulletRotationSpeed);
        bulletOffset = Mathf.Floor(bulletOffset / 1f) * 1f;
        bulletSpread = bulletOffset * (bulletTotal - 1);

        positions = new Vector3[bulletTotal];
        bullets = new GameObject[bulletTotal];
        shootDelay = Mathf.Floor(shootDelay / 0.01f) * 0.01f;

        bulletMinSpawnDistance = Mathf.Min(bulletMinSpawnDistance, bulletMaxSpawnDistance);
        bulletMaxSpawnDistance = Mathf.Max(bulletMinSpawnDistance, bulletMaxSpawnDistance);
    }
}
