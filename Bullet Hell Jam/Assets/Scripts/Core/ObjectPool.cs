using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private List<PrespawnPool> prespawnPools;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    public Dictionary<GameObject, BulletController> componentCache = new Dictionary<GameObject, BulletController>();

    private List<GameObject> enemyBulletList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        PrepopulatePools();
        enemyBulletList.Clear();
    }

    private void OnEnable()
    {
        BossController.OnBossDeath += WipeAllEnemyBullets;
    }

    private void OnDisable()
    {
        BossController.OnBossDeath -= WipeAllEnemyBullets;
    }

    public GameObject GetObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count > 0)
            {
                GameObject _object = objectList.Dequeue();
                _object.SetActive(true);

                if (gameObject.layer == 9)
                    enemyBulletList.Add(_object);

                return _object;
            }
        }
        
        return CreateNewObject(gameObject); 
    }

    public GameObject[] GetObject(GameObject gameObject, int amount)
    {
        GameObject[] returnList = new GameObject[amount];

        for (int i = 0; i < amount; i++)
        {
            if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
            {
                if (objectList.Count > 0)
                {
                    GameObject _object = objectList.Dequeue();
                    _object.SetActive(true);
                    returnList[i] = _object;

                    if (gameObject.layer == 9)
                        enemyBulletList.Add(_object);

                    continue;
                }
            }
            else
            {
                GameObject _object = CreateNewObject(gameObject);
                returnList[i] = _object;
            }
        }

        return returnList;
    }

    private GameObject CreateNewObject(GameObject gameObject)
    {
        GameObject newObject = Instantiate(gameObject);
        newObject.name = gameObject.name;

        if (newObject.TryGetComponent<BulletController>(out var comp))
        {
            componentCache.Add(newObject, comp);
            comp.pool = this;

            if (newObject.layer == 9)
                enemyBulletList.Add(newObject);
        }

        return newObject;
    }

    public void ReturnObject(GameObject gameObject, bool removeFromList = true)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);

            if (gameObject.layer == 9 && removeFromList)
                enemyBulletList.Remove(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            objectPool.Add(gameObject.name, newObjectQueue);
        }

        gameObject.SetActive(false);
    }

    public void WipeAllEnemyBullets()
    {
        FindObjectOfType<GameManager>().AddPoints(enemyBulletList.Count * 5);

        foreach(var bullet in enemyBulletList)
            ReturnObject(bullet, false);

        enemyBulletList.Clear();
    }

    private void PrepopulatePools()
    {
        foreach (var pool in prespawnPools)
        {
            GameObject categoryObject = new GameObject();
            categoryObject.name = "[Pool] " + pool.prefab.name;
            categoryObject.transform.SetParent(transform);

            for (int i = 0; i < pool.amount; i++)
            {
                GameObject newObject = CreateNewObject(pool.prefab);
                newObject.transform.SetParent(categoryObject.transform);

                ReturnObject(newObject);
            }
        }
    }

    [Serializable]
    public class PrespawnPool
    {
        public GameObject prefab;
        public int amount;
    }
}
