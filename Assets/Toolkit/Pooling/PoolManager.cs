using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic.Pooling
{
    public class PoolManager {

        private static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

        public static GameObject Spawn(GameObject original, Vector3 position, Quaternion rotation, string poolName)
        {
            if (!pools.ContainsKey(poolName))
            {
                pools.Add(poolName, new Pool());
                pools[poolName].pooledObject = original;
            }
            GameObject objRef = pools[poolName].Spawn();
            objRef.transform.position = position;
            objRef.transform.rotation = rotation;
            objRef.SetActive(true);
            return objRef;
        }

        public static GameObject Spawn(GameObject original, string poolName)
        {
            if (!pools.ContainsKey(poolName))
            {
                pools.Add(poolName, new Pool());
                pools[poolName].pooledObject = original;
            }
            GameObject objRef = pools[poolName].Spawn();
            objRef.SetActive(true);
            return objRef;
        }

        public static void DeSpawn(GameObject instance, string poolName)
		{
            if (!pools.ContainsKey(poolName))
            {
                return;
            }
            pools[poolName].Despawn(instance);
            instance.SetActive(false);
		}
	}
}