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

    #region Fields

    [Header("Base Stats")]
    [Range(0f, 360f), SerializeField] public float baseDirection = 0f;
    [Range(0f, 360f), SerializeField] public float baseSpread = 90f;
    [Space]
    [Range(0f, 2f), SerializeField] public float bulletMinSpawnDistance = 1f;
    [Range(0f, 2f), SerializeField] public float bulletMaxSpawnDistance = 1f;
    [Space]
    [SerializeField] public SpawnOscillation baseOscillationType;
    [Range(0f, 720f), SerializeField] public float baseOscillationSpeed = 45f;

    public float spawnRadius;

    [Header("Bullet Stats")]
    [SerializeField] public GameObject bulletPrefab;
    [Space]
    [SerializeField] public float bulletMinLifeTime = 5f;
    [SerializeField] public float bulletMaxLifeTime = 5f;
    [Space]
    [SerializeField] public float bulletMinBaseSpeed = 3f;
    [SerializeField] public float bulletMaxBaseSpeed = 3f;
    [Space]
    [Range(-30f, 30f), SerializeField] public float bulletRotationSpeed;

    [Header("Spawner Stats")]
    [Range(0.02f, 5f)] public float shootDelay = 0.1f;
    [Range(1, 50), SerializeField] public int bulletTotal = 1;
    [Range(0f, 360), SerializeField] public float bulletOffset = 0f;
    [SerializeField] public bool wrapAngles;

    [SerializeField, Tooltip("If disabled, bullets will move directly away from spawn position")] 
    public bool mirrorShootAngles;

    [SerializeField] public float spawnerOscillationSpeed;
    
    [Header("Incremental Spawn Adjustments")]
    [Range(-0.3f, 0.3f), SerializeField] public float incrementalSpeed;
    [Range(-90f, 90f), SerializeField] public float incrementalRotation;    

    public float bulletSpread;
    public float spawnRotation;
    public float currentBaseAngle;

    #endregion

    public Vector3[] positions;
    public GameObject[] bullets;
    public ObjectPool pool;

    public void OnEnable()
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
            minAngle = baseAngle - (baseSpread / 2f);
        else
            minAngle = baseAngle - (bulletSpread / 2f);

        for (int i = 0; i < bulletTotal; i++)
        {
            float deltaIncrement = bulletOffset * i;
            
            if (wrapAngles)
            {
                if (baseSpread > 0f)
                {
                    deltaIncrement += baseAngle;
                    deltaIncrement %= baseSpread;
                }
                else deltaIncrement = 0f;
            }

            float spawnerAngle = (deltaIncrement + (spawnerOscillationSpeed * Time.time)) % (Mathf.Max(1f, bulletSpread) + 1f);
            float angle = (minAngle + spawnerAngle) * Mathf.Deg2Rad;

            Vector3 bulletPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

            positions[i] = bulletPos;
        }

        return positions;
    }

    public void SpawnBullets(Vector3 startPosition)
    {
        if (pool == null)
            pool = FindObjectOfType<ObjectPool>();

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
            comp.moveSpeed = Random.Range(bulletMinBaseSpeed, bulletMaxBaseSpeed);
            comp.lifetime = Random.Range(bulletMinLifeTime, bulletMaxLifeTime);
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

    public void OnValidate()
    {
        baseOscillationSpeed = Mathf.Floor(baseOscillationSpeed / 1f) * 1f;
        baseSpread = Mathf.Max(0f, Mathf.Floor(baseSpread / 5f) * 5f);
        spawnRadius = baseSpread / 2f;

        baseDirection = Mathf.Floor(baseDirection / 5f) * 5f;

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
