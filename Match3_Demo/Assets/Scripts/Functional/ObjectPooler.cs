using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public DropColor.DropColorState colorCode;
        public GameObject dropPrefab;
        public int size;
    }

    public List<Pool> pools;

    public Dictionary<DropColor.DropColorState, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        poolDictionary = new Dictionary<DropColor.DropColorState, Queue<GameObject>>();
        CreatePools();
    }

    private void CreatePools()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.dropPrefab);
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.colorCode, objectPool);
        }
    }

    public GameObject SpawnFromPool(DropColor.DropColorState color, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(color))
        {
            Debug.LogWarning("Pool with color " + color + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[color].Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;        // todo delete if no need

        poolDictionary[color].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
