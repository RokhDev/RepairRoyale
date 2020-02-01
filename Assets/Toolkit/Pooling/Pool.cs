using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic.Pooling
{
	public class Pool {
		public GameObject pooledObject;
		private List<GameObject> availableObjects;
		private List<GameObject> spawnedObjects;

		public GameObject Spawn () {

			if (availableObjects == null)
				availableObjects = new List<GameObject>();
			if (spawnedObjects == null)
				spawnedObjects = new List<GameObject>();

			if (availableObjects.Count != 0)
			{
				GameObject objRef = availableObjects[0];
				availableObjects.Remove(objRef);
				spawnedObjects.Add(objRef);
				objRef.transform.position = Vector3.zero;
				objRef.transform.rotation = Quaternion.identity;
				return objRef;
			}
			else {
				GameObject objRef = MonoBehaviour.Instantiate(pooledObject, Vector3.zero, Quaternion.identity);
				spawnedObjects.Add(objRef);
				return objRef;
			}
		}

		public void Despawn (GameObject obj) {
			if (availableObjects == null)
				availableObjects = new List<GameObject>();
			if (spawnedObjects == null)
				spawnedObjects = new List<GameObject>();
			spawnedObjects.Remove(obj);
            if (!availableObjects.Contains(obj))
                availableObjects.Add(obj);
		}
	}
}